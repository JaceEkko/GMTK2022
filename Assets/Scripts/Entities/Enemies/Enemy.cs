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
    [SerializeField] private int diceThrowRange = 7;
    [SerializeField] private List<Die> heldDice;
    protected Player player;

    private void Awake()
    {
        type = EntityType.Enemy;
        player = FindObjectOfType<Player>();
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

	#region Movement
	protected virtual IEnumerator RandomMove() {
        Vector2 direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        yield return StartCoroutine(Move(direction));
    }
    protected virtual IEnumerator MoveTowardsPlayer() {
        Vector2 direction = Vector2.zero;

        if (player.transform.position.x < transform.position.x)
            direction.x = -1;
        else if (player.transform.position.x > transform.position.x)
            direction.x = 1;

        if (player.transform.position.z < transform.position.z)
            direction.y = -1;
        else if (player.transform.position.z > transform.position.z)
            direction.y = 1;

        yield return StartCoroutine(Move(direction));
	}
	#endregion

    protected virtual IEnumerator SkipTurn() {
        yield return new WaitForEndOfFrame();
        IsTakingTurn = false;
	}
}
