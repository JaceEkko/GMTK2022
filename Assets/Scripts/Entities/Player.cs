using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    CharacterInputs characterInput;

    bool isThrowDiePressed = false;

    void Awake()
    {
        type = EntityType.Player;

        //Character Input
        characterInput = new CharacterInputs();
        characterInput.PlayerControls.Enable();

        //ThrowDie
        characterInput.PlayerControls.ThrowDie.started += OnThrowDie;
        characterInput.PlayerControls.ThrowDie.canceled += OnThrowDie;
    }

    void OnThrowDie(InputAction.CallbackContext _context) {
        isThrowDiePressed = _context.ReadValueAsButton();
    }

    public override IEnumerator RunTurn()
    {
        Debug.Log("Player: " + name + " Running Turn"); ;
        while (!isThrowDiePressed) {
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnMove(InputValue value) {
        Vector2Int destination = new Vector2Int(coords.x + (int) value.Get<Vector2>().x, coords.y + (int) value.Get<Vector2>().y);
        Debug.Log("Moving " + value.Get<Vector2>());
        GridManager.instance.MoveTo(this, destination);
    }
}
