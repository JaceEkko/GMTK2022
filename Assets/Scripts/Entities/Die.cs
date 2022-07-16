using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;
    List<DiePower> diePowers; //The list of all powers the die has at it's disposal

    private Character owner;
    private bool isThrown;

    public string DieName { get => dieName; set => dieName = value; }

    private void Awake()
    {
        type = EntityType.Die;
        //Temporarily adding powers to dies
        diePowers.Add(new NonAimedDiePower(DiePower.DamageType.PLASMA, NonAimedDiePower.Pattern.Cross, this));
    }

    public override IEnumerator RunTurn()
    {
        yield return null;
    }

    public IEnumerator ExecuteDieAction() {
        int faceNum = Random.Range(0, diePowers.Count);
        yield return null;
        //do something with diePowers[faceNum];
	}

    public Entity GetOwner() {
        return owner;
    }
    public bool IsThrown() {
        return isThrown;
	}
}
