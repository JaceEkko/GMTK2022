using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public enum VisionType {
        Circle,
        Line,
        Cone
	}

    [Header("Vision Variables")]
    [SerializeField] private VisionType visionType;
    [SerializeField] private int visionRange;

    private bool hasSeenPlayer;

    private void Awake()
    {
        type = EntityType.Enemy;
    }

    public override IEnumerator RunTurn()
    {
        //Determine Action
        return base.RunTurn();
    }

    protected virtual IEnumerator RandomMove() {
        yield return null;
	}
}
