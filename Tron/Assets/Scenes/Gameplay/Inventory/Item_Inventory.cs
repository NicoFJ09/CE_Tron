using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ItemInventoryUI : MonoBehaviour
{
    private Inventory inventory;

    public Transform itemSlotContainer;
    public float slotCellSize = 40f;

    private void Awake()
    {
        itemSlotContainer = transform.Find("Itemslot");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventory(inventory);
    }

    public void RefreshInventory(Inventory inventory)
    {
        RefreshInventorySlots(inventory.GetItemQueue(), itemSlotContainer, item => item.GetItemSprite());
    }

    private void RefreshInventorySlots<T>(IEnumerable<T> items, Transform slotContainer, Func<T, Sprite> getSprite)
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        int x = 0;
        float[] slotPositions = { 0f, 52f, 104f };

        foreach (T item in items)
        {
            GameObject slot = new GameObject("Slot", typeof(RectTransform), typeof(Image));
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slot.transform.SetParent(slotContainer, false);
            slotRectTransform.sizeDelta = new Vector2(slotCellSize, slotCellSize);
            slotRectTransform.anchoredPosition = new Vector2(slotPositions[x], 0);

            Image image = slot.GetComponent<Image>();
            image.sprite = getSprite(item);

            x++;
            if (x > 2)
            {
                break;
            }
        }
    }
}

