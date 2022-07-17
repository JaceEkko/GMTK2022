using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MovableEntity
{
    protected List<Die> dice = new List<Die>();
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
        currentDieIndex = _dieIndex;
        //Debug.Log(name + " has selected " + currentDieInHand.name);
    }

    public IEnumerator ThrowDie(Vector2 targetTile) {
        yield return StartCoroutine(dice[currentDieIndex].BeThrown(targetTile));
        IsTakingTurn = false;
        RemoveDie(dice[currentDieIndex]);
	}
    
    public void AddNewDieToInventory(Die newDie)
    {
        dice.Add(newDie);
        newDie.transform.parent = equippedDieLocation;
        newDie.transform.localPosition = Vector3.zero;
        TurnManager.instance.RemoveDie(newDie);
        EquipDie(dice.Count - 1);
        if (newDie.GetOwner() == null)
            newDie.SetOwner(this);
    }
    public void RemoveDie(Die die) {
        dice.Remove(die);
        if(die == currentDieInHand) {
            currentDieInHand = null;
            currentDieIndex = 0;
		}
	}
}
