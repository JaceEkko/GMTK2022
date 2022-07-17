using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    int currentRound = 0;

    private List<Entity> allPlayer = new List<Entity>();
    private List<Entity> allEnemy = new List<Entity>();
    private List<Entity> allDice = new List<Entity>();
    private List<Entity> otherEntities = new List<Entity>();
    private List<Entity>[] orderedEntityLists;

    private List<Entity> entitiesDiedThisTurn = new List<Entity>();

    public static TurnManager instance { get; private set; }

    void Start() {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

        List<Entity> allEntities = new List<Entity>(FindObjectsOfType<Entity>());
        foreach(Entity entity in allEntities) {
            if(entity.type == EntityType.Player) {
                allPlayer.Add(entity);
			}
            else if(entity.type == EntityType.Enemy) {
                allEnemy.Add(entity);
			}
            else if(entity.type == EntityType.Die) {
                allDice.Add(entity);
			}
			else {
                otherEntities.Add(entity);
			}
		}
        orderedEntityLists = new List<Entity>[] { allPlayer, allEnemy, allDice, otherEntities };

        StartCoroutine(RoundStart());
    }

    IEnumerator RoundStart() {
        while (true) {
            currentRound += 1;
            Debug.Log("Round " + currentRound + " Start!!");
            foreach(List<Entity> turnPhase in orderedEntityLists) {
                foreach(Entity entity in turnPhase) {
                    //Debug.Log(entity.name + " taking turn!");
                    entity.IsTakingTurn = true;
                    yield return StartCoroutine(entity.RunTurn());
                    entity.IsTakingTurn = false;
				}
                KillDeadEntities();
			}
            //Debug.Log("End Round " + currentRound + "!!");
        }
    }

    public void AddDeadEntity(Entity entity) {
        entitiesDiedThisTurn.Add(entity);
    }
    public void KillDeadEntities() {
        foreach (Entity entity in entitiesDiedThisTurn) {
            entity.gameObject.SetActive(false);
            if (entity.type == EntityType.Enemy)
                RemoveEnemy((Enemy)entity);
            else if (entity.type == EntityType.Die)
                RemoveDie((Die)entity);
            else
                RemoveEntity(entity);                
        }
        entitiesDiedThisTurn.Clear();
	}

    public void StopRound() {
        StopAllCoroutines();
	}
    public void StartRound() {
        StartCoroutine(RoundStart());
	}

    public void AddPlayer(Player player) {
        allPlayer.Add(player);
	}
    public void RemovePlayer(Player player) {
        allPlayer.Remove(player);
	}
    public void AddEnemy(Enemy enemy) {
        allEnemy.Add(enemy);
	}
    public void RemoveEnemy(Enemy enemy) {
        allEnemy.Remove(enemy);
	}
    public void AddDie(Die die) {
        allDice.Add(die);
	}
    public void RemoveDie(Die die) {
        allDice.Remove(die);
	}
    public void AddEntity(Entity entity) {
        otherEntities.Add(entity);
	}
    public void RemoveEntity(Entity entity) {
        otherEntities.Remove(entity);
	}
}
