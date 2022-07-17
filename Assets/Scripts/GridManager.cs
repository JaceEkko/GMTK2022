using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private int width = 50, height = 50;
    private Entity[,] physicalEntityMap;
    private NonPhysicalEntity[,] nonPhysicalEntityMap, checkpointMap;
    private List<Die> allDice;

    [SerializeField] private GameObject tileHighlighter;

    public static GridManager instance;

    void Start() {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

        Entity[] allEntities = FindObjectsOfType<Entity>(true);

        physicalEntityMap = new Entity[width, height];
        nonPhysicalEntityMap = new NonPhysicalEntity[width, height];
        checkpointMap = new NonPhysicalEntity[width, height];
        allDice = new List<Die>();

        //Sort all entities in the scene into the proper maps and set their grid coordinates
        foreach (Entity entity in allEntities) {
            Vector2Int coords = WorldspaceToCoords(entity.transform.position);
            entity.coords = coords;

            switch (entity.type) {
                case EntityType.Die:
                    allDice.Add((Die)entity);
                    break;
                case EntityType.NonPhysical: //I don't think any nonphysical tiles should be active on start? But maybe
                    if (entity.type == EntityType.Checkpoint) {
                        checkpointMap[coords.x, coords.y] = (NonPhysicalEntity)entity;
                    }
                    else {
                        if (nonPhysicalEntityMap[coords.x, coords.y] != null)
                            Debug.LogError("Overlap between " + entity.name + " and " + nonPhysicalEntityMap[coords.x, coords.y].name);
                        else
                            nonPhysicalEntityMap[coords.x, coords.y] = (NonPhysicalEntity)entity;
                    }
                    break;
                default:
                    if (physicalEntityMap[coords.x, coords.y] != null)
                        Debug.LogError("Overlap between " + entity.name + " and " + physicalEntityMap[coords.x, coords.y].name);
                    else
                        physicalEntityMap[coords.x, coords.y] = entity;
                    break;
            }
        }
	}

	private void Update() {
        Vector3 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.transform.position.y;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2Int mouseCoords = WorldspaceToCoords(mousePos);
        tileHighlighter.transform.position = new Vector3(mouseCoords.x, tileHighlighter.transform.position.y, mouseCoords.y);
	}

	//Moves entity from its current position to the destination (if possible)
    //If moveVisiblePosition is false, the object's actual transform won't be updated. Use when the entity should move itself with an animation or something
	public bool MoveTo(Entity movingEntity, Vector2Int destination, bool moveVisiblePosition = false) {
        if (!IsSpaceEmpty(destination, movingEntity.type))
            return false;
		switch (movingEntity.type) {
            case EntityType.Die:
                break;
            case EntityType.NonPhysical: //Can nonphys move?? Probably not?
                nonPhysicalEntityMap[movingEntity.coords.x, movingEntity.coords.y] = null;
                nonPhysicalEntityMap[destination.x, destination.y] = (NonPhysicalEntity)movingEntity;
                break;
            default:
                physicalEntityMap[movingEntity.coords.x, movingEntity.coords.y] = null;
                physicalEntityMap[destination.x, destination.y] = movingEntity;
                break;
        }
        movingEntity.coords = destination;
        if(moveVisiblePosition)
            movingEntity.UpdatePosition();

        if(movingEntity.type == EntityType.Player) {
            if(checkpointMap[destination.x, destination.y] != null) {
                GameStateManager.instance.SetCheckpoint(destination);
			}
		}
        return true;
	}

    //Tries to place an entity at the destination position, or within 1 tile if the destination is blocked
    public bool PlaceNewEntity(Entity newEntity, Vector2Int destination) {
        if (!IsSpaceEmpty(destination, newEntity.type)) {
            //Loop through the 3x3 area around the target, then give up if there's no valid position
            bool foundTile = false;
            for (int y = -1; y < 2 && !foundTile; y++) {
                for (int x = -1; x < 2 && !foundTile; x++) {
                    Vector2Int newDestination = new Vector2Int(destination.x + x, destination.y + y);
                    if (IsSpaceEmpty(newDestination, newEntity.type)) {
                        destination = newDestination;
                        foundTile = true;
                    }
                }
            }
            if (!foundTile)
                return false;
        }

        switch (newEntity.type) {
            case EntityType.Die:
                allDice.Add((Die) newEntity);
                break;
            case EntityType.NonPhysical:
                nonPhysicalEntityMap[destination.x, destination.y] = (NonPhysicalEntity)newEntity;
                break;
            default:
                physicalEntityMap[destination.x, destination.y] = newEntity;
                break;
        }
        newEntity.coords = destination;
        newEntity.UpdatePosition();
        return true;
    }

    public bool IsSpaceEmpty(Vector2Int coords, EntityType type) {
        if (coords.x < 0 || coords.x >= width || coords.y < 0 || coords.y >= height)
            return false;
		switch (type) {
            case EntityType.Die: //Dice can overlap
                return true;
            case EntityType.NonPhysical: //Nonphys can overwrite each other
                return true;
            default:
                return (physicalEntityMap[coords.x, coords.y] == null);
        }
	}

    public Entity GetEntityOnTile(Vector2Int coords, EntityType type) {
        if (coords.x < 0 || coords.x >= width || coords.y < 0 || coords.y >= height)
            return null;
        switch (type) {
            case EntityType.Die:
                Debug.LogWarning("Did you mean to use GetDieOnTile instead?"); //todo should all the tilemaps be converted to store lists??
                foreach(Die die in allDice) {
                    if (die.coords.x == coords.x && die.coords.y == coords.y)
                        return die;
				}
                return null;
            case EntityType.NonPhysical:
                return nonPhysicalEntityMap[coords.x, coords.y];
            default:
                return physicalEntityMap[coords.x, coords.y];
        }
    }
    //Pick up all the dice on the selected tile. Used by player on mouse click
    public List<Die> PickUpDiceOnTile(Vector2Int coords, Character checkingEntity = null) {
        List<Die> diceOnTile = new List<Die>();
        foreach (Die die in allDice) {
            if (die.coords.x == coords.x && die.coords.y == coords.y)
                diceOnTile.Add(die);
        }
        List<Die> dicePickedUp = new List<Die>();
        foreach (Die die in diceOnTile) {
            if (checkingEntity == die.GetOwner() || checkingEntity.type == EntityType.Player) {
                dicePickedUp.Add(die);
                allDice.Remove(die);
            }
        }
        return dicePickedUp;
    }
    //Gets all die in a radius around the checker. Used by enemies to pick up any dice they've thrown
    public List<Die> PickUpAllAdjacentDice(Character checkingCharacter) {
        List<Die> diceInRange = new List<Die>();
        foreach (Die die in allDice) {
            if (die.coords.x >= checkingCharacter.coords.x - 1 && die.coords.x <= checkingCharacter.coords.x + 1
                && die.coords.y >= checkingCharacter.coords.y - 1 && die.coords.y <= checkingCharacter.coords.y + 1) {
                diceInRange.Add(die);
            }
        }

        List<Die> dicePickedUp = new List<Die>();
        foreach(Die die in diceInRange) {
            if(checkingCharacter.type == EntityType.Player || die.GetOwner() == checkingCharacter) {
                dicePickedUp.Add(die);
                allDice.Remove(die);
			}
		}

        return dicePickedUp;
    }

    public void RemoveEntity(Entity entity) {
		switch (entity.type) {
            case EntityType.Die:
                allDice.Remove((Die)entity);
                break;
            case EntityType.NonPhysical:
                if (nonPhysicalEntityMap[entity.coords.x, entity.coords.y] == (NonPhysicalEntity)entity)
                    nonPhysicalEntityMap[entity.coords.x, entity.coords.y] = null;
                break;
            default:
                if (physicalEntityMap[entity.coords.x, entity.coords.y] == entity)
                    physicalEntityMap[entity.coords.x, entity.coords.y] = null;
                break;
        }
	}

    public Vector2Int GetMouseCoords() {
        return WorldspaceToCoords(tileHighlighter.transform.position);
	}
    public Entity GetEntityUnderMouse() {
        Vector2Int coords = GetMouseCoords();
        return physicalEntityMap[coords.x, coords.y];
	}

    #region static helper methods
	public static Vector2Int WorldspaceToCoords(Vector2 worldspace) {
        return new Vector2Int((int)Mathf.Round(worldspace.x), (int)Mathf.Round(worldspace.y));
    }
    public static Vector2Int WorldspaceToCoords(Vector3 worldspace) {
        return WorldspaceToCoords(new Vector2(worldspace.x, worldspace.z));
	}

    //Converts a Vector2Int direction into degrees around the y axis
    //0 degrees is looking straight north
    public static float DirectionToDegrees(Vector2Int direction) {
        if(direction.y == 1) {
            if (direction.x == -1)
                return -45;
            else if (direction.x == 0)
                return 0;
            else if (direction.x == 1)
                return 45;
		}
        else if (direction.y == 0) {
            if (direction.x == -1)
                return -90;
            else if (direction.x == 0)
                return 0;
            else if (direction.x == 1)
                return 90;
        }
        else if (direction.y == -1) {
            if (direction.x == -1)
                return 225;
            else if (direction.x == 0)
                return 180;
            else if (direction.x == 1)
                return 135;
        }

        Debug.LogError("Invalid direction given");
        return 0;
    }
    #endregion
}
