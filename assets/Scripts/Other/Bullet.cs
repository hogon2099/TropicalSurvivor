using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{
	public bool IsLethal;
	
	void Start()
	{
		Destroy(gameObject, 5);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(this.gameObject);
	}
}
