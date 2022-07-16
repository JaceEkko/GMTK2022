using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEntity : Entity
{
    //How long it takes (realtime) to animate moving between two tiles
    [SerializeField] private float moveDuration;

    private void Awake()
    {
        type = EntityType.IndestructibleObj;
    }

    protected IEnumerator Move(Vector2 direction) {
        Vector2Int destination = new Vector2Int(coords.x + (int)direction.x, coords.y + (int)direction.y);
        if (GridManager.instance.MoveTo(this, destination))
            yield return StartCoroutine(AnimateMoveToCoords());
    }

    Vector3 originalPosition;
    protected virtual IEnumerator AnimateMoveToCoords() {
        originalPosition = transform.position;
        float timer = 0;
        while(timer < moveDuration) {
            Vector3 coordsIn3D = new Vector3(coords.x, transform.position.y, coords.y);
            transform.position = Vector3.Slerp(originalPosition, coordsIn3D, timer / moveDuration);
            Debug.Log("Moving " + originalPosition + " to " + coordsIn3D);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
		}

        hasCompletedTurn = true;
    }
}
