using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public Sprite fuelSprite;
    public Sprite growthSprite;
    public Sprite bombSprite;
    public Sprite shieldSprite;
    public Sprite speedSprite;

    private void Awake()
    {
        Instance = this;
    }
}