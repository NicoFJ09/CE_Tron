using UnityEngine;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public BoxCollider2D GridArea;
    [SerializeField] private ItemInventoryUI itemInventoryUI;
    [SerializeField] private PowerInventoryUI powerInventoryUI;

    private void Start()
    {
        // Asignar referencias si no están asignadas en el Inspector
        if (itemInventoryUI == null)
        {
            itemInventoryUI = FindObjectOfType<ItemInventoryUI>();
            if (itemInventoryUI == null)
            {
                Debug.LogError("ItemInventoryUI is not assigned and could not be found in the scene.");
                return;
            }
        }

        if (powerInventoryUI == null)
        {
            powerInventoryUI = FindObjectOfType<PowerInventoryUI>();
            if (powerInventoryUI == null)
            {
                Debug.LogError("PowerInventoryUI is not assigned and could not be found in the scene.");
                return;
            }
        }

        // Asignar el inventario a las interfaces de usuario
        itemInventoryUI.SetInventory(Inventory.Instance);
        powerInventoryUI.SetInventory(Inventory.Instance);
    }

    public void RandomizePosition(List<Vector3> occupiedPositions)
    {
        Bounds bounds = this.GridArea.bounds;
        Vector3 position;

        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
        } while (occupiedPositions.Contains(position));

        this.transform.position = position;
        occupiedPositions.Add(position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.ReplaceItem(this.gameObject);
                string itemName = this.gameObject.name;

                // Verificar que Inventory.Instance no sea null
                if (Inventory.Instance == null)
                {
                    Debug.LogError("Inventory instance is null.");
                    return;
                }

                // Verificar que itemInventoryUI y powerInventoryUI no sean null
                if (itemInventoryUI == null)
                {
                    Debug.LogError("ItemInventoryUI is null in OnTriggerEnter2D.");
                    return;
                }

                if (powerInventoryUI == null)
                {
                    Debug.LogError("PowerInventoryUI is null in OnTriggerEnter2D.");
                    return;
                }

                // Verificar condiciones basadas en el nombre del ítem
                if (itemName.Contains("Fuel"))
                {
                    Debug.Log("You picked up fuel!");
                    Inventory.Instance.EnqueueItem(new Objects(Objects.ItemType.Fuel));
                    FunctionTimer.Create(() => {
                        Inventory.Instance.DequeueItem();
                        itemInventoryUI.RefreshInventory(Inventory.Instance);
                        Debug.Log("Fuel item dequeued after 1 second.");
                    }, 1.5f);
                }
                if (itemName.Contains("Growth"))
                {
                    Debug.Log("You picked up a growth item!");
                    Inventory.Instance.EnqueueItem(new Objects(Objects.ItemType.Growth));
                    FunctionTimer.Create(() => {
                        Inventory.Instance.DequeueItem();
                        itemInventoryUI.RefreshInventory(Inventory.Instance);
                        Debug.Log("Growth item dequeued after 1 second.");
                    }, 1.5f);
                }
                if (itemName.Contains("Bomb"))
                {
                    Debug.Log("You picked up a bomb!");
                    Inventory.Instance.EnqueueItem(new Objects(Objects.ItemType.Bomb));
                    FunctionTimer.Create(() => {
                        Inventory.Instance.DequeueItem();
                        itemInventoryUI.RefreshInventory(Inventory.Instance);
                        Debug.Log("Bomb item dequeued after 1 second.");
                    }, 1.5f);
                }
                if (itemName.Contains("Shield"))
                {
                    Debug.Log("You picked up a shield!");
                    Inventory.Instance.PushPower(new Objects(Objects.PowerType.Shield));
                }
                if (itemName.Contains("Speed"))
                {
                    Debug.Log("You picked up a speed item!");
                    Inventory.Instance.PushPower(new Objects(Objects.PowerType.Speed));
                }
                // Actualizar la UI
                itemInventoryUI.RefreshInventory(Inventory.Instance);
                powerInventoryUI.RefreshInventory(Inventory.Instance);
            }
        }
    }


    private void Update()
    {
        // Verificar si se presiona la tecla L
        if (Input.GetKeyDown(KeyCode.L))
        {
            Objects poppedPower = Inventory.Instance.PopPower();
            if (poppedPower != null)
            {
                Debug.Log($"Popped power: {poppedPower.GetPowerType()}");
            }
            powerInventoryUI.RefreshInventory(Inventory.Instance);
        }
    }
}