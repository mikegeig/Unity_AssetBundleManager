using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour 
{
	public GameObject effect;


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag != "Player")	return;

		Instantiate (effect, transform.position, Quaternion.Euler (0f, 180f, 0f));
		Destroy (gameObject);
	}
}
