using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    CharacterInputs characterInput;

    bool isThrowDiePressed = false;
    bool isMovePressed = false;

    void Awake()
    {
        type = EntityType.Player;

        //Character Input
        characterInput = new CharacterInputs();
        characterInput.PlayerControls.Enable();

        //Jump
        characterInput.PlayerControls.ThrowDie.started += onThrowDie;
        characterInput.PlayerControls.ThrowDie.canceled += onThrowDie;
    }

    void onThrowDie(InputAction.CallbackContext _context) {
        isThrowDiePressed = _context.ReadValueAsButton();
    }

    public override IEnumerator RunTurn()
    {
        while (!isThrowDiePressed) {
            Debug.Log("Waiting For Player: " + name); ;
            yield return new WaitForEndOfFrame();
        }
    }
}
