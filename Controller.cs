using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Controller : MonoBehaviour {

	public float jumpForce = 400f;
	public float crouchSpeed = .36f;
	public float moveSmooth = 0.05f;
	public bool airControl = false;
	public LayerMask whatIsGround;
	public Transform ceil;
	public Transform floor;
	public Collider2D disableCollider;

	float GroundedRadius = 0.2f;
	public bool grounded;
	float ceilRadius = 0.2f;
	Rigidbody2D rb;
	bool facingRight = true;
	Vector3 velocity = Vector3.zero;

	public int jumpCount = 1;

	[Header("Events")]
	[Space]

	public UnityEvent onLandEvent;
	[System.Serializable]
	public class BoolEvent : UnityEvent<bool>{ }

	public BoolEvent OnCrouchevent;
	bool crouching = false;

	public static Controller Instance;

	// Use this for initialization
	void Awake () {
		Instance = this;


		rb = GetComponent<Rigidbody2D>();

		if(onLandEvent == null)
		{
			onLandEvent = new UnityEvent();
		}
		if(OnCrouchevent == null)
		{
			OnCrouchevent = new BoolEvent();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		bool wasGrounded = grounded;
		grounded = false;

		//Debug.Log(Physics2D.OverlapCircle(ceil.position, ceilRadius, whatIsGround));

		Collider2D[] colliders = Physics2D.OverlapCircleAll(floor.position, GroundedRadius, whatIsGround);
		for(int i = 0; i<colliders.Length; i++)
		{
			if(colliders[i].gameObject != gameObject)
			{
				grounded = true;
				if(!wasGrounded)
				{
					onLandEvent.Invoke();
				}
			}
		}

		if(grounded)
		{
			jumpCount = 1;
		}
	}

	public void Move(float move, bool crouch, bool jump, bool secondJump)
	{
		if(!crouch)
		{
			if(Physics2D.OverlapCircle(ceil.position, ceilRadius, whatIsGround))
			{
				crouch = true;
			}
		}

		if(grounded || airControl)
		{
			if(crouch)
			{
				if(!crouching)
				{
					crouching = true;
					OnCrouchevent.Invoke(true);
				}

				move *= crouchSpeed;

				if(disableCollider!=null)
				{
					disableCollider.enabled = false;
				}
			}
			else
			{
				if(disableCollider!=null)
				{
					disableCollider.enabled = true;
				}

				if(crouching)
				{
					crouching = false;
					OnCrouchevent.Invoke(false);
				}
			}

			Vector3 targetVelocity = new Vector2(move*10f, rb.velocity.y);
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, moveSmooth);


			if(move>0 && !facingRight)
			{
				Flip();
			}
			else if(move<0 && facingRight)
			{
				Flip();
			}
		}

		if(grounded && jump)
		{
			grounded = false;
			rb.AddForce(new Vector2(0, jumpForce));
		}

		if(!grounded && secondJump && jumpCount>0)
		{
			grounded = false;
			rb.AddForce(new Vector2(0, jumpForce*.8f));
			jumpCount--;
		}

	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 localS = transform.localScale;
		localS.x *= -1;
		transform.localScale = localS;
	}

}
