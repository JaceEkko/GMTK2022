using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiePower
{
    Die die;

    public enum DamageType { 
        Plasma,
        Cryo,
        Zap
    };
    DamageType powerDamage;
    public float damageOutput = 5.0f;

    public Die Die { get => die; set => die = value; }

    public DiePower(DamageType _damageType, Die _die) {
        die = _die;
        powerDamage = _damageType;
    }

    public abstract IEnumerator ActivatePower();

    public abstract void Attack();

    public void DetermineDamageOutput(DamageType _damageType) {
        switch (_damageType) {
            case DamageType.Plasma:
                damageOutput = 5.0f;
                break;
            case DamageType.Cryo:
                damageOutput = 7.0f;
                break;
            case DamageType.Zap:
                damageOutput = 10.0f;
                break;
            default:
                break;
        }
    }

}
