using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter.Script
{
    public class GridData : MonoBehaviour
    {
        Dictionary<Vector3Int, PlacementData> placedObject = new();
    
        public void AddObjectAt(Vector3Int gridPosition,
            int id,
            int placedObjectIndex)
        {
            List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition);
            PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);
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

        public int GetResponseIndex(Vector3Int gridPosition)
        {
            if (!placedObject.TryGetValue(gridPosition, out var value))
                return -1;
            return value.PlacedOnjectIndex;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach (var pos in placedObject[gridPosition].OccupiedPosition)
            {
                placedObject.Remove(pos);
            }
        }
    }

    public class PlacementData
    {
        public List<Vector3Int> OccupiedPosition;
        public int ID { get; private set; }
        public int PlacedOnjectIndex { get; private set; }

        public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placedOnjectIndex)
        {
            OccupiedPosition = occupiedPosition;
            ID = iD;
            PlacedOnjectIndex = placedOnjectIndex;
        }
    }
}