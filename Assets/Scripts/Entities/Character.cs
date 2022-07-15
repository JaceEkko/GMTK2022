using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MovableEntity
{
    void Awake()
    {
        type = EntityType.Enemy;
    }
}
