using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaDie : Die
{
    private void Start()
    {
        //Temporarily adding powers to dies
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Plasma, NonAimedDiePower.Pattern.AOE, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Plasma, NonAimedDiePower.Pattern.Basic, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Plasma, NonAimedDiePower.Pattern.Border, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Plasma, NonAimedDiePower.Pattern.Puddle, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Plasma, NonAimedDiePower.Pattern.Cross, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Plasma, NonAimedDiePower.Pattern.Basic, this));

        //DetermineRandomPower();
        //StartCoroutine(ChosenPower.ActivatePower());
    }
}
