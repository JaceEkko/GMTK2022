using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    float healthPoints;
    public EntityType type = EntityType.IndestructibleObj;

    public Vector2Int coords { get; set; }

    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    protected bool hasCompletedTurn = false;

    //Determine what Actions will run on an Entity's turn
    public virtual IEnumerator RunTurn() { 
        Debug.Log(name + " Running Turn");
        hasCompletedTurn = false;
        yield return new WaitForEndOfFrame();
    }

    public void UpdatePosition() {
        transform.position = new Vector3(coords.x, transform.position.y, coords.y);
    }
}
