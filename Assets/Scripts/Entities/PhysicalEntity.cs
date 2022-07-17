using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalEntity : Entity {
    void Awake() {
        type = EntityType.IndestructibleObj;
    }

    public override IEnumerator RunTurn() {
        yield return null;
    }
}
