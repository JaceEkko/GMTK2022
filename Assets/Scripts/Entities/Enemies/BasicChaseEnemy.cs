using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicChaseEnemy : Enemy {
	[SerializeField] private float desiredDistToPlayer = 3;

	private bool idledLastTurn = false;

	public override IEnumerator RunTurn() {
		LookForPlayer();
		if (hasSeenPlayer) {
			if (Vector3.Distance(transform.position, player.transform.position) > desiredDistToPlayer)
				yield return StartCoroutine(MoveTowardsEntity(player));
			else {
				transform.LookAt(player.transform);
				//Needs to check if dice in inventory
				yield return StartCoroutine(SkipTurn()); //This should be replaced with a thing to throw dice
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
