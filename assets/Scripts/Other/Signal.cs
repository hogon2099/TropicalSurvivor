using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
	private Rigidbody2D rgbody2D;
	public float speed;
    void Start()
	{
		rgbody2D = GetComponent<Rigidbody2D>();
		Destroy(this.gameObject, 5);
    }
	private void Update()
	{
		rgbody2D.velocity = Vector2.up * speed;
	}

}
