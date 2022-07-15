using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEntity : Entity
{
    void Awake() {
        type = EntityType.IndestructibleObj;
    }
}
