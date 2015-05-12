using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour 
{
	public float timeToLive = 1f;


	void Start () 
	{
		Destroy (gameObject, timeToLive);
	}
}
