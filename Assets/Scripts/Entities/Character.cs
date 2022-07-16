using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MovableEntity
{
    [SerializeField] protected List<Die> dice = new List<Die>();
    [SerializeField] private Transform equippedDieLocation;
    protected Die currentDieInHand;
    protected int currentDieIndex = 0;

    public void EquipDie(int _dieIndex) {
        if(dice.Count < _dieIndex) {
            Debug.LogError("Equipping die index out of range");
            return;
		}

        if(currentDieInHand != null)
            currentDieInHand.gameObject.SetActive(false);

        currentDieInHand = dice[_dieIndex];
        currentDieInHand.gameObject.SetActive(true);
        //Debug.Log(name + " has selected " + currentDieInHand.name);
    }

    public IEnumerator ThrowDie() {
        yield return new WaitForEndOfFrame();
	}
    
    public void AddNewDieToInventory(Die newDie)
    {
        dice.Add(newDie);
        newDie.transform.parent = equippedDieLocation;
        newDie.transform.localPosition = Vector3.zero;
        EquipDie(dice.Count - 1);
    }
    public void RemoveDie(Die die) {
        dice.Remove(die);
        if(die == currentDieInHand) {
            currentDieInHand = null;
		}
	}
}
