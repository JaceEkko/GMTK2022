using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;
    List<DiePower> diePowers = new List<DiePower>(); //The list of all powers the die has at it's disposal
    private GameObject dieMesh; //The 3D model that represents the Dice when it is thrown
    private MeshRenderer dieMR;

    int inventorySlot; //location in the inventory array of a Character

    GridManager gridManager;

    private Character owner;
    private bool isThrown;
    private bool hasReachedTargetDest;
    private bool hasAttacked;

    public string DieName { get => dieName; set => dieName = value; }
    public GameObject DieMesh { get => dieMesh; }
    public MeshRenderer DieMR { get => dieMR; }
    public int InventorySlot { get => inventorySlot; set => inventorySlot = value; }
    public Character Owner { get => owner; set => owner = value; }

    private void Awake()
    {
        type = EntityType.Die;
        dieMesh = transform.Find("DieRot").Find("DieMesh").gameObject;
        dieMR = dieMesh.GetComponent<MeshRenderer>();

        gridManager = GameObject.Find("TestGrid").GetComponent<GridManager>();
    }

    public override IEnumerator RunTurn()
    {
        if (!hasReachedTargetDest) {
        
        }
        return base.RunTurn();
    }

    IEnumerator Action_ReachThrowDestination() {

        Vector2Int mousePos = gridManager.GetMouseCoords();
        //while () {

        //}
        yield return null;
    }

    public Entity GetOwner() {
        return owner;
    }

    public void ReturnReset() {
        isThrown = false;
        hasReachedTargetDest = false;
    }

    public bool IsThrown() {
        return isThrown;
	}
    public bool HasReachedTargetDest() {
        return hasReachedTargetDest;
    }
}
