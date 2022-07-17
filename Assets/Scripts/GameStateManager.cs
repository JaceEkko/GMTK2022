using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    private List<Enemy> allEnemies = new List<Enemy>();
    private Player player;

    private Vector2Int lastCheckpoint;

    public static GameStateManager instance;
    void Start() {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

        player = FindObjectOfType<Player>();
    }

    public void AddEnemy(Enemy enemy) {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

    public void SetCheckpoint(Vector2Int coords) {
        lastCheckpoint = coords;
	}

    public void Reset() {
        TurnManager.instance.StopRound();
        foreach (Enemy enemy in allEnemies) {
            enemy.gameObject.SetActive(true);
            enemy.Reset();
        }

        GridManager.instance.MoveTo(player, lastCheckpoint, true);
        TurnManager.instance.StartRound();
    }
}
