using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem;

    private void Awake()
    {
        if(gridDebugObjectPrefab == null)
        {
            Debug.LogError("GridDebugObjectPrefab is not set in the inspector!");
        }
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("Singleton Error! Level Grid already exists!");
            Destroy(gameObject);
            return;
        }
        Instance.gridSystem = new GridSystem(10, 10, 2f);
        Instance.gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void AddUnitToGrid(GridPosition gridPosition, Unit unit)
    {
        GridSquare gridSquare = gridSystem.GetGridSquare(gridPosition);
        gridSquare.AddUnit(unit);
    }

    //public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition) 
    //{
    //    GridSquare gridSquare = gridSystem.GetGridSquare(gridPosition);
    //    return gridSquare.GetUnitList();   
    //}

    public void RemoveUnitFromGrid(GridPosition gridPosition, Unit unit)
    {
        GridSquare gridSquare = gridSystem.GetGridSquare(gridPosition);
        gridSquare.RemoveUnit(unit);
    }

    //Passthroughs for encapsulation
    public GridPosition WorldPositionToGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
}
