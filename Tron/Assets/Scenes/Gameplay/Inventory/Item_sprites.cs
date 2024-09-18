using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects
{
    // OBJECT MANAGEMENT FOR INVENTORY

    public enum ItemType
    {
        Fuel,
        Growth,
        Bomb
    }

    public enum PowerType
    {
        Shield,
        Speed
    }

    private ItemType? itemType;
    private PowerType? powerType;

    public Objects(ItemType itemType)
    {
        this.itemType = itemType;
        this.powerType = null;
    }

    public Objects(PowerType powerType)
    {
        this.powerType = powerType;
        this.itemType = null;
    }

    public ItemType? GetItemType()
    {
        return itemType;
    }

    public PowerType? GetPowerType()
    {
        return powerType;
    }

    public Sprite GetItemSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Fuel: return ItemAssets.Instance.fuelSprite;
            case ItemType.Growth: return ItemAssets.Instance.growthSprite;
            case ItemType.Bomb: return ItemAssets.Instance.bombSprite;
        }
    }

    public Sprite GetPowerSprite()
    {
        switch (powerType)
        {
            default:
            case PowerType.Shield: return ItemAssets.Instance.shieldSprite;
            case PowerType.Speed: return ItemAssets.Instance.speedSprite;
        }
    }

    public override string ToString()
    {
        if (itemType.HasValue)
        {
            return itemType.Value.ToString();
        }
        else if (powerType.HasValue)
        {
            return powerType.Value.ToString();
        }
        else
        {
            return "Unknown";
        }
    }
}