using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Bomb : MonoBehaviour
{
    // Función para establecer la posición del objeto
    public void SetPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    // Función para manejar la colisión con otro objeto
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destruir el objeto si colisiona con otro objeto
        Destroy(gameObject);
    }
}