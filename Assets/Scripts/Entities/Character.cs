using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MovableEntity
{
    [SerializeField] GameObject[] dice = new GameObject[5];
    GameObject currentDieInHand;
    int currentDieIndex = 0;

    public int CurrentDieIndex { get => currentDieIndex; set => currentDieIndex = value; }

    private void Awake()
    {
        type = EntityType.Enemy;
    }

    public void EquipDie(int _dieIndex) {
        if (_dieIndex < 0) {
            _dieIndex = 0;
        } else if (_dieIndex >= dice.Length) {
            _dieIndex = dice.Length - 1;
        }
        /*if (_dieIndex < 0 || _dieIndex > dice.Length) {
            Debug.LogError(name + ": _dieIndex falls out of range _dieIndex = " + _dieIndex);
            return;
        }*/

        GameObject selectedDie;
        if (dice[_dieIndex]) { //Equip the Die
            selectedDie = dice[_dieIndex];
            currentDieIndex = _dieIndex;
        }
        else {  //There is No Die at the selected Index position
            //Debug.LogError(name + ": There is no Die to select at position " + _dieIndex);
            selectedDie = getNextAvailableDie(_dieIndex);
        }
        currentDieInHand = selectedDie;
        Debug.Log(name + " has selected " + currentDieInHand.name);
    }
    GameObject getNextAvailableDie(int _dieIndex) {
        int newIndex = _dieIndex;
        GameObject nextDie = null;
        while (!nextDie) {
            newIndex += 1;
            if (newIndex >= dice.Length) { newIndex = 0; }
            if (dice[newIndex]) {
                nextDie = dice[newIndex];
            }
        }
        return nextDie;
    }

    //Method(s) to Add Dice to your Inventory
    public void AddDieToInventory(GameObject _die)
    {
        for (int i = 0; i < dice.Length; i++){
            if (!dice[i]) {
                dice[i] = _die;
                Die diceData = _die.GetComponent<Die>();
                diceData.InventorySlot = i;
                diceData.enabled = false;
                Debug.Log("Added " + _die.name + " to " + name + "'s Inventory at slot " + diceData.InventorySlot);
                break;
            }
        }
    }
    //Method(s) to remove Dice from your Inventory
    public void RemoveDieFromInventory(Die _die) {
    }


}
