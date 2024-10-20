using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    Dictionary<Vector3Int, PlacementData> placedObject = new();
    
    public void AddObjectAt(Vector3Int gridPosition,
                            int ID,
                            int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach(var pos in positionToOccupy) 
        {
            if(placedObject.ContainsKey(pos))
                throw new Exception($"Cell occupied {pos}");
            placedObject[pos] = data; 
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition)
    {
        List<Vector3Int> returnVal = new();
        returnVal.Add(gridPosition);
        return returnVal;
    }
    
    public bool CanPlacedObjectAt(Vector3Int gridPosition)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition);
        foreach(var pos in positionToOccupy) 
        {
            if(placedObject.ContainsKey(pos))
                return false;
        }
        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPosition;
    public int ID { get; private set; }
    public int PlacedOnjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placedOnjectIndex)
        {
            this.occupiedPosition = occupiedPosition;
            ID = iD;
            PlacedOnjectIndex = placedOnjectIndex;
        }
}