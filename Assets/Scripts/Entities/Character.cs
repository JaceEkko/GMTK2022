using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MovableEntity
{
    [SerializeField] List<GameObject> dice = new List<GameObject>();
    GameObject currentDieLoc;
    [SerializeField] GameObject currentDieInHand;
    [SerializeField] int currentDieIndex = 0;

    public GameObject CurrentDieLoc { get => currentDieLoc; set => currentDieLoc = value; }
    public GameObject CurrentDieInHand { get => currentDieInHand; set => currentDieInHand = value; }
    public int CurrentDieIndex { get => currentDieIndex; set => currentDieIndex = value; }

    private void Awake()
    {
        type = EntityType.Enemy;
    }

    private void Start()
    {
        
    }

    public void EquipDie(int _dieIndex, int _scrollDir = 1) {
        if (dice.Count == 0) { return; }
        var dieMr = currentDieInHand.GetComponent<Die>().DieMR;
        if(dieMr) dieMr.enabled = false;
        //Make sure The _dieIndex doesn't fall out of range
        if (_dieIndex < 0) {
            _dieIndex = dice.Count - 1;
        } else if (_dieIndex >= dice.Count) {
            _dieIndex = 0;
        }
        
        //Determine Which Die will be Selected
        GameObject selectedDie = null;
        if (dice[_dieIndex]) { //Equip the Die
            selectedDie = dice[_dieIndex];
            currentDieIndex = _dieIndex;
        }

        currentDieInHand = selectedDie;
        currentDieInHand.GetComponent<Die>().DieMR.enabled = true;
        Debug.Log(name + " has selected " + currentDieInHand.name);
    }
    
    //Method(s) to Add Dice to your Inventory
    public void AddNewDieToInventory(GameObject _die)
    {
        dice.Add(Instantiate(_die, currentDieLoc.transform.position, Quaternion.identity));
        currentDieInHand = dice[dice.Count - 1];
        currentDieInHand.transform.parent = currentDieLoc.transform;
        currentDieInHand.GetComponent<Die>().DieMR.enabled = false; //disable the Die's mesh from displaying in the scene
        //TurnManagerCall.UpdateEntityLists(EntityType.Die);
    }
    //Method(s) to remove Dice from your Inventory
    public void RemoveDieFromInventory(GameObject _die) {
        dice.Remove(_die);
        Destroy(GameObject.Find(_die.name));
        EquipDie(0);
        TurnManagerCall.UpdateEntityLists(EntityType.Die);
    }

    public void ThrowDieFromInventory(GameObject _die) {
        dice.Remove(_die);
    }


}
