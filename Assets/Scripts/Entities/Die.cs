using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    List<DiePower> diePowers = new List<DiePower>();

    private Character owner;
    private bool isThrown;

    public Die() {
        
    }

    private void Awake()
    {
        type = EntityType.Die;
    }

    public override IEnumerator RunTurn()
    {
        return base.RunTurn();
    }

    public IEnumerator ActivateDie() {
        yield return null;
    }

    void determineRandomPower() {
    
    }

    void setDiePowers() {
    
    }

    public Entity GetOwner() {
        return owner;
    }

    public bool IsThrown() {
        return isThrown;
	}
}
