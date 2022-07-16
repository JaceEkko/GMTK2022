using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private CharacterInputs characterInput;

    private bool isThrowDiePressed = false;

    void Awake()
    {
        type = EntityType.Player;

        //Character Input
        characterInput = new CharacterInputs();
        characterInput.PlayerControls.Enable();

        //EquipDice
        characterInput.PlayerControls.EquipDie.performed += OnEquipDice;
    }

    void Start()
    {
        AddNewDieToInventory(Instantiate(Resources.Load("TestDiePrefab") as GameObject).GetComponent<Die>());
        AddNewDieToInventory(Instantiate(Resources.Load("TestDiePrefab 1") as GameObject).GetComponent<Die>());
        AddNewDieToInventory(Instantiate(Resources.Load("TestDiePrefab 2") as GameObject).GetComponent<Die>());
    }

    void OnEquipDice(InputAction.CallbackContext _context) {
        var scroll = _context.ReadValue<float>();
        if(scroll == 0) {
            return;
		} else if (scroll < 0) {
            currentDieIndex -= 1;
        } else if (scroll > 0) {
            currentDieIndex += 1;
        }

        if (currentDieIndex < 0)
            currentDieIndex = dice.Count - 1;
        else if (currentDieIndex >= dice.Count)
            currentDieIndex = 0;
        EquipDie(currentDieIndex);
    }

    public override IEnumerator RunTurn() {
        while (IsTakingTurn) {
            if (currentDieInHand != null) {
                if (characterInput.PlayerControls.ThrowDie.ReadValue<float>() > 0) {
                    yield return StartCoroutine(ThrowDie());
                }
            }

            Vector2 intendedDirection = GetMovementDirection();
            if (intendedDirection.magnitude > 0) {
                yield return StartCoroutine(Move(intendedDirection));
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private Vector2 GetMovementDirection() {
        Vector2 direction = characterInput.PlayerControls.Move.ReadValue<Vector2>();
        direction.x -= characterInput.PlayerControls.DiagonalNW.ReadValue<float>();
        direction.y += characterInput.PlayerControls.DiagonalNW.ReadValue<float>();
        direction.x += characterInput.PlayerControls.DiagonalNE.ReadValue<float>();
        direction.y += characterInput.PlayerControls.DiagonalNE.ReadValue<float>();
        direction.x += characterInput.PlayerControls.DiagonalSE.ReadValue<float>();
        direction.y -= characterInput.PlayerControls.DiagonalSE.ReadValue<float>();
        direction.x -= characterInput.PlayerControls.DiagonalSW.ReadValue<float>();
        direction.y -= characterInput.PlayerControls.DiagonalSW.ReadValue<float>();

        return direction;
	}
}
