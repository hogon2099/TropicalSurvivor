using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerCombatController : MonoBehaviour
{

	private Animator animator;
	private Rigidbody2D rigidBody2D;

	#region combat
	public Weapon Weapon;
	private bool isMaskOn = true;
	private Rigidbody2D Mask;
	private float timer = 0f;
	private float timeToDamage = 0.2f;
	#endregion

	#region audio
	public AudioClip PunchClip;
	private AudioSource audioSource;
	#endregion

	private bool allowToReload = false;
	private float timerToDie = 0f;
	private float timeToDie = 0.4f;

	private bool isFacingRight
	{
		get => GetComponent<PlayerMovementController >()?.isFacingRight ?? true;
	}

	void Start()
	{
		Mask = GetComponentInChildren<MaskController>()?.GetComponent<Rigidbody2D>();
		if (Mask == null)
			isMaskOn = false;

		Weapon.Owner = this.transform;

		rigidBody2D = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();

		animator.SetTrigger("Help");

		if (Weapon == null)
			Weapon = GetComponentInChildren<Weapon>();
	}

	void Update()
	{
		timer -= Time.deltaTime;

		if (allowToReload)
			timerToDie += Time.deltaTime;
		if (timerToDie >= timeToDie)
		SceneManager.LoadScene(0);
		

		if (Input.GetMouseButtonDown(0))
		{
			Weapon.IsAbleToDamage = true;
			timer = timeToDamage;

			Strike();
		}

		if (timer <= 0)
		{
			Weapon.IsAbleToDamage = false;
			timer = 0;
		}
	}
	private void Strike()
	{
		animator.SetTrigger("Strike");
	}
	public void Die()
	{
		animator.SetTrigger("Fall");
		GetComponent<CapsuleCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		rigidBody2D.bodyType = RigidbodyType2D.Static;
		allowToReload = true;		
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

		if (isFacingRight) direction = -1;
		else direction = 1;

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
		if (weapon != null && weapon.IsAbleToDamage)
		{
			GetDamaged();
			audioSource.pitch = Random.Range(0.5f, 0.7f);
			audioSource.PlayOneShot(PunchClip);
			weapon.IsAbleToDamage = false;
		}
	}
}
