using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MovableEntity
{
    protected List<Die> dice = new List<Die>();
    [SerializeField] private Transform equippedDieLocation;
    protected Die currentDieInHand;
    protected int currentDieIndex = 0;

	protected override void Start() {
        base.Start();
        Die[] heldDice = GetComponentsInChildren<Die>();
        foreach(Die die in heldDice) {
            die.SetOwner(this);
            AddNewDieToInventory(die);
            GridManager.instance.RemoveEntity(die);
		}
	}

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

    protected virtual IEnumerator ThrowDie(Vector2 targetTile) {
        Die thrownDie = currentDieInHand;
        RemoveDie(currentDieInHand);
        yield return StartCoroutine(thrownDie.BeThrown(targetTile));
        IsTakingTurn = false;
	}
    
    public void AddNewDieToInventory(Die newDie)
    {
        if (dice.Contains(newDie))
            return;
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

	protected override void Die() {
        int x = -1;
        int y = -1;
        foreach(Die die in dice) {
            die.SetOwner(null);
            die.transform.parent = null;

            GridManager.instance.PlaceNewEntity(die, new Vector2Int(coords.x + x, coords.y + y));
            x++;
            if (x == 2) {
                x = -1;
                y++;
			}
            if(y == 2) {
                y = -1;
			}
		}
		base.Die();
	}

    public virtual void Reset() {
        SetHealthPoints(initialHP);
	}
}
