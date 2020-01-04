using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{
	#region basic
	private Rigidbody2D rigidBody2D;
	private Animator animator;
	#endregion

	#region movement
	public Transform GroundCheck;
	public float GroundCheckRadius = 0.15f;
	public LayerMask WhatIsGround;

	public float Speed = 10f;
	public float JumpForce = 10f;
	private bool IsOnGround = false;

	private bool isFacingRight = true;
	#endregion

	#region AI
	public float weaponRange;
	private Transform target;
	private float direction;
	private float currentPos;
	#endregion

	#region combat
	public Weapon Weapon;
	public bool isDead = false;
	private bool isMaskOn = true;
	private Rigidbody2D Mask;
	private float timer = 0f;
	private float timeToDamage = 0.2f;
	private float cooldownTimer = 0f;
	private float cooldownTime = 0.2f;
	#endregion

	#region audio
	public AudioClip PunchClip;
	private AudioSource audioSource;
	#endregion

	void Start()
	{
		Mask = GetComponentInChildren<MaskController>()?.GetComponent<Rigidbody2D>();
		if (Mask == null)
			isMaskOn = false;

		Weapon.Owner = this.transform;

		audioSource = GetComponent<AudioSource>();
		rigidBody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		target = GameObject.FindWithTag("Player").transform;

		Physics2D.IgnoreCollision(
			Weapon.GetComponent<Collider2D>(),
			target.GetComponent<PlayerCombatController>().Weapon.GetComponent<Collider2D>()
			);

		if(isMaskOn)
		Physics2D.IgnoreCollision(
			Mask.GetComponent<Collider2D>(),
			Weapon.GetComponent<Collider2D>()
			);

		Weapon.isOnEnemy = true;
	}
	void Update()
	{
		Pursue();

		timer -= Time.deltaTime;
		cooldownTimer -= Time.deltaTime;

		if (timer <= 0)
		{
			Weapon.IsAbleToDamage = false;
			timer = 0;
		}


		currentPos = transform.position.x;
		IsOnGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
		//if(IsOnGround)
		rigidBody2D.velocity = new Vector2(Speed * direction, rigidBody2D.velocity.y);

		animator.SetFloat("Speed", Mathf.Abs(rigidBody2D.velocity.x));
		animator.SetBool("IsOnGround", IsOnGround);
		//animator.Setint("HP", GetComponent<Health>().HP);
	}
	public void Pursue()
	{
		// повороты 
		if (target.position.x < currentPos && isFacingRight) Flip();
		if (target.position.x > currentPos && !isFacingRight) Flip();

		// следование за целью
		if ((Mathf.Abs(target.position.x - currentPos) > weaponRange))
		{
			if (target.position.x < currentPos) direction = -1;
			else if (target.position.x > currentPos) direction = 1;
		}
		else if ((Mathf.Abs(target.position.x - currentPos) <= weaponRange))
		{
			direction = 0;
			Punch();
		}
	}
	public void Punch()
	{
		if (cooldownTimer > 0) return;
		cooldownTimer = cooldownTime;

		Weapon.IsAbleToDamage = true;
		timer = timeToDamage;
		animator.SetTrigger("Strike");
	}
	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	public void Die()
	{
		animator.SetTrigger("Fall");
		Weapon.GetComponent<Collider2D>().enabled = false;
		GetComponent<CapsuleCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		rigidBody2D.bodyType = RigidbodyType2D.Static;
		isDead = true;
		this.enabled = false;
	}

	public void LoseMask()
	{
		isMaskOn = false;
		Mask.bodyType = RigidbodyType2D.Dynamic;
		Mask.transform.parent = null;
		Mask.GetComponent<CircleCollider2D>().enabled = true;
		//Mask.GetComponent<SpriteRenderer>().sortingLayerName = "Environment";

		float force = 2f;

		int direction;
		if (target.position.x > this.currentPos)
			direction = -1;
		else
			direction = 1;

		Mask.AddForce(new Vector2(direction * force, force), ForceMode2D.Impulse);
	}
	public void GetDamaged()
	{
		if (isMaskOn)
		{
			LoseMask();
			animator.SetTrigger("GetDamaged");
		}
		else
			Die();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Weapon weapon = collision.gameObject.GetComponent<Weapon>();
		if (weapon == null) return;
		if (weapon.isOnEnemy) return;
		if (weapon != null && weapon.IsAbleToDamage)
		{
			GetDamaged();
			audioSource.pitch = Random.Range(0.5f, 0.7f);
			audioSource.PlayOneShot(PunchClip);
			weapon.IsAbleToDamage = false;
		}
	}
}
