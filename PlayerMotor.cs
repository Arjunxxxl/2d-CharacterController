using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

	public Controller contol;
	float h;

	public float runSpeed = 400f;
	public bool jump;
	public bool secondJump;
	public int jumpCount = 0;
	public bool crouch;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
		{	
			if(jump || secondJump)
			{
				return;
			}

			jump = true;
			anim.SetBool("jump", true);

			
			if(jumpCount==1)
			{
				secondJump = true;
				anim.SetBool("jump", false);
				anim.SetTrigger("second");
			}

			jumpCount++;
			Debug.Log(secondJump);
		}


		if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad0))
		{
			crouch = true;
			anim.SetBool("crouch", true);
		}

		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
		{
			jump = false;
		}

		if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.Keypad0))
		{
			crouch = false;
		}

		h = Input.GetAxisRaw("Horizontal");

		anim.SetFloat("Speed", Mathf.Abs(h));

	}

	private void FixedUpdate() {
		contol.Move(h, crouch, jump, secondJump);
		if(jump)
		{jump = false; }
		
	}

	public void OnCrouchEvent(bool b)
	{
		anim.SetBool("crouch", b);
	}

	public void OnLandEvent()
	{
		//anim.SetBool("jump", false);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "ground")
		{
			anim.SetBool("jump", false);
			secondJump = false;
			jumpCount = 0;
		}
	}

}
