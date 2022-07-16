using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    private Character owner;
    private bool isThrown;

    private void Awake()
    {
        type = EntityType.Die;
    }

    public override IEnumerator RunTurn()
    {
        return base.RunTurn();
    }

    public Entity GetOwner() {
        return owner;
    }

    public bool IsThrown() {
        return isThrown;
	}
}
