using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MovableEntity
{
    [SerializeField] string dieName;
    List<DiePower> diePowers = new List<DiePower>(); //The list of all powers the die has at it's disposal

    private Character owner;
    private bool isThrown;

    [SerializeField] private float throwDuration = 0.1f;
    [SerializeField] private LayerMask ignorePlayerWhenThrown = 55;

    public string DieName { get => dieName; set => dieName = value; }

    private void Awake()
    {
        type = EntityType.Die;
        //Temporarily adding powers to dies
        diePowers.Add(new NonAimedDiePower(DiePower.DamageType.PLASMA, NonAimedDiePower.Pattern.Cross, this));
    }

    public override IEnumerator RunTurn()
    {
        Debug.Log("Dice turn " + name);
        if(Vector2.Distance(coords, owner.coords) >= 2) {
            Debug.Log("Moving");
            yield return StartCoroutine(MoveTowardsEntity(owner));
		}
        yield return new WaitForEndOfFrame();
    }

    //Lerps die to target, but stops if it hits a wall
    public IEnumerator BeThrown(Vector2 destination) {
        Vector3 destinationIn3D = new Vector3(destination.x, transform.position.y, destination.y);
        coords = GridManager.WorldspaceToCoords(destination);
        transform.parent = null;

        RaycastHit hitInfo;
        Vector3 collisionPosition = Vector3.positiveInfinity;
        if(Physics.Raycast(transform.position, destinationIn3D - transform.position, out hitInfo, Vector3.Distance(transform.position, destinationIn3D), ignorePlayerWhenThrown, QueryTriggerInteraction.Ignore)) {
            Debug.Log("Die collided with " + hitInfo.collider.gameObject.name);
            collisionPosition = hitInfo.point;
		}

        float timer = 0;
        while (timer < throwDuration) {
            transform.position = Vector3.Slerp(transform.position, destinationIn3D, timer / throwDuration);
            timer += Time.deltaTime;

            if (collisionPosition != Vector3.positiveInfinity && Vector3.Distance(transform.position, collisionPosition) < 0.1f) {
                coords = GridManager.WorldspaceToCoords(transform.position);
                break;
			}
            yield return new WaitForEndOfFrame();
        }
        GridManager.instance.PlaceNewEntity(this, coords);

        TurnManager.instance.AddDie(this);
        yield return StartCoroutine(ExecuteDieAction());
    }

    public IEnumerator ExecuteDieAction() {
        int faceNum = Random.Range(0, diePowers.Count);
        yield return null;
        //do something with diePowers[faceNum];
	}

    public void SetOwner(Character character) {
        owner = character;
	}
    public Entity GetOwner() {
        return owner;
    }
    public bool IsThrown() {
        return isThrown;
	}
}
