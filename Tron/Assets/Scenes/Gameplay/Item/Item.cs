using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
public class Item : MonoBehaviour
{
    public BoxCollider2D GridArea;
    [SerializeField] private ItemInventoryUI itemInventoryUI;
    [SerializeField] private PowerInventoryUI powerInventoryUI;

    public static event Action<string> OnObjectUsed;
    public static event Action<string> OnBotObjectUsed;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Restablecer las pilas cuando se carga una nueva escena
        Inventory.Instance.Reset();
        itemInventoryUI.RefreshInventory(Inventory.Instance);
        powerInventoryUI.RefreshInventory(Inventory.Instance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
        Objects poppedPower = Inventory.Instance.PopPower();
        if (poppedPower != null)
        {
            OnObjectUsed?.Invoke($"{poppedPower}");
        }
        powerInventoryUI.RefreshInventory(Inventory.Instance);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
        Inventory.Instance.ShufflePowerStack();
        powerInventoryUI.RefreshInventory(Inventory.Instance);
        }
    }

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
            float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
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
                    Inventory.Instance.EnqueueItem(new Objects(Objects.ItemType.Fuel));
                    FunctionTimer.Create(() => {
                        Inventory.Instance.DequeueItem();
                        itemInventoryUI.RefreshInventory(Inventory.Instance);
                        OnObjectUsed?.Invoke("Fuel");
                    }, 1.5f);
                }
                if (itemName.Contains("Growth"))
                {
                    Inventory.Instance.EnqueueItem(new Objects(Objects.ItemType.Growth));
                    FunctionTimer.Create(() => {
                        Inventory.Instance.DequeueItem();
                        itemInventoryUI.RefreshInventory(Inventory.Instance);
                        OnObjectUsed?.Invoke("Growth");
                    }, 1.5f);
                }
                if (itemName.Contains("Bomb"))
                {
                    Inventory.Instance.EnqueueItem(new Objects(Objects.ItemType.Bomb));
                    FunctionTimer.Create(() => {
                        Inventory.Instance.DequeueItem();
                        itemInventoryUI.RefreshInventory(Inventory.Instance);
                        OnObjectUsed?.Invoke("Bomb");
                    }, 1.5f);
                }
                if (itemName.Contains("Shield"))
                {
                    Inventory.Instance.PushPower(new Objects(Objects.PowerType.Shield));
                }
                if (itemName.Contains("Speed"))
                {
                    Inventory.Instance.PushPower(new Objects(Objects.PowerType.Speed));
                }
                // Actualizar la UI
                itemInventoryUI.RefreshInventory(Inventory.Instance);
                powerInventoryUI.RefreshInventory(Inventory.Instance);
            }
        }
        if (other.tag == "Bot")
        {
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.ReplaceItem(this.gameObject);
                string itemName = this.gameObject.name;
                string botName = other.gameObject.name;

                if(itemName.Contains("Fuel"))
                {
                    FunctionTimer.Create(() => {
                        OnBotObjectUsed?.Invoke($"Fuel|{botName}");
                    }, 1.5f);
                }
                if(itemName.Contains("Growth"))
                {
                    FunctionTimer.Create(() => {
                        OnBotObjectUsed?.Invoke($"Growth|{botName}");
                    }, 1.5f);
                }
                if(itemName.Contains("Bomb"))
                {
                    FunctionTimer.Create(() => {
                        OnBotObjectUsed?.Invoke($"Bomb|{botName}");
                    }, 1.5f);
                }
                if(itemName.Contains("Shield"))
                {
                    FunctionTimer.Create(() => {
                        OnBotObjectUsed?.Invoke($"Shield|{botName}");
                    }, 1.5f);
                }
            }
        }
    }

}