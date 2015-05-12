using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BundleManager : MonoBehaviour 
{
	public static BundleManager instance;
	public string[] bundleVariants;

	[SerializeField] string pathToBundles;

	Dictionary<string, AssetBundle> bundles;
	AssetBundleManifest manifest = null;

	public bool isReady
	{
		get
		{
			return !object.ReferenceEquals(manifest, null);
		}
	}


	void Awake()
	{
		if (object.ReferenceEquals (instance, null)) 
		{
			instance = this;
		}
		else if (!object.ReferenceEquals (instance, this))
		{
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		string platform = "";

		#if UNITY_IOS
				platform = "iOS";
		#elif UNITY_ANDROID
				platform = "Android";
		#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				platform = "PC";
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
				platform = "OSX";
		#elif UNITY_WEBPLAYER
				platform = "Web";
		#elif UNITY_WP8
				platform = "WP8";
		#else
				platform = "error";
				Debug.Log("unsupported platform");
		#endif

		pathToBundles += platform + "/";
		bundles = new Dictionary<string, AssetBundle> ();
		StartCoroutine (LoadManifest(platform));
	}

	IEnumerator LoadManifest (string platform) 
	{
		Debug.Log( "Loading Manifest");
		
		using(WWW www = new WWW(pathToBundles + platform))
		{
			yield return www;
			if(!string.IsNullOrEmpty(www.error))
			{
				Debug.Log(www.error);
				return false;
			}

			manifest = (AssetBundleManifest)www.assetBundle.LoadAsset("AssetBundleManifest", typeof(AssetBundleManifest));
			yield return null;
			www.assetBundle.Unload(false);
		} 

		if (!isReady)
			Debug.Log ("There was an error loading manifest");
		else
			Debug.Log ("Manifest loaded successfully");
	}

	public bool IsBundleLoaded(string bundleName)
	{
		return bundles.ContainsKey (bundleName);
	}

	public Object GetAssetFromBundle(string bundleName, string assetName)
	{
		if (!IsBundleLoaded (bundleName))
			return null;

		return bundles [bundleName].LoadAsset (assetName);
	}

	public bool LoadBundle(string bundleName)
	{
		if (IsBundleLoaded(bundleName))
			return true;

		StartCoroutine(LoadBundleCoroutine(bundleName));
		return false;
	}

	IEnumerator LoadBundleCoroutine(string bundleName)
	{
		if (IsBundleLoaded (bundleName))
			yield break;

		string[] dependencies = manifest.GetAllDependencies (bundleName);
		for (int i = 0; i < dependencies.Length; i++)
			yield return StartCoroutine (LoadBundleCoroutine (dependencies [i]));

		bundleName = RemapVariantName (bundleName);
		string url = pathToBundles + bundleName;
		Debug.Log ("Beginning to load " + bundleName + " / " + manifest.GetAssetBundleHash(bundleName));

		using(WWW www = WWW.LoadFromCacheOrDownload(url, manifest.GetAssetBundleHash(bundleName)))
		{
			yield return www;
			if(!string.IsNullOrEmpty(www.error))
			{
				Debug.Log(www.error);
				return false;
			}

			bundles.Add(bundleName, www.assetBundle);
		}
		Debug.Log ("Finished loading " + bundleName);
	}

	void OnDisable()
	{
		Debug.Log ("Unloading Bundles");

		foreach(KeyValuePair<string, AssetBundle> entry in bundles)
			entry.Value.Unload(false);
		bundles.Clear ();

		Debug.Log ("Bundles Unloaded");
	}
	
	string RemapVariantName(string assetBundleName)
	{
		string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

		if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0 )
			return assetBundleName;
		
		string[] splitBundleName = assetBundleName.Split('.');
		string[] candidateBundles = System.Array.FindAll (bundlesWithVariant, element => element.StartsWith(splitBundleName [0]));

		int index = -1;
		for(int i = 0; i < bundleVariants.Length; i++)
		{
			index = System.Array.IndexOf(candidateBundles, splitBundleName[0] + "." + bundleVariants[i]);
			if(index != -1)
				return candidateBundles[index];
		}

		return assetBundleName;
	}
}




