using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    int currentRound = 0;
    public enum roundState {STARTROUND, PLAYERTURN, ENEMYTURN, DICETURN, ENDROUND}
    private roundState currentRoundState;

    public List<MovableEntity> allPlayer = new List<MovableEntity>();
    public List<MovableEntity> allEnemy = new List<MovableEntity>();
    public List<MovableEntity> allDie = new List<MovableEntity>();

    public List<MovableEntity> turnPrecedence = new List<MovableEntity>();

    public roundState CurrentRoundState { get => currentRoundState; set => currentRoundState = value; }


    // Start is called before the first frame update
    void Start()
    {
        UpdateEntityLists(EntityType.Player);
        UpdateEntityLists(EntityType.Enemy);
        UpdateEntityLists(EntityType.Die);

        UpdateTurnOrder();

        currentRoundState = roundState.STARTROUND;
        StartCoroutine(RoundStart());
    }

    IEnumerator RoundStart() {
        while (true) {
            currentRound += 1;
            Debug.Log("Round " + currentRound + " Start!!");
            foreach (Entity entity in turnPrecedence) {
                Debug.Log(entity.name + " taking turn");
                entity.IsTakingTurn = true;
                yield return StartCoroutine(entity.RunTurn());
                entity.IsTakingTurn = false;
            }
            Debug.Log("End Round " + currentRound + "!!");
        }
    }

    public void UpdateEntityLists(EntityType _entityType) {
        List<MovableEntity> entites = new List<MovableEntity>(FindObjectsOfType<MovableEntity>());
        List<MovableEntity> tempEntityList = new List<MovableEntity>();

        //Clear List we want to Update
        if (_entityType == EntityType.Player) {
            allPlayer.Clear();
        } else if (_entityType == EntityType.Enemy) {
            allEnemy.Clear();
        } else if (_entityType == EntityType.Die) {
            allDie.Clear();
        }

        //Save Off all the Movable Entities of the Specific Type
        foreach (var movEntity in entites) {
            if (movEntity.type == _entityType) {
                tempEntityList.Add(movEntity);
            }
        }

        //Update the Appropriate List
        if (_entityType == EntityType.Player)
        {
            allPlayer = tempEntityList;
        }
        else if (_entityType == EntityType.Enemy)
        {
            allEnemy = tempEntityList;
        }
        else if (_entityType == EntityType.Die)
        {
            allDie = tempEntityList;
        }
    }

    void UpdateTurnOrder() {
        turnPrecedence = allPlayer.Union(allEnemy).Union(allDie).ToList();
    }
}
