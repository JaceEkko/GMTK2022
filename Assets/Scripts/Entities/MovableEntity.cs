using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEntity : Entity
{
    private void Awake()
    {
        type = EntityType.IndestructibleObj;
    }

    protected bool Move(Vector2 direction) {
        Vector2Int destination = new Vector2Int(coords.x + (int)direction.x, coords.y + (int)direction.y);
        return GridManager.instance.MoveTo(this, destination);
    }
}
