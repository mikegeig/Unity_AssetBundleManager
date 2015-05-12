using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class AssetBundleMenu
{
	[MenuItem("AssetBundles/Clear Cache", false, 10)]
	static void ClearCache()
	{
		Caching.CleanCache ();
	}

	[MenuItem("AssetBundles/Build for PC")]
	static void TogglePCBuild ()
	{
		EditorPrefs.SetBool("buildPC", !EditorPrefs.GetBool("buildPC", false));
	}

	[MenuItem("AssetBundles/Build for PC", true)]
	static bool TogglePCBuildValidate ()
	{
		Menu.SetChecked("AssetBundles/Build for PC", EditorPrefs.GetBool("buildPC", false));
		return true;
	}

	[MenuItem("AssetBundles/Build for OSX")]
	static void ToggleOSXBuild ()
	{
		EditorPrefs.SetBool("buildOSX", !EditorPrefs.GetBool("buildOSX", false));
	}

	[MenuItem("AssetBundles/Build for OSX", true)]
	static bool ToggleOSXBuildValidate ()
	{
		Menu.SetChecked("AssetBundles/Build for OSX", EditorPrefs.GetBool("buildOSX", false));
		return true;
	}

	[MenuItem("AssetBundles/Build for Web")]
	static void ToggleWebBuild ()
	{
		EditorPrefs.SetBool("buildWeb", !EditorPrefs.GetBool("buildWeb", false));
	}

	[MenuItem("AssetBundles/Build for Web", true)]
	static bool ToggleWebBuildValidate ()
	{
		Menu.SetChecked("AssetBundles/Build for Web", EditorPrefs.GetBool("buildWeb", false));
		return true;
	}

	[MenuItem("AssetBundles/Build for iOS")]
	static void ToggleiOSBuild ()
	{
		EditorPrefs.SetBool("buildiOS", !EditorPrefs.GetBool("buildiOS", false));
	}

	[MenuItem("AssetBundles/Build for iOS", true)]
	static bool ToggleiOSBuildValidate ()
	{
		Menu.SetChecked("AssetBundles/Build for iOS", EditorPrefs.GetBool("buildiOS", false));
		return true;
	}

	[MenuItem("AssetBundles/Build for Android")]
	static void ToggleAndroidBuild ()
	{
		EditorPrefs.SetBool("buildAndroid", !EditorPrefs.GetBool("buildAndroid", false));
	}

	[MenuItem("AssetBundles/Build for Android", true)]
	static bool ToggleAndroidBuildValidate ()
	{
		Menu.SetChecked("AssetBundles/Build for Android", EditorPrefs.GetBool("buildAndroid", false));
		return true;
	}

	[MenuItem("AssetBundles/Build for Windows Phone 8")]
	static void ToggleWP8Build ()
	{
		EditorPrefs.SetBool("buildWP8", !EditorPrefs.GetBool("buildWP8", false));
	}

	[MenuItem("AssetBundles/Build for Windows Phone 8", true)]
	static bool ToggleWP8BuildValidate ()
	{
		Menu.SetChecked("AssetBundles/Build for Windows Phone 8", EditorPrefs.GetBool("buildWP8", false));
		return true;
	}

	[MenuItem("AssetBundles/Build Asset Bundles")]
	static void BuildAssetBundles() 
	{
		// Bring up save panel
		string path = EditorUtility.SaveFolderPanel ("Save Bundle", EditorPrefs.GetString("BundleManagerLastDir", ""), "");
		if (path.Length != 0) 
		{	
			EditorPrefs.SetString ("BundleManagerLastDir", path);

			if(EditorPrefs.GetBool("buildPC", false))	
				BuildBundle(path + "/PC", BuildTarget.StandaloneWindows);

			if(EditorPrefs.GetBool("buildOSX", false))
				BuildBundle(path + "/OSX", BuildTarget.StandaloneOSXUniversal);

			if(EditorPrefs.GetBool("buildWeb", false))
				BuildBundle(path + "/Web", BuildTarget.WebPlayer);

			if(EditorPrefs.GetBool("buildiOS", false))
				BuildBundle(path + "/iOS", BuildTarget.iOS);

			if(EditorPrefs.GetBool("buildAndroid", false))
				BuildBundle(path + "/Android", BuildTarget.Android);

			if(EditorPrefs.GetBool("buildWP8", false))
				BuildBundle(path + "/WP8", BuildTarget.WP8Player);
		}
	}

	static void BuildBundle(string path, BuildTarget target)
	{
		if (!Directory.Exists (path))
			Directory.CreateDirectory (path);

		BuildPipeline.BuildAssetBundles (path, BuildAssetBundleOptions.None, target);
	}
}








