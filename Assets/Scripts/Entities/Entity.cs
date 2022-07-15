using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    float healthPoints;
    public EntityType type;

    public Vector2Int coords { get; set; }

    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    void Awake()
    {
        type = EntityType.IndestructibleObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual IEnumerator RunTurn() {
        while (!Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(this.name + " Running Turn");
            yield return null;
        }
    }

    public void UpdatePosition() {
        transform.position = new Vector3(coords.x, coords.y);
    }
}
