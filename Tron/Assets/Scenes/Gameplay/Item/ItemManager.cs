using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public BoxCollider2D GridArea;
    public GameObject[] itemPrefabs; // Prefabs de los diferentes tipos de ítems
    public int maxItems = 5;
    public int maxInstancesPerItem = 3; // Máximo de instancias por tipo de ítem

    private List<GameObject> items = new List<GameObject>();
    private List<Vector3> occupiedPositions = new List<Vector3>();


    private void Start()
    {
        for (int i = 0; i < maxItems; i++)
        {
            SpawnRandomItem();
        }
    }

    private void SpawnRandomItem()
    {
        if (items.Count >= maxItems) return;

        GameObject itemPrefab = GetRandomItemPrefab();
        if (itemPrefab == null) return; // No se encontró un prefab válido

        GameObject newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        Item itemComponent = newItem.GetComponent<Item>();
        itemComponent.GridArea = GridArea; // Asignar GridArea en tiempo de ejecución
        itemComponent.RandomizePosition(occupiedPositions);
        items.Add(newItem);
        occupiedPositions.Add(newItem.transform.position);

    }

    private GameObject GetRandomItemPrefab()
    {
        List<GameObject> validPrefabs = new List<GameObject>();

        foreach (GameObject prefab in itemPrefabs)
        {
            int count = 0;
            foreach (GameObject item in items)
            {
                if (item.name.Contains(prefab.name))
                {
                    count++;
                }
            }

            if (count < maxInstancesPerItem)
            {
                validPrefabs.Add(prefab);
            }
        }

        if (validPrefabs.Count == 0) return null;

        return validPrefabs[Random.Range(0, validPrefabs.Count)];
    }

    public void ReplaceItem(GameObject item)
    {
        items.Remove(item);
        occupiedPositions.Remove(item.transform.position);
        Destroy(item);
        SpawnRandomItem();
    }
}