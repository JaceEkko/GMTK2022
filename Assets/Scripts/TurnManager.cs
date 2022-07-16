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
    int enitytInTurn = 0;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RoundStart() {
        while (true) {
            currentRound += 1;
            enitytInTurn = 0;
            Debug.Log("Round " + currentRound + " Start!!");
            yield return StartCoroutine(NextTurn());
            Debug.Log("End Round " + currentRound + "!!");
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator NextTurn() {
        while (enitytInTurn != turnPrecedence.Count) {
            yield return StartCoroutine(turnPrecedence[enitytInTurn].RunTurn());
            StopCoroutine(turnPrecedence[enitytInTurn].RunTurn());
            enitytInTurn += 1;
        }
    }

    void UpdateEntityLists(EntityType _entityType) {
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
