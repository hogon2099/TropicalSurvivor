using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovementController : MonoBehaviour
{
	private UnityEngine.Rigidbody2D rgBody2D;
	private Animator animator;

	public Transform GroundCheck;
	public float GroundCheckRadius = 0.15f;
	public LayerMask WhatIsGround;

	public bool IsOnGround = false;
	public float Speed = 10f;
	public float JumpForce = 10f;

	public bool isFacingRight = true;

	void Start()
	{
		rgBody2D = GetComponent<UnityEngine.Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		IsOnGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
		Jump();
		Walk();

		animator.SetFloat("Speed", Mathf.Abs(rgBody2D.velocity.x));
		animator.SetBool("IsOnGround", IsOnGround);
		FollowMouse();
	}

	private void Walk()
	{
		float inputX = Input.GetAxis("Horizontal");
		rgBody2D.velocity = new Vector2(inputX * Speed, rgBody2D.velocity.y);
	}

	private void Jump()
	{
		if (IsOnGround && Input.GetKeyDown(KeyCode.Space))
			rgBody2D.velocity = Vector2.up * JumpForce;
	}
	private void FollowMouse()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
		float angle = Mathf.Atan2(direction.y, direction.x);
		if (isFacingRight)
			if (Mathf.Cos(angle) < 0)
				Flip();
		if (!isFacingRight)
			if (Mathf.Cos(angle) > 0)
				Flip();
	}
	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}