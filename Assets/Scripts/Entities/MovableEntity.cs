using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableEntity : Entity
{
    //How long it takes (realtime) to animate moving between two tiles
    [Header("Movement Variables")]
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float turnDuration = 0.1f;

    TurnManager turnManager;
    public TurnManager TurnManagerCall { get => turnManager; set => turnManager = value; }

    private void Awake()
    {
        type = EntityType.IndestructibleObj;
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }

    protected IEnumerator Move(Vector2 direction) {
        Vector2Int destination = new Vector2Int(coords.x + (int)direction.x, coords.y + (int)direction.y);
        StartCoroutine(SmoothRotate(new Vector2Int((int) direction.x, (int) direction.y)));
        if (GridManager.instance.MoveTo(this, destination))
            yield return StartCoroutine(AnimateMoveToCoords());
    }

    protected virtual IEnumerator MoveTowardsEntity(Entity target) {
        Vector2 direction = Vector2.zero;

        if (target.transform.position.x < transform.position.x)
            direction.x = -1;
        else if (target.transform.position.x > transform.position.x)
            direction.x = 1;

        if (target.transform.position.z < transform.position.z)
            direction.y = -1;
        else if (target.transform.position.z > transform.position.z)
            direction.y = 1;

        yield return StartCoroutine(Move(direction));
    }

    protected IEnumerator MoveToTile(Vector2 coords) {
        if (GridManager.instance.MoveTo(this, new Vector2Int((int)coords.x, (int)coords.y)));
            yield return StartCoroutine(AnimateMoveToCoords());
    }

    protected virtual IEnumerator AnimateMoveToCoords() {
        Vector3 originalPosition = transform.position;
        float timer = 0;
        while(timer < moveDuration) {
            Vector3 coordsIn3D = new Vector3(coords.x, transform.position.y, coords.y);
            transform.position = Vector3.Slerp(originalPosition, coordsIn3D, timer / moveDuration);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
		}
        UpdatePosition();
        IsTakingTurn = false;
    }
    protected virtual IEnumerator SmoothRotate(Vector2Int direction) {
        float originalRotation = transform.eulerAngles.y;
        float goalRotation = GridManager.DirectionToDegrees(direction);
        float timer = 0;
        while(timer < turnDuration) {
            float currentRotation = Mathf.Lerp(originalRotation, goalRotation, timer / turnDuration);
            transform.eulerAngles = new Vector3(0, currentRotation, 0);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
		}
        transform.eulerAngles = new Vector3(0, goalRotation, 0);
	}
}
