using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoDie : Die
{
    private void Start()
    {
        //Temporarily adding powers to dies
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Cryo, NonAimedDiePower.Pattern.Star, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Cryo, NonAimedDiePower.Pattern.Basic, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Cryo, NonAimedDiePower.Pattern.Cross, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Cryo, NonAimedDiePower.Pattern.Flake, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Cryo, NonAimedDiePower.Pattern.Basic, this));
        DiePowers.Add(new NonAimedDiePower(DiePower.DamageType.Cryo, NonAimedDiePower.Pattern.Basic, this));

        //DetermineRandomPower();
        //StartCoroutine(ChosenPower.ActivatePower());
    }
}
