using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NonAimedDiePower : DiePower
{
    public enum Pattern
    {
        Cross
    };
    List<string> patternFileLocs = new List<string>() { "Pattern_Cross.txt" };
    Pattern powerPattern;

    public NonAimedDiePower(DamageType _damageType, Pattern _pattern, Die _die) : base(_damageType, _die) {
        powerPattern = _pattern;
        CallPattern();
    }

    void CallPattern() {
        StreamReader patternFile = new System.IO.StreamReader(Application.dataPath + "/Resources/Patterns/Pattern_" + powerPattern + ".txt");
        string[] startPoint = patternFile.ReadLine().Split(',');
        int startPosX = Die.coords.x - Convert.ToInt32(startPoint[0]);
        int startPosY = Die.coords.y + Convert.ToInt32(startPoint[1]);

        string patternInTxt;
        int currentGridX = startPosX;
        int currentGridY = startPosY;
        while ((patternInTxt = patternFile.ReadLine()) != null) {
            string[] gridRow = patternInTxt.Split(',');
            for (var i = 0; i < gridRow.Length; i++) {
                Vector2Int gridSpace = new Vector2Int(currentGridX, currentGridY);
                DetermineHazardAtCoords(gridRow[i]);
                currentGridX++;
            }
            currentGridY++;
        }
        //string patternInTxt;
        /*while ((patternInTxt = patternFile.ReadLine()) != null) { //while text exists.. repeat
            string[] gridSpaces = patternInTxt.Split(',');
            Debug.Log(patternInTxt);
        }
        patternFile.Close();*/
    }

    void DetermineHazardAtCoords(string _hasHazard) {
        
    }
}
