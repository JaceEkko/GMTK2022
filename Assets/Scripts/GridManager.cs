using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private int width = 50, height = 50;
    private Entity[,] physicalEntityMap;
    private NonPhysicalEntity[,] nonPhysicalEntityMap;
    private List<Die> allDice;

    [SerializeField] private GameObject tileHighlighter;

    public static GridManager instance { get; private set; }

    void Start() {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

        Entity[] allEntities = FindObjectsOfType<Entity>(true);

        physicalEntityMap = new Entity[width, height];
        nonPhysicalEntityMap = new NonPhysicalEntity[width, height];
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
                    if (nonPhysicalEntityMap[coords.x, coords.y] != null)
                        Debug.LogError("Overlap between " + entity.name + " and " + nonPhysicalEntityMap[coords.x, coords.y].name);
                    else
                        nonPhysicalEntityMap[coords.x, coords.y] = (NonPhysicalEntity)entity;
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

    //Returns a list of all dice that can be picked up at the position myCoords
    public List<Die> CheckForAdjacentDice(Character checkingCharacter, bool canPickUpAnyDie) {
        List<Die> diceInRange = new List<Die>();
        foreach(Die die in allDice) {
			if (die.IsThrown()) {
                if(die.coords.x >= checkingCharacter.coords.x - 1 && die.coords.x <= checkingCharacter.coords.x + 1
                    && die.coords.y >= checkingCharacter.coords.y - 1 && die.coords.y <= checkingCharacter.coords.y + 1) {

                    if (canPickUpAnyDie || die.GetOwner() == checkingCharacter)
                        diceInRange.Add(die);
				}
			}
		}

        return diceInRange;
	}

    public Entity GetEntityOnTile(Vector2Int coords, EntityType type) {
        if (coords.x < 0 || coords.x >= width || coords.y < 0 || coords.y >= height)
            return null;
        switch (type) {
            case EntityType.Die:
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
