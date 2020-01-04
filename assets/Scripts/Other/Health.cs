using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
	private AudioSource audioSource;
	private void Start()
	{
		audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
	}

	[SerializeField] private int _hp = 2;
	public int HP
	{
		get => _hp;
		set
		{
			if (value < _hp && value != 0)
			{
				GetComponent<PlayerCombatController>()?.GetDamaged();
			}

			_hp = value;

			if (_hp <= 0)
			{
				_hp = 0;

				if(GetComponent<PlayerMovementController>())
					SceneManager.LoadScene(0);
			}		
		}
	}

	private void ReduceHealth(int damagePoints)
	{ 
		HP -= damagePoints;
	}

	private void IncreaseHealth(int healthPoints)
	{
		HP += healthPoints;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Weapon weapon = collision.gameObject.GetComponent<Weapon>();
		if (weapon == null) return;
		if (weapon.Owner.GetComponent<EnemyController>().isDead) return;
		if (weapon != null && weapon.IsAbleToDamage)
		{
			ReduceHealth(1);
			audioSource.pitch = Random.Range(0.5f, 0.7f);
			audioSource.Play();
			weapon.IsAbleToDamage = false;
		}
	}

}
