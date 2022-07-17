using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapDie : Die
{
    private void Start()
    {
        //Temporarily adding powers to dies
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Zap, NonAimedDiePower.Pattern.TwinForkLeft, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Zap, NonAimedDiePower.Pattern.Basic, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Zap, NonAimedDiePower.Pattern.TwinForkRight, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Zap, NonAimedDiePower.Pattern.Speratic, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Zap, NonAimedDiePower.Pattern.Strike, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Zap, NonAimedDiePower.Pattern.Basic, this));

        //DetermineRandomPower();
        //StartCoroutine(ChosenPower.ActivatePower());
    }
}
