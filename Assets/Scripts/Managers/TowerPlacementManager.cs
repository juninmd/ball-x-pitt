using UnityEngine;
using NeonDefense.Towers;
using NeonDefense.ScriptableObjects;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace NeonDefense.Managers
{
    /// <summary>
    /// Handles player input for placing towers on the grid.
    /// </summary>
    public class TowerPlacementManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TowerFactory towerFactory;
        [SerializeField] private EconomyManager economyManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private TowerConfig defaultTowerConfig;

        [Header("Settings")]
        [Tooltip("Layer mask for valid placement surfaces (e.g. Ground).")]
        [SerializeField] private LayerMask placementLayer = ~0;

        private void Start()
        {
            if (towerFactory == null) towerFactory = FindObjectOfType<TowerFactory>();
            if (economyManager == null) economyManager = EconomyManager.Instance;
            if (gridManager == null) gridManager = GridManager.Instance;

            if (defaultTowerConfig == null)
            {
                Debug.LogWarning("TowerPlacementManager: No Default Tower Config assigned!");
            }
        }

        private void Update()
        {
            if (WasMouseClicked())
            {
                HandleClick();
            }
        }

        private bool WasMouseClicked()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                return Mouse.current.leftButton.wasPressedThisFrame;
            }
            return false;
#else
            return Input.GetMouseButtonDown(0);
#endif
        }

        private Vector3 GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                return Mouse.current.position.ReadValue();
            }
            return Vector3.zero;
#else
            return Input.mousePosition;
#endif
        }

        private void HandleClick()
        {
            if (towerFactory == null || economyManager == null || defaultTowerConfig == null || gridManager == null) return;

            Ray ray = Camera.main.ScreenPointToRay(GetMousePosition());
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementLayer))
            {
                // Get snapped grid position
                Vector3 gridPosition = gridManager.GetNearestGridPosition(hit.point);

                // Check if occupied
                if (gridManager.IsCellOccupied(gridPosition))
                {
                    Debug.Log("Cell is occupied!");
                    return;
                }

                // Check cost
                if (economyManager.CurrentBits >= defaultTowerConfig.cost)
                {
                    // Place tower
                    Tower tower = towerFactory.CreateTower(defaultTowerConfig, gridPosition);

                    if (tower != null)
                    {
                        economyManager.SpendBits(defaultTowerConfig.cost);
                        gridManager.OccupyCell(gridPosition);
                        Debug.Log($"Built tower at {gridPosition}");
                    }
                }
                else
                {
                    Debug.Log("Not enough bits!");
                }
            }
        }
    }
}
