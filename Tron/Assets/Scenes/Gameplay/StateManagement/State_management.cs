using System.Collections.Generic;
using UnityEngine;

public static class BikeStateManager
{
    public static void SaveState(Transform bikeTransform, Vector2 direction, List<Transform> trail)
    {
        PlayerPrefs.SetFloat("Bike_PosX", bikeTransform.position.x);
        PlayerPrefs.SetFloat("Bike_PosY", bikeTransform.position.y);
        PlayerPrefs.SetFloat("Bike_DirX", direction.x);
        PlayerPrefs.SetFloat("Bike_DirY", direction.y);
        PlayerPrefs.SetInt("Trail_Length", trail.Count);

        for (int i = 1; i < trail.Count; i++)
        {
            PlayerPrefs.SetFloat($"Trail_{i}_PosX", trail[i].position.x);
            PlayerPrefs.SetFloat($"Trail_{i}_PosY", trail[i].position.y);
        }

        PlayerPrefs.Save();
    }

    public static void LoadState(Transform bikeTransform, ref Vector2 direction, List<Transform> trail, Transform trailPrefab, int initialTrailLength)
    {
        float posX = PlayerPrefs.GetFloat("Bike_PosX", bikeTransform.position.x);
        float posY = PlayerPrefs.GetFloat("Bike_PosY", bikeTransform.position.y);
        float dirX = PlayerPrefs.GetFloat("Bike_DirX", direction.x);
        float dirY = PlayerPrefs.GetFloat("Bike_DirY", direction.y);
        int trailLength = PlayerPrefs.GetInt("Trail_Length", initialTrailLength);

        bikeTransform.position = new Vector2(posX, posY);
        direction = new Vector2(dirX, dirY);

        // Clear existing trail and initialize with saved length
        foreach (Transform segment in trail)
        {
            if (segment != bikeTransform)
            {
                Object.Destroy(segment.gameObject);
            }
        }
        trail.Clear();
        trail.Add(bikeTransform);
        for (int i = 1; i < trailLength+1; i++)
        {
            Transform segment = Object.Instantiate(trailPrefab);
            float trailPosX = PlayerPrefs.GetFloat($"Trail_{i}_PosX", segment.position.x);
            float trailPosY = PlayerPrefs.GetFloat($"Trail_{i}_PosY", segment.position.y);
            segment.position = new Vector2(trailPosX, trailPosY);
            trail.Add(segment);
        }
    }
}


public static class ItemStateManager
{
    public static void SaveState(Transform itemTransform)
    {
        PlayerPrefs.SetFloat("Item_PosX", itemTransform.position.x);
        PlayerPrefs.SetFloat("Item_PosY", itemTransform.position.y);
        PlayerPrefs.Save();
    }

    public static void LoadState(Transform itemTransform, BoxCollider2D gridArea)
    {
        if (PlayerPrefs.HasKey("Item_PosX") && PlayerPrefs.HasKey("Item_PosY"))
        {
            float posX = PlayerPrefs.GetFloat("Item_PosX");
            float posY = PlayerPrefs.GetFloat("Item_PosY");
            itemTransform.position = new Vector3(posX, posY, 0.0f);
        }
        else
        {
            RandomizePosition(itemTransform, gridArea);
        }
    }

    private static void RandomizePosition(Transform itemTransform, BoxCollider2D gridArea)
    {
        Bounds bounds = gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        itemTransform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }
}