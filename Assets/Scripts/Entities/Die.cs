using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;
    List<DiePower> diePowers; //The list of all powers the die has at it's disposal
    [SerializeField] private GameObject dieMesh;
    private MeshRenderer dieMR;

    GridManager gridManager;

    private Character owner;
    private bool isThrown;
    private bool hasReachedTargetDest;

    public string DieName { get => dieName; set => dieName = value; }
    public MeshRenderer DieMR { get => dieMR; }

    private void Awake()
    {
        type = EntityType.Die;
        dieMR = dieMesh.GetComponent<MeshRenderer>();

        gridManager = GameObject.Find("TestGrid").GetComponent<GridManager>();
        //Temporarily adding powers to dies
        diePowers.Add(new NonAimedDiePower(DiePower.DamageType.PLASMA, NonAimedDiePower.Pattern.Cross, this));
    }

    public override IEnumerator RunTurn()
    {        
        if (!hasReachedTargetDest) {
        	yield return null;
        }
    }

    private void ExecuteDieAction() {
        int faceNum = Random.Range(0, diePowers.Count);

        //do something with diePowers[faceNum];
	}

    public void ReturnReset() {
        isThrown = false;
        hasReachedTargetDest = false;
    }

    public Entity GetOwner() {
        return owner;
    }
    public bool IsThrown() {
        return isThrown;
	}
}
