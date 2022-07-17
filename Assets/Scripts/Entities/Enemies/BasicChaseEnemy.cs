using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChaseEnemy : Enemy {
	[SerializeField] private float desiredDistToPlayer = 3;

	private bool idledLastTurn = false;

	public override IEnumerator RunTurn() {
		StartCoroutine(base.RunTurn());
		if (hasSeenPlayer) {
			//Approach player
			if (Vector3.Distance(transform.position, player.transform.position) > desiredDistToPlayer)
				yield return StartCoroutine(MoveTowardsEntity(player));
			//Think about attacking
			else {
				transform.LookAt(player.transform);
				if(dice.Count > 0) { //Throw!
					EquipDie(Random.Range(0, dice.Count));
					Vector2 targetTile = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) + player.coords;
					yield return StartCoroutine(ThrowDie(targetTile));
				}
				else { //Try to pick up one of my thrown dice
					bool foundDie = false;
					foreach(Die die in diceIveThrown) {
						if(die.transform.parent == null) {
							yield return StartCoroutine(MoveTowardsEntity(die));
							foundDie = true;
							break;
						}
					}
					if (!foundDie)
						yield return StartCoroutine(MoveAwayFromEntity(player));
				}
			}
		}
		else {
			if (idledLastTurn) {
				yield return StartCoroutine(RandomMove());
				idledLastTurn = false;
				LookForPlayer();
			}
			else {
				yield return StartCoroutine(SkipTurn());
				idledLastTurn = true;
			}
		}
	}
}
