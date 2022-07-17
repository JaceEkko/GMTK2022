using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicalEntity : Entity {
	public override IEnumerator RunTurn() {
		yield return null;
	}
}
