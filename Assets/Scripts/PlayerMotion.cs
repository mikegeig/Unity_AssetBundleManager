using UnityEngine;
using System.Collections;

public class PlayerMotion: MonoBehaviour
{				
	public float jumpForce = 400f;			
	public float movementSpeed = 5f;
	public bool facingRight = true;
	
	bool jump = false;	
	bool grounded = false;
	float move;

	Animator anim;
	Rigidbody2D rigbody2D;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rigbody2D = GetComponent<Rigidbody2D> ();
	}
	
	void OnCollisionEnter2D(Collision2D hit)
	{
		if(hit.gameObject.tag == "Ground")
			grounded = true;
	}
	
	void Update()
	{
		
#if UNITY_STANDALONE || UNITY_EDITOR
		move = Input.GetAxis("Horizontal");
		move *= movementSpeed;

		bool shouldJump = Input.GetButtonDown("Jump");

#elif UNITY_IOS || UNITY_ANDROID
		move = touchPad.GetDirection ().x;
		move *= movementSpeed;

		bool shouldJump = touchButton.GetButtonDown();
#endif

		if((facingRight && move < 0) ||
		   (!facingRight && move > 0))
			Flip ();
		
		if(shouldJump && grounded == true)
		{		
			jump = true;
			grounded = false;
			anim.SetTrigger("Jump");
		}

		anim.SetFloat ("Speed", Mathf.Abs (move));
		anim.SetBool("Grounded", grounded);
	}
	
	void FixedUpdate ()
	{
		rigbody2D.velocity = new Vector2( move , GetComponent<Rigidbody2D>().velocity.y  );
		
		if(jump == true)
		{
			rigbody2D.AddForce(new Vector2(0f, jumpForce));
			
			jump = false;
		}
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
