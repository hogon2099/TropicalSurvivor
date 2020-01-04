using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MaskController : MonoBehaviour
{
    private Rigidbody2D rgBody2D;

	public float GroundCheckRadius = 0.15f;
	public LayerMask WhatIsGround;

	public bool IsOnGround = false;

	void Start()
    {
		rgBody2D = GetComponent<Rigidbody2D>();

	}

    // Update is called once per frame
    void Update()
    {
		IsOnGround = Physics2D.OverlapCircle(transform.position, GroundCheckRadius, WhatIsGround);

		if (IsOnGround)
		{
			rgBody2D.bodyType = RigidbodyType2D.Static;
			this.GetComponent<Collider2D>().enabled = false;
			this.enabled = false;
		}
	}
}
