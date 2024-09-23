using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Bomb : MonoBehaviour
{
    private BoxCollider2D bombCollider;

    private void Start()
    {
        bombCollider = GetComponent<BoxCollider2D>();
        if (bombCollider != null)
        {
            bombCollider.enabled = false; // Desactivar el collider inicialmente
            FunctionTimer.Create(EnableCollider, 0.1f); // Activar el collider después de 0.5 segundos usando FunctionTimer
        }
    }

    // Función para activar el collider
    private void EnableCollider()
    {
        bombCollider.enabled = true;
    }

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