using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;

    List<DiePower> diePowers = new List<DiePower>(); //The list of all powers the die has at it's disposal
    DiePower chosenPower; //power that is determine at random once the Die is thrown


    [SerializeField] private GameObject dieMesh;
    private MeshRenderer dieMR;

    GridManager gridManager;

    private Character owner;
    private bool isThrown;
    private bool hasReachedTargetDest;

    public string DieName { get => dieName; set => dieName = value; }
    public List<DiePower> DiePowers { get => diePowers; set => diePowers = value; }
    public MeshRenderer DieMR { get => dieMR; }
    public GridManager GridManager { get => gridManager; set => gridManager = value; }
    public DiePower ChosenPower { get => chosenPower; set => chosenPower = value; }

    private void Awake()
    {
        type = EntityType.Die;
        dieMR = dieMesh.GetComponent<MeshRenderer>();

        gridManager = GameObject.Find("TestGrid").GetComponent<GridManager>();
    }

    public override IEnumerator RunTurn()
    {        
        if (!hasReachedTargetDest) {
        	yield return null;
        }
    }

    public IEnumerator ExecuteDieAction() {
        DetermineRandomPower();
        yield return StartCoroutine(chosenPower.ActivatePower());
	}

    public void ReturnReset() {
        isThrown = false;
        hasReachedTargetDest = false;
    }

    public void DetermineRandomPower() {
        int randomIndex = Mathf.RoundToInt(Random.Range(0, diePowers.Count));
        chosenPower = diePowers[randomIndex];
    }

    public void UsePower() {
        chosenPower.Attack();
    }

    public Entity GetOwner() {
        return owner;
    }
    public bool IsThrown() {
        return isThrown;
	}
}
