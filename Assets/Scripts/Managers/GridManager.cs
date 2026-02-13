using System.Collections.Generic;
using UnityEngine;

namespace NeonDefense.Managers
{
    /// <summary>
    /// Manages the game grid, handling coordinate conversion and cell occupation.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [Header("Grid Settings")]
        [SerializeField] private float cellSize = 2.0f;
        [SerializeField] private Vector2 gridOffset = Vector2.zero;

        private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Snaps a world position to the center of the nearest grid cell.
        /// </summary>
        public Vector3 GetNearestGridPosition(Vector3 worldPosition)
        {
            Vector2Int gridCoords = GetGridCoordinates(worldPosition);
            return GetWorldPosition(gridCoords);
        }

        /// <summary>
        /// Converts world position to grid coordinates.
        /// </summary>
        public Vector2Int GetGridCoordinates(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt((worldPosition.x - gridOffset.x) / cellSize);
            int z = Mathf.RoundToInt((worldPosition.z - gridOffset.y) / cellSize);
            return new Vector2Int(x, z);
        }

        /// <summary>
        /// Converts grid coordinates back to world position.
        /// </summary>
        public Vector3 GetWorldPosition(Vector2Int gridCoords)
        {
            float x = gridCoords.x * cellSize + gridOffset.x;
            float z = gridCoords.y * cellSize + gridOffset.y;
            return new Vector3(x, 0, z); // Assuming flat ground at y=0
        }

        /// <summary>
        /// Checks if a cell at the given world position is occupied.
        /// </summary>
        public bool IsCellOccupied(Vector3 worldPosition)
        {
            Vector2Int coords = GetGridCoordinates(worldPosition);
            return occupiedCells.Contains(coords);
        }

        /// <summary>
        /// Marks the cell at the given world position as occupied.
        /// </summary>
        public void OccupyCell(Vector3 worldPosition)
        {
            Vector2Int coords = GetGridCoordinates(worldPosition);
            if (!occupiedCells.Contains(coords))
            {
                occupiedCells.Add(coords);
            }
        }

        /// <summary>
        /// Clears a cell (e.g. if a tower is sold/destroyed).
        /// </summary>
        public void ClearCell(Vector3 worldPosition)
        {
            Vector2Int coords = GetGridCoordinates(worldPosition);
            if (occupiedCells.Contains(coords))
            {
                occupiedCells.Remove(coords);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            // Draw a small grid visualization around the center (just for debug)
            for (int x = -10; x <= 10; x++)
            {
                for (int z = -10; z <= 10; z++)
                {
                    Vector3 center = GetWorldPosition(new Vector2Int(x, z));
                    Gizmos.DrawWireCube(center, new Vector3(cellSize, 0.1f, cellSize));
                }
            }
        }
    }
}
