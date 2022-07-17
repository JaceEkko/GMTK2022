using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character 
{
    [Header("Vision Variables")]
    [SerializeField] private int visionRange = 10;
    [SerializeField] private int visionAngle = 65;
    protected bool hasSeenPlayer;

    [Header("Combat Variables")]
    [SerializeField] private List<Die> heldDice;
    protected Player player;

    protected List<Die> diceIveThrown = new List<Die>();

    private Vector2Int initialPosition;
    private float initialRotation;
    private List<Die> initialDice;

    private void Awake()
    {
        type = EntityType.Enemy;
	}

	protected override void Start() {
		base.Start();
        player = FindObjectOfType<Player>();

        initialPosition = coords;
        initialRotation = transform.eulerAngles.y;
        initialDice = new List<Die>();
        foreach(Die die in dice) {
            initialDice.Add(die);
		}

        GameStateManager.instance.AddEnemy(this);
    }

	public override IEnumerator RunTurn() {
        if(!hasSeenPlayer)
            LookForPlayer();
        List<Die> pickedUpDice = GridManager.instance.PickUpAllAdjacentDice(this);
        foreach (Die die in pickedUpDice) {
            diceIveThrown.Remove(die);
            AddNewDieToInventory(die);
        }
        yield return null;
	}

	protected override IEnumerator ThrowDie(Vector2 targetTile) {
        diceIveThrown.Add(currentDieInHand);
		return base.ThrowDie(targetTile);
	}

	protected virtual void LookForPlayer() {
        //Check if player is in range and within vision cone
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float angleToPlayer = Vector3.Angle(player.transform.position - transform.position, transform.forward);
        if (distance > visionRange || angleToPlayer > visionAngle)
            return;

        //Raycast to check line of sight to the target
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, visionRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Player")
            hasSeenPlayer = true;
    }

	protected virtual IEnumerator RandomMove() {
        Vector2 direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        yield return StartCoroutine(Move(direction));
    }

    protected virtual IEnumerator SkipTurn() {
        yield return new WaitForEndOfFrame();
        IsTakingTurn = false;
	}

	public override void SetHealthPoints(float _newHealth) {
		base.SetHealthPoints(_newHealth);
        hasSeenPlayer = true;
	}

	protected override void Die() {
        TurnManager.instance.RemoveEnemy(this);
        hasSeenPlayer = false;
        base.Die();
	}

	public override void Reset() {
        base.Reset();
        GridManager.instance.PlaceNewEntity(this, initialPosition);
        TurnManager.instance.AddEnemy(this);
        transform.eulerAngles = new Vector3(0, initialRotation, 0);

        foreach(Die die in initialDice) {
            if(die.GetOwner() != null) {
                die.GetOwner().RemoveDie(die);
                die.SetOwner(this);
                AddNewDieToInventory(die);
			}
		}
	}
}
