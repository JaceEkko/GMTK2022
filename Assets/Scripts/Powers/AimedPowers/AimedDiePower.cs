using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedDiePower : DiePower
{
    public AimedDiePower(DamageType _damageType, Die _die) : base(_damageType, _die)
    {
    }

	public override IEnumerator ActivatePower() {
		throw new System.NotImplementedException();
	}

	public override void Attack() {
		throw new System.NotImplementedException();
	}
}
