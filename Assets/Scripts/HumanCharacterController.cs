using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class HumanCharacterController : MonoBehaviour {

	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	[SerializeField]
	private Vector3 _velocity;

	public Vector2 attackVelocity;

	[System.NonSerialized]
	public bool attacking = false;

	public ParticleSystem smashParticles;

	void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
//		if( _controller.isGrounded ) {
//			_velocity.y = 0;
//		}

		if(!attacking) {
//			if( _controller.isGrounded ) {
			Debug.DrawRay(transform.position, Vector3.up, Color.green, 0.5f);
			bool melee = InputController.input.Melee.WasPressed;
			Vector3	moveDir = InputController.input.Move;

			if(melee &&  _controller.isGrounded) {
				_animator.Play( Animator.StringToHash( "Attack" ) );
			}

			if( moveDir.x > 0f )
			{
				normalizedHorizontalSpeed = 1;
//				if( transform.localScale.x < 0f )
//					transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

				if( _controller.isGrounded ) {
					_animator.Play( Animator.StringToHash( "Walk" ) );
					if( transform.localScale.x < 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				}
			}
			else if( moveDir.x < 0f )
			{
				normalizedHorizontalSpeed = -1;
//				if( transform.localScale.x > 0f )
//					transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

				if( _controller.isGrounded ) {
					_animator.Play( Animator.StringToHash( "Walk" ) );
					if( transform.localScale.x > 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				}
			}
			else if( _controller.isGrounded )
			{
				normalizedHorizontalSpeed = 0;

//					if( _controller.isGrounded )
					_animator.Play( Animator.StringToHash( "Idle" ) );
			}
//			}
		}


//		// we can only jump whilst grounded
//		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
//		{
//			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
//			_animator.Play( Animator.StringToHash( "Jump" ) );
//		}

		if( _controller.isGrounded ) //something with velocity here?
		{

		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
//		if(_velocity.x < 0.03f)
//			_velocity.x = 0f;

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
//		if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
//		{
//			_velocity.y *= 3f;
//			_controller.ignoreOneWayPlatformsThisFrame = true;
//		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

	public void Launch() {
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.Find("SmashPosition").position, 1f);
		for(int i = 0; i < cols.Length; i++) {
			if(cols[i].GetComponent<CrystalDevice>() != null) {
				cols[i].GetComponent<CrystalDevice>().Smash();
			}
		}


		_velocity = new Vector3(attackVelocity.x * transform.localScale.x, attackVelocity.y, 0f);
		Destroy(Instantiate(smashParticles, transform.Find("SmashPosition").position, smashParticles.transform.rotation), 1.2f);
//		smashParticles.Play(true);
//		attacking = false;
	}

}
