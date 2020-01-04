using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
	//public Camera MainCamera;
	public Rigidbody2D Player;
	public float FloatingSpeed = 1;
	public float ParallaxSpeed = 1;

	private float playerSpeed;
	private Vector2 directon;
	private float spriteLength;

	private void Start()
	{
		spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
	}
	void Update()
	{
		playerSpeed = Player.velocity.x;
		directon = new Vector2(-FloatingSpeed - playerSpeed * ParallaxSpeed, 0) * Time.deltaTime;
		transform.Translate(directon);

		if (this.transform.position.x < Player.position.x - spriteLength * 1.5f)
			transform.position = new Vector2(Player.position.x + spriteLength, transform.position.y);

		if (this.transform.position.x > Player.position.x + spriteLength * 1.5f)
			transform.position = new Vector2(Player.position.x - spriteLength, transform.position.y);
	}
}
