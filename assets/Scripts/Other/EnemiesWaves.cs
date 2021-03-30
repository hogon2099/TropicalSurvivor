using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemiesWaves : MonoBehaviour
{
	public Animator Choppa;
	public Animator Rope;
	public Animator Survived;

	public List<GameObject> WavesPrefab;
	private List<GameObject> WavesSpawned = new List<GameObject>();
	private int counter = 0;

	private bool allowChange = false;
	private float timeBetweenWaves = 1f;
	private float timer = 0f;

	private bool allowReload = false;
	private float reloadTimer = 0f;
	private float reloadTime = 4f;
		
	public Transform player;
	bool end = false;

	void Start()
    {
		player = (GameObject.FindGameObjectWithTag("Player")).transform;
		var temp = Instantiate(WavesPrefab[counter]);
		WavesSpawned.Add(temp);
    }

	// Update is called once per frame
	void Update()
	{
		if (allowReload)
			reloadTimer += Time.deltaTime;
		if(reloadTimer>= reloadTime)
			SceneManager.LoadScene(0);

			if (allowChange)
			timer += Time.deltaTime;

		if (timer >= timeBetweenWaves)
		{
			timer = 0;
			allowChange = false;
			WavesSpawned.Add(Instantiate(WavesPrefab[counter]));
		}

		if (!end)
		{
			if (!(WavesSpawned.Count >= (counter+1))) return;
			EnemyController[] currentEnemies = (WavesSpawned[counter].GetComponentsInChildren<EnemyController>());

			if (IsEveryoneDead(currentEnemies))
			{
				counter++;
				if (WavesPrefab.Count > counter)
					allowChange = true;
				else
				{
					Choppa.SetTrigger("GoDown");
					Rope.SetTrigger("Drop");
					end = true;
				}
			}
		}
	}
	private bool IsEveryoneDead(EnemyController[] enemies)
	{
		bool isEveryoneDead = true;

		foreach (var enemy in enemies)
		{
			if (!enemy.isDead)
				isEveryoneDead = false;
		}
		return isEveryoneDead;
	}

	public void Save()
	{
		Rope.transform.parent = Choppa.transform;
		player.parent = Rope.transform;
		player.GetComponent<Rigidbody2D>().simulated = false;
		player.GetComponent<Animator>().SetTrigger("Hold");
		player.GetComponentInChildren<Camera>().transform.parent = null;
		Choppa.SetTrigger("GoUp");
		Survived.SetTrigger("Show");
		allowReload = true;
	}
}

