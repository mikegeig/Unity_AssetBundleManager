using UnityEngine;
using System.Collections;
using System;

public class LoadScene : MonoBehaviour 
{
	[SerializeField] string sceneBundle;
	[SerializeField] string sceneName;
	[SerializeField] string optionalVariantBundle;
	[SerializeField] string optionalVariantName;
	
	IEnumerator Start () 
	{
		if (!string.IsNullOrEmpty (optionalVariantBundle) && !string.IsNullOrEmpty (optionalVariantName))
			BundleManager.instance.RegisterVariant (optionalVariantBundle, optionalVariantName);

		while (!BundleManager.instance.isReady)
			yield return null;

		BundleManager.instance.LoadBundle (sceneBundle);

		while (!BundleManager.instance.IsBundleLoaded(sceneBundle))
			yield return null;

		Application.LoadLevel (sceneName);
	}
}
