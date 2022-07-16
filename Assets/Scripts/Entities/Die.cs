using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;
    DiePower[] diePowers; //The list of all powers the die has at it's disposal
    [SerializeField] private GameObject dieMesh;

    private Character owner;
    private bool isThrown;

    public string DieName { get => dieName; set => dieName = value; }

    private void Awake()
    {
        type = EntityType.Die;
    }

    public override IEnumerator RunTurn()
    {
        yield return null;
    }

    public IEnumerator ExecuteDieAction() {
        int faceNum = Random.Range(0, diePowers.Length);
        yield return null;
        //do something with diePowers[faceNum];
	}

    public Entity GetOwner() {
        return owner;
    }
    public bool IsThrown() {
        return isThrown;
	}
}
