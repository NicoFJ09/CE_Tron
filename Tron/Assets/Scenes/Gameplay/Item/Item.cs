using UnityEngine;

public class Item : MonoBehaviour
{
    public BoxCollider2D GridArea;

    private void Start() {
        // Load the saved state if it exists
        ItemStateManager.LoadState(this.transform, GridArea);
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.GridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x),Mathf.Round(y),0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player"){
            RandomizePosition();
            ItemStateManager.SaveState(this.transform);
        }
    }
}