using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePower
{
    Die die;

    public enum DamageType { 
        PLASMA,
        CRYO,
        ZAP
    };
    DamageType powerDamage;

    public Die Die { get => die; set => die = value; }

    public DiePower(DamageType _damageType, Die _die) {
        die = _die;
        powerDamage = _damageType;
    }

    public virtual IEnumerator ActivatePower() {
        yield return null;
    }



}
