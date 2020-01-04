using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
	public EnemiesWaves Helper;
	private void Start()
	{
		Physics2D.IgnoreCollision(
	Helper.player.GetComponent<PlayerCombatController>().Weapon.GetComponent<Collider2D>(),
	this.GetComponent<Collider2D>()
	);

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Helper.Save();
	}
}
