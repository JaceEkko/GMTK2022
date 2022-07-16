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

        //EquipDice
        characterInput.PlayerControls.EquipDie.started += OnEquipDice;
        characterInput.PlayerControls.EquipDie.performed += OnEquipDice;
        characterInput.PlayerControls.EquipDie.canceled += OnEquipDice;
        //ThrowDie
        characterInput.PlayerControls.ThrowDie.started += OnThrowDice;
        characterInput.PlayerControls.ThrowDie.canceled += OnThrowDice;
    }

    void Start()
    {
        GameObject newDie = Resources.Load("TestDiePrefab") as GameObject;
        AddDieToInventory(newDie);
        AddDieToInventory(newDie);
        AddDieToInventory(newDie);

        EquipDie(0); //equip the first die in the Player's Inventory
        EquipDie(5);
    }

    void OnEquipDice(InputAction.CallbackContext _context) {
        var scroll = _context.ReadValue<float>();
        Debug.Log(scroll);
        if (scroll > 0) {
            CurrentDieIndex += 1;
        } else if (scroll < 0) {
            CurrentDieIndex -= 1;
        }
    }
    void OnThrowDice(InputAction.CallbackContext _context) {
        isThrowDiePressed = _context.ReadValueAsButton();
    }

    public override IEnumerator RunTurn()
    {
        Debug.Log("Player: " + name + " Running Turn"); ;
        while (true) {
            if (isThrowDiePressed)
                break;

            Vector2 intendedDirection = GetMovementDirection();
            if(intendedDirection.magnitude > 0) {
                bool didMove = Move(intendedDirection);
                if (didMove)
                    break;
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
