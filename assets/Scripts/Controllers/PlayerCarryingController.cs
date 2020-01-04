using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCarryingController : MonoBehaviour
{
	public Transform ItemHolder;
	public LineRenderer AimLine;

	public bool IsRadiusVisible = false;
	public float CheckRadius = 1.5f;
	public LayerMask WhatIsPickupable;
	public Vector2 CarriedItemsPosition = new Vector2(0.5f, 1.6f);
	public int ThrowStrength = 80;
	public bool IsCarrying
	{
		get => carriedItem == null ? false : true;
	}

	private Animator animator;

	Collider2D lootOnGround;
	Transform carriedItem;

	Vector2 mousePosition_Test;
	Vector2 direction_Test;

	void Start()
	{
		animator = this.GetComponent<Animator>();
	}

	void Update()
	{
		#region тестирование
		mousePosition_Test = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction_Test =  new Vector2(mousePosition_Test.x - transform.position.x, mousePosition_Test.y - transform.position.y);
		#endregion тестирование
	
		lootOnGround = Physics2D.OverlapCircle(transform.position, CheckRadius, WhatIsPickupable);

		// Можно подбирать редметы только с Rigidbody2D
		if (lootOnGround != null && lootOnGround.GetComponent<UnityEngine.Rigidbody2D>() == null)
			lootOnGround = null;

		Aim();

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (carriedItem != null)
				Throw();
			else if (lootOnGround != null)
				Pickup();
		}
	}
	public void Pickup()
	{
		bool? IsOnGround = GetComponent<PlayerMovementController >()?.IsOnGround;
		if (!IsOnGround ?? true)
			return;

		carriedItem = lootOnGround.transform;
		ItemHolder.position = carriedItem.position;
		carriedItem.SetParent(ItemHolder);

		UnityEngine.Rigidbody2D rgbody2d = carriedItem.GetComponent<UnityEngine.Rigidbody2D>();
		rgbody2d.isKinematic = true;

		carriedItem.GetComponent<Collider2D>().enabled = false;

		this.animator.SetTrigger("PickUp");
		var carriedItemAnimator = carriedItem.GetComponent<Animator>();
		carriedItemAnimator.enabled = true;
		carriedItemAnimator.SetTrigger("BePickedUp");

	}
	public void Aim()
	{
		if (carriedItem == null)
			return;

		if (Input.GetMouseButton(1))
		{
			AimLine.enabled = true;
			AimLine.positionCount = 2;
			AimLine.SetPosition(0, carriedItem.position);

			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 direction =  new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
			direction *= ThrowStrength;

			AimLine.SetPosition(1, mousePosition);

			if (Input.GetMouseButton(0))
				Throw(direction * ThrowStrength);
		}
		else
		{
			AimLine.enabled = false;
		}
	}

	public void Throw()
	{
		AimLine.enabled = false;
		Vector2 directon = new Vector2(250 * transform.localScale.x, 400).normalized;

		animator.SetTrigger("Throw");
		carriedItem.GetComponent<Animator>().SetTrigger("Reset");
		carriedItem.GetComponent<Animator>().enabled = false;

		UnityEngine.Rigidbody2D rgbody2d = carriedItem.GetComponent<UnityEngine.Rigidbody2D>();
		rgbody2d.isKinematic = false;
		rgbody2d.AddForce(directon);

		carriedItem.GetComponent<Collider2D>().enabled = true;
		carriedItem.SetParent(null);
		carriedItem = null;
	}

	public void Throw(Vector2 direction)
	{
		AimLine.enabled = false;
		animator.SetTrigger("Throw");
		carriedItem.GetComponent<Animator>().SetTrigger("Reset");
		carriedItem.GetComponent<Animator>().enabled = false;

		UnityEngine.Rigidbody2D rgbody2d = carriedItem.GetComponent<UnityEngine.Rigidbody2D>();
		rgbody2d.isKinematic = false;
		rgbody2d.AddForce(direction);

		carriedItem.GetComponent<Collider2D>().enabled = true;
		carriedItem.SetParent(null);
		carriedItem = null;
	}

	private void OnDrawGizmosSelected()
	{
		if (IsRadiusVisible)
			Gizmos.DrawWireSphere(transform.position, CheckRadius);

		Gizmos.DrawLine(transform.position, mousePosition_Test);
		Gizmos.DrawLine(transform.position, direction_Test);
	}
}
