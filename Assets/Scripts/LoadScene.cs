using UnityEngine;
using System.Collections;
using System;

public class LoadScene : MonoBehaviour 
{
	public string sceneBundle;
	public string sceneName;
	
	IEnumerator Start () 
	{
		while (!BundleManager.instance.isReady)
			yield return null;

		BundleManager.instance.LoadBundle (sceneBundle);

		while (!BundleManager.instance.IsBundleLoaded(sceneBundle))
			yield return null;

		Application.LoadLevel (sceneName);
	}
}
