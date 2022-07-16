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
        characterInput.PlayerControls.EquipDie.started += OnEquipDice;
        characterInput.PlayerControls.EquipDie.performed += OnEquipDice;
        characterInput.PlayerControls.EquipDie.canceled += OnEquipDice;
        //ThrowDie
        characterInput.PlayerControls.ThrowDie.started += OnThrowDice;
    }

    void Start()
    {
        GameObject newDie = Resources.Load("TestDiePrefab") as GameObject;
        GameObject newDie1 = Resources.Load("TestDiePrefab 1") as GameObject;
        GameObject newDie2 = Resources.Load("TestDiePrefab 2") as GameObject;
        AddNewDieToInventory(newDie);
        AddNewDieToInventory(newDie1);
        AddNewDieToInventory(newDie2);

        EquipDie(0); //equip the first die in the Player's Inventory
        //RemoveDieFromInventory(newDie);
    }

    void OnEquipDice(InputAction.CallbackContext _context) {
        var scroll = _context.ReadValue<float>();
        if (scroll > 0) {
            scroll = -1;
            CurrentDieIndex -= 1;
        } else if (scroll < 0) {
            scroll = 1;
            CurrentDieIndex += 1;
        }
        if(scroll != 0) EquipDie(CurrentDieIndex, (int)scroll);
    }
    void OnThrowDice(InputAction.CallbackContext _context) {
        isThrowDiePressed = _context.ReadValueAsButton();
    }

    public override IEnumerator RunTurn()
    {
        while (IsTakingTurn) {
            if (isThrowDiePressed) {
                RemoveDieFromInventory(CurrentDieInHand);
                IsTakingTurn = false;
                yield return StartCoroutine(CurrentDieInHand.GetComponent<Die>().RunTurn());
            }

            Vector2 intendedDirection = GetMovementDirection();
            if(intendedDirection.magnitude > 0) {
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
