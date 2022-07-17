using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public string map;
    public Vector2Int mapStartLoc;

    public GameObject floorTile;
    public GameObject gateTile;
    public GameObject cornerTile;
   
    void Start()
    {
        GenerateLevel(map);
    }

    void GenerateLevel(string _map) {
        StreamReader mapFile = new StreamReader(Application.dataPath + "/Resources/Levels/" + _map + ".txt");
        string[] startPoint = mapFile.ReadLine().Split(',');
        int startPosX = mapStartLoc.x - Convert.ToInt32(startPoint[0]);
        int startPosY = mapStartLoc.y + Convert.ToInt32(startPoint[1]);

        string patternInTxt;
        int currentGridX = startPosX;
        int currentGridY = startPosY;
        while ((patternInTxt = mapFile.ReadLine()) != null)
        {
            string[] gridRow = patternInTxt.Split(',');
            for (int x = 0; x < gridRow.Length; ++x)
            {
                Vector2Int gridSpace = new Vector2Int(currentGridX, currentGridY);
                string tileType = gridRow[x];//bool hasHazard = gridRow[x] == "1" ? true : false;
                DetermineTileToPlace(gridSpace, tileType);

                currentGridX++;
            }
            currentGridX = startPosX;
            currentGridY--;
        }
        mapFile.Close();
    }

    void DetermineTileToPlace(Vector2Int _coords, string _tile) {
        GameObject tileObj = null;
        if (_tile == "T")
        {
            tileObj = floorTile;
        }
        else if (_tile == "G")
        {
            tileObj = gateTile;
        }
        else if (_tile == "C")
        {
            tileObj = cornerTile;
        }

        if (tileObj != null) {
            Instantiate(tileObj, new Vector3(_coords.x, 0, _coords.y), Quaternion.identity);
        }
    }
}
