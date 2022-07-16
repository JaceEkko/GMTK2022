using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    void Awake()
    {
        type = EntityType.Enemy;
    }

    public override IEnumerator RunTurn()
    {
        //Determine Action
        return base.RunTurn();
    }
}
