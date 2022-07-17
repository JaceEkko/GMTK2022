using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    float healthPoints = 50f;
    public EntityType type = EntityType.IndestructibleObj;

    public Vector2Int coords { get; set; }

    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    public bool IsTakingTurn = true;

    //Determine what Actions will run on an Entity's turn
    public abstract IEnumerator RunTurn();

    public void UpdatePosition() {
        transform.position = new Vector3(coords.x, transform.position.y, coords.y);
    }

    public void SetHealthPoints(float _newHealth) {
        healthPoints = _newHealth;
    }
}
