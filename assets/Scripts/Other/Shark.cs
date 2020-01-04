using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shark : MonoBehaviour
{
	// Start is called before the first frame update

	public GameObject blood;
	public float speed = 5f;
	public float radius = 5f;
	public float kek;

	float timer = 1.5f;
	float currTime = 0;
	bool allow = false;

	Transform player;
	UnityEngine.Rigidbody2D rgbody2D;

    void Start()
    {
		rgbody2D = GetComponent<UnityEngine.Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	// Update is called once per frame
	void Update()
	{
		if (allow)
			currTime += Time.deltaTime;
		if (currTime >= timer)
			SceneManager.LoadScene(0);

		kek = (player.position - transform.position).magnitude;
		if ((player.position - transform.position).magnitude < 1)
		{
			GameObject splash = Instantiate(blood, transform.position, transform.rotation);
			splash.GetComponent<ParticleSystem>().Play();
			Destroy(splash, splash.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
			player.GetComponentInChildren<Camera>().transform.parent = null;
			Destroy(player.gameObject);

			this.GetComponent<SpriteRenderer>().enabled = false;
			allow = true;
		}
		if (Mathf.Abs(player.position.x - this.transform.position.x) < radius)
		{
			Vector3 direction = (player.transform.position - this.transform.position).normalized;
			GetComponent<Animator>().enabled = false;
			rgbody2D.velocity = direction * speed;
			if (player.position.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
			if (player.position.x > transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			GetComponent<Animator>().enabled = true;
		}
	}
}
