using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;
    List<DiePower> diePowers = new List<DiePower>(); //The list of all powers the die has at it's disposal
    private GameObject diePrefab; //The 3D model that represents the Dice when it is thrown

    int inventorySlot; //location in the inventory array of a Character

    private Character owner;
    private bool isThrown;

    public string DieName { get => dieName; set => dieName = value; }
    public GameObject DiePrefab { get => diePrefab; set => diePrefab = value; }
    public int InventorySlot { get => inventorySlot; set => inventorySlot = value; }
    public Character Owner { get => owner; set => owner = value; }

    private void Awake()
    {
        type = EntityType.Die;
    }

    public override IEnumerator RunTurn()
    {
        yield return null;
    }

    public Entity GetOwner() {
        return owner;
    }

    public bool IsThrown() {
        return isThrown;
	}
}
