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

	[SerializeField]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	public Vector2 attackVelocity;

	[System.NonSerialized]
	public bool attacking = false;

	public ParticleSystem smashParticles;

	public Transform startAngle;
	public Transform endAndgle;

	public Transform hammerObj;
	float throwTimer = 0f;

	bool hasHammer = true;

	[SerializeField]
	BoxCollider2D hammerCollider;
	[SerializeField]
	BoxCollider2D normalCollider;
	[SerializeField]
	BoxCollider2D hammerTriggerCollider;
	[SerializeField]
	BoxCollider2D normalTriggerCollider;

	[Header("Sounds")]
	[Space]
	AudioSource hammerSounds;
	public AudioClip swingSound;
	public AudioClip launchSound;

	[Space]
	AudioSource playerSounds;
	public AudioClip jumpSound;
	public AudioClip walkSound;

	[Space]
	AudioSource altPlayerSounds;
	public AudioClip dragSound;

	[Space]
	AudioSource hurtPlayerSounds;
	public AudioClip doorSound;
	public AudioClip[] hurtClips;

	void Awake()
	{
		hammerSounds = gameObject.AddComponent<AudioSource>();
		playerSounds = gameObject.AddComponent<AudioSource>();
		altPlayerSounds = gameObject.AddComponent<AudioSource>();
		hurtPlayerSounds = gameObject.AddComponent<AudioSource>();

		_animator = GetComponentInChildren<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerStayEvent += onTriggerStayEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		hammerObj.transform.parent = null;
	}

	void Start() {
		hammerCollider.enabled = true;
		hammerTriggerCollider.enabled = true;
		normalCollider.enabled = false;
		normalTriggerCollider.enabled = false;
		_controller.boxCollider = hammerCollider;
		_controller.recalculateDistanceBetweenRays();
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
//		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
		if(col.gameObject.name == "Door") {
			col.gameObject.GetComponent<Animator>().Play("DoorOpen");
			hurtPlayerSounds.clip = doorSound;
			hurtPlayerSounds.Play();
			Invoke("GhettoNextScene", 1.2f);
		}
//		if(col.gameObject.name == "HammerThrow") {
//			if(throwTimer > 0f)
//				return;
//			col.gameObject.SetActive(false);
//			transform.Find("Graphics").Find("Hammer").gameObject.SetActive(true);
//			hasHammer = true;
//		}
	}

	void onTriggerStayEvent( Collider2D col )
	{
		if(col.gameObject.name == "HammerThrow") {
			if(hasHammer)
				return;
//			Debug.LogError("picking up");
			if(throwTimer > 0f)
				return;
			col.gameObject.SetActive(false);
			transform.Find("Graphics").Find("Hammer").gameObject.SetActive(true);
			hasHammer = true;
			hammerCollider.enabled = true;
			normalCollider.enabled = false;
			hammerTriggerCollider.enabled = true;
			normalTriggerCollider.enabled = false;
			_controller.boxCollider = hammerCollider;
			_controller.recalculateDistanceBetweenRays();
			transform.Find("Graphics").transform.localPosition = new Vector3(0.3f, 0f, 0f);
			transform.position -= new Vector3(0.469f, 0f, 0f) * Mathf.Sign(transform.localScale.x);
			transform.Find("Graphics").Find("CameraTarget").localPosition = new Vector3(-0.29f, 0.344f, 0f);
		}
	}

	void GhettoNextScene() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
	}


	void onTriggerExitEvent( Collider2D col )
	{
//		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		if(GetComponent<HealthHandler>().invulnerableTimer <= GetComponent<HealthHandler>().invulnerableTime/2f) {
		if(throwTimer > 0f)
			throwTimer -= Time.deltaTime;
		
		transform.Find("DustTrail_0").GetComponent<SpriteRenderer>().enabled = false; //Eh. Not efficient.

		if(attacking) {
			bool release = InputController.input.Release.WasPressed;
//			_animator.GetCurrentAnimatorStateInfo(
		} else if(!attacking) {
//			if( _controller.isGrounded ) {
			bool melee = InputController.input.Melee.WasPressed;
			Vector3	moveDir = InputController.input.Move;

			if(hasHammer && melee &&  _controller.isGrounded) {
				_animator.Play( Animator.StringToHash( "Attack" ) );
				hammerSounds.clip = swingSound;
				hammerSounds.Play();
			}

			if( moveDir.x > 0f )
			{
				normalizedHorizontalSpeed = 1;

				if( _controller.isGrounded ) {
					if(playerSounds.clip != walkSound || !playerSounds.isPlaying) {
						playerSounds.clip = walkSound;
						playerSounds.Play();
					}
					if(hasHammer) {
						if(altPlayerSounds.clip != dragSound || !altPlayerSounds.isPlaying) {
							altPlayerSounds.clip = dragSound;
							altPlayerSounds.Play();
						}

						transform.Find("DustTrail_0").GetComponent<SpriteRenderer>().enabled = true;
					}

					if(hasHammer)
						_animator.Play( Animator.StringToHash( "Walk" ) );
					else
						_animator.Play( Animator.StringToHash( "WalkNoHammer" ) );
					if( transform.localScale.x < 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				}
			}
			else if( moveDir.x < 0f )
			{
				normalizedHorizontalSpeed = -1;

				if( _controller.isGrounded ) {
					if(playerSounds.clip != walkSound || !playerSounds.isPlaying) {
						playerSounds.clip = walkSound;
						playerSounds.Play();
					}
					if(hasHammer) {
						if(altPlayerSounds.clip != dragSound || !altPlayerSounds.isPlaying) {
							altPlayerSounds.clip = dragSound;
							altPlayerSounds.Play();
						}

						transform.Find("DustTrail_0").GetComponent<SpriteRenderer>().enabled = true;
					}

					if(hasHammer)
						_animator.Play( Animator.StringToHash( "Walk" ) );
					else
						_animator.Play( Animator.StringToHash( "WalkNoHammer" ) );
					if( transform.localScale.x > 0f )
						transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				}
			}
			else if( _controller.isGrounded )
			{
				normalizedHorizontalSpeed = 0;

//					if( _controller.isGrounded )
				if(hasHammer)
					_animator.Play( Animator.StringToHash( "Idle" ) );
				else
					_animator.Play( Animator.StringToHash( "IdleNoHammer" ) );
			}
		}
		} else {
			normalizedHorizontalSpeed = 0;
		}


//		// we can only jump whilst grounded
//		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
//		{
//			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
//			_animator.Play( Animator.StringToHash( "Jump" ) );
//		}

		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

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
		if(!attacking)
			return;
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.Find("SmashPosition").position, 1f);
		for(int i = 0; i < cols.Length; i++) {
			if(cols[i].GetComponent<CrystalDevice>() != null) {
				cols[i].GetComponent<CrystalDevice>().Smash();
			}
			if(cols[i].GetComponent<SimpleEnemy>() != null) {
				cols[i].GetComponent<HealthHandler>().ApplyDamage(1);
			}
		}

//		Destroy(Instantiate(smashParticles, transform.Find("SmashPosition").position, smashParticles.transform.rotation), 1.2f); //idk this doesnt work yet

		hammerSounds.clip = launchSound;
		hammerSounds.Play();

		playerSounds.clip = jumpSound;
		playerSounds.Play();

//		Collider2D[] col

		_velocity = new Vector3(attackVelocity.x * transform.localScale.x, attackVelocity.y, 0f);
		GameObject smashParticleObj = (GameObject)Instantiate(smashParticles.gameObject, transform.Find("SmashPosition").position, smashParticles.transform.rotation);
		Destroy(smashParticleObj, 1.2f);
	}
		
	public void KnockBack() {
		attacking = false;
		_animator.Play( Animator.StringToHash( "Damage" ) );
		_velocity = new Vector3(-5f * transform.localScale.x, 5f, 0f);

		hurtPlayerSounds.clip = hurtClips[Random.Range(0, hurtClips.Length)];
		hurtPlayerSounds.Play();
	}

	public void Release(float normalizedTime) {
		if(normalizedTime < 0.2f || normalizedTime > 0.8f)
			return;
		attacking = false;
//		_animator.GetCurrentAnimatorStateInfo(0).normalizedTime = 1f;
		_animator.Play( Animator.StringToHash( "Idle" ) );
		Vector3 hammerAngle = Vector3.Lerp(startAngle.up, endAndgle.up, normalizedTime);
		Debug.DrawRay(transform.position, hammerAngle, Color.blue, 1.5f);
		hammerObj.gameObject.SetActive(true);
		hammerObj.up = hammerAngle;
		hammerObj.transform.position = transform.position + new Vector3(0.323f, -0.17f, 0f);
		hammerObj.GetComponent<Rigidbody2D>().velocity = hammerAngle*10f;
		transform.Find("Graphics").Find("Hammer").gameObject.SetActive(false);
		throwTimer = 0.5f;
		hasHammer = false;
		hammerCollider.enabled = false;
		hammerTriggerCollider.enabled = false;
		normalCollider.enabled = true;
		normalTriggerCollider.enabled = true;
		_controller.boxCollider = normalCollider;
		_controller.recalculateDistanceBetweenRays();
		transform.Find("Graphics").transform.localPosition -= new Vector3(0.469f, 0f, 0f);
		transform.position += new Vector3(0.469f, 0f, 0f) * Mathf.Sign(transform.localScale.x);
		transform.Find("Graphics").Find("CameraTarget").localPosition = new Vector3(0.169f, 0.344f, 0f);
	}

}
