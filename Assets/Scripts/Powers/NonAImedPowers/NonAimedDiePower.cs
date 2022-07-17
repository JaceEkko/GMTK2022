using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NonAimedDiePower : DiePower
{
    public enum Pattern
    {
        AOE,
        Basic,
        Border,
        Cross,
        Flake,
        Puddle,
        Speratic,
        Star,
        Strike,
        TwinForkLeft,
        TwinForkRight
    };
    Pattern powerPattern;

    [SerializeField] private float dieDeactivateTime = 1.0f;

    GameObject hazardPrefab;

    public NonAimedDiePower(DamageType _damageType, Pattern _pattern, Die _die) : base(_damageType, _die) {
        powerPattern = _pattern;
        hazardPrefab = Resources.Load(_damageType + "Hazard") as GameObject;
        DetermineDamageOutput(_damageType);
    }

    public override IEnumerator ActivatePower() {
        Attack(); //initiate attack
        yield return new WaitForSeconds(dieDeactivateTime); //wait a set amount of time before moving on
    }

    public override void Attack() {
        StreamReader patternFile = new StreamReader(Application.dataPath + "/Resources/Patterns/Pattern_" + powerPattern + ".txt");
        string[] startPoint = patternFile.ReadLine().Split(',');
        int startPosX = Die.coords.x - Convert.ToInt32(startPoint[0]);
        int startPosY = Die.coords.y + Convert.ToInt32(startPoint[1]);

        string patternInTxt;
        int currentGridX = startPosX;
        int currentGridY = startPosY;
        while ((patternInTxt = patternFile.ReadLine()) != null) {
            string[] gridRow = patternInTxt.Split(',');
            for (int x = 0; x < gridRow.Length; ++x) {
                Vector2Int gridSpace = new Vector2Int(currentGridX, currentGridY);
                bool hasHazard = gridRow[x] == "1"? true : false;
                DetermineHazardAtCoords(gridSpace, hasHazard);
                currentGridX++;
            }
            currentGridX = startPosX;
            currentGridY--;
        }
        patternFile.Close();
    }

    void DetermineHazardAtCoords(Vector2Int coords, bool _hasHazard) {
        //bool spaceHasEnemy = Die.GridManager.IsSpaceEmpty(coords, EntityType.Enemy);
        Entity e = GridManager.instance.GetEntityOnTile(coords, EntityType.Enemy);
        //Debug.Log("Space Has Enemy: " + e + " at Coords: " + coords);

        if (_hasHazard) {
            Die.Instantiate(hazardPrefab, new Vector3(coords.x, 1, coords.y), Quaternion.identity);
            if (e != null && e.type != EntityType.Die) {
                Debug.Log(e + " Has been hit");
                e.SetHealthPoints(e.HealthPoints - damageOutput);
                Debug.Log(e + "'s Health: " + e.HealthPoints);
            }
        }
    }
}
