using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Awake()
    {
        type = EntityType.Player;
    }

    public override IEnumerator RunTurn()
    {
        return base.RunTurn();
    }


}
