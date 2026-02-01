using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TowerPlacementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TowerFactory towerFactory;
    [SerializeField] private EconomyManager economyManager;
    [SerializeField] private TowerConfig defaultTowerConfig;

    [Header("Settings")]
    // Default to 'Everything' so it works out of the box without inspector setup
    [SerializeField] private LayerMask placementLayer = ~0;

    private void Start()
    {
        // FindObjectOfType is compatible with older Unity versions (2020/2021/2022)
        if (towerFactory == null) towerFactory = FindObjectOfType<TowerFactory>();
        if (economyManager == null) economyManager = FindObjectOfType<EconomyManager>();

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
        // If New Input System is enabled but no mouse, return false (or handle touch)
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
        if (towerFactory == null || economyManager == null || defaultTowerConfig == null) return;

        Ray ray = Camera.main.ScreenPointToRay(GetMousePosition());
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementLayer))
        {
            // Simple check: Don't build on top of other towers or enemies?
            // For now, rely on layer mask (user should set it to 'Ground').

            if (economyManager.CurrentBits >= defaultTowerConfig.cost)
            {
                Tower tower = towerFactory.CreateTower(defaultTowerConfig, hit.point);
                if (tower != null)
                {
                    economyManager.TrySpendBits(defaultTowerConfig.cost);
                    Debug.Log($"Built tower at {hit.point}");
                }
            }
            else
            {
                Debug.Log("Not enough bits!");
            }
        }
    }
}
