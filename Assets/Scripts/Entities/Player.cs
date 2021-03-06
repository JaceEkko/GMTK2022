using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private CharacterInputs characterInput;

    void Awake()
    {
        type = EntityType.Player;

        //Character Input
        characterInput = new CharacterInputs();
        characterInput.PlayerControls.Enable();

        //EquipDice
        characterInput.PlayerControls.EquipDie.performed += OnEquipDice;
    }

    protected override void Start()
    {
        base.Start();
        //Add 3 dice to inventory for testing
        AddNewDieToInventory(Instantiate(Resources.Load("PlasmaDie") as GameObject).GetComponent<Die>());
        AddNewDieToInventory(Instantiate(Resources.Load("CryoDie") as GameObject).GetComponent<Die>());
        AddNewDieToInventory(Instantiate(Resources.Load("ZapDie") as GameObject).GetComponent<Die>());
    }

    void OnEquipDice(InputAction.CallbackContext _context) {
        if (dice.Count == 0)
            return;

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
            //Throw die
            if (currentDieInHand != null) {
                if (characterInput.PlayerControls.ThrowDie.ReadValue<float>() > 0) {
                    yield return StartCoroutine(ThrowDie(GridManager.instance.GetMouseCoords()));
                    break;
                }
            }

            //Pick up die
            if(characterInput.PlayerControls.PickUpDie.ReadValue<float>() > 0) {
				Vector2Int mouseCoords = GridManager.instance.GetMouseCoords();
                if(Vector2.Distance(mouseCoords, coords) < 2) {
                    List<Die> pickedUpDice = GridManager.instance.PickUpDiceOnTile(mouseCoords, this);
                    foreach (Die die in pickedUpDice)
                        AddNewDieToInventory(die);
				}
			}

            Vector2 intendedDirection = GetMovementDirection();
            if (intendedDirection.magnitude > 0) {
                yield return StartCoroutine(Move(intendedDirection));
            }
            else if(characterInput.PlayerControls.SkipTurn.ReadValue<float>() > 0) {
                IsTakingTurn = false;
                break;
			}
            yield return new WaitForEndOfFrame();
        }
    }

	protected override void Die() {
        SetHealthPoints(initialHP);
        GameStateManager.instance.Reset();
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
