using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysical : Entity
{
    void Awake()
    {
        type = EntityType.NonPhysical;
    }
}
