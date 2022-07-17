using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] float healthPoints = 50f;
    protected float initialHP;
    public EntityType type = EntityType.IndestructibleObj;

    public Vector2Int coords { get; set; }

    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    [System.NonSerialized] public bool IsTakingTurn = true;

    //Determine what Actions will run on an Entity's turn
    public abstract IEnumerator RunTurn();

    protected virtual void Start() {
        initialHP = healthPoints;
	}

    public void UpdatePosition() {
        transform.position = new Vector3(coords.x, transform.position.y, coords.y);
    }

    public virtual void SetHealthPoints(float _newHealth) {
        if (type != EntityType.IndestructibleObj) {
            healthPoints = _newHealth;

            if (healthPoints <= 0) {
                healthPoints = 0;
                Die();
            }
        }
    }

    protected virtual void Die() {
		switch (type) {
            case EntityType.IndestructibleObj:
                return;
            case EntityType.Enemy:
                TurnManager.instance.AddDeadEntity(this);
                GridManager.instance.RemoveEntity(this);
                break;
		}
        TurnManager.instance.AddDeadEntity(this);
	}
}
