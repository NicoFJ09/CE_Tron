using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // Singleton instance
    private static Inventory instance;

    // Definir cola para items y pila para poderes
    private Queue<Objects> itemQueue;
    private Stack<Objects> powerStack;

    private const int MaxItems = 3; // Máximo de elementos en la cola
    private const int MaxPowers = 3; // Máximo de elementos en la pila
    private bool canPopPower = true;
    private bool canShufflePower = true;
    private float popPowerCooldown = 0.5f; // Tiempo de espera entre llamadas a PopPower
    private float shufflePowerCooldown = 0.5f; // Tiempo de espera entre llamadas a ShufflePowerStack

    // Constructor privado para evitar instanciación directa
    private Inventory()
    {
        // Inicializar cola y pila
        itemQueue = new Queue<Objects>();
        powerStack = new Stack<Objects>();
    }

    // Método para obtener la instancia Singleton
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Inventory();
            }
            return instance;
        }
    }

    // Métodos para manipular la cola de items
    public void EnqueueItem(Objects item)
    {
        if (itemQueue.Count >= MaxItems)
        {
            Debug.LogWarning("No se puede añadir más items al inventario. Límite alcanzado.");
            return; // Ignorar el nuevo item
        }
        itemQueue.Enqueue(item);

        // Imprimir el contenido de la cola de items en una sola línea
        Debug.Log("Items en la cola: " + string.Join(", ", itemQueue));
    }

    public Objects DequeueItem()
    {
        if (itemQueue.Count > 0)
        {
            return itemQueue.Dequeue();
        }
        else
        {
            return null; // Ignorar y pasar si la cola está vacía
        }
    }


    // Métodos para manipular la pila de poderes
    public void PushPower(Objects power)
    {
        if (powerStack.Count >= MaxPowers)
        {
            Debug.LogWarning("No se puede añadir más poderes al inventario. Límite alcanzado.");
            return; // Ignorar el nuevo poder
        }
        powerStack.Push(power);
    }

    public Objects PopPower()
    {
        if (!canPopPower)
        {
            return null;
        }

        int initialCount = powerStack.Count;
        if (initialCount > 0)
        {
            Objects poppedPower = powerStack.Pop();
            int finalCount = powerStack.Count;

            // Iniciar el temporizador para evitar llamadas repetidas
            canPopPower = false;
            FunctionTimer.Create(() => {
                canPopPower = true;
            }, popPowerCooldown);

            return poppedPower;
        }

        return null;
    }

    public void ShufflePowerStack()
    {
        if (!canShufflePower)
        {
            return;
        }

        if (powerStack.Count > 1)
        {
            // Convertir la pila en una lista para facilitar la manipulación
            List<Objects> powerList = new List<Objects>(powerStack);

            // Eliminar el último elemento y añadirlo al principio
            Objects lastPower = powerList[powerList.Count - 1];
            powerList.RemoveAt(powerList.Count - 1);
            powerList.Insert(0, lastPower);

            // Limpiar la pila original y volver a llenarla con los elementos reorganizados
            powerStack.Clear();
            for (int i = powerList.Count - 1; i >= 0; i--)
            {
                powerStack.Push(powerList[i]);
            }

            Debug.Log("Pila de poderes reorganizada: " + string.Join(", ", powerStack));
        }

        // Iniciar el temporizador para evitar llamadas repetidas
        canShufflePower = false;
        FunctionTimer.Create(() => {
            canShufflePower = true;
        }, shufflePowerCooldown);
    }

    public void Reset()
    {
        itemQueue.Clear();
        powerStack.Clear();
    }
    // Métodos para obtener la cola de items y la pila de poderes
    public IEnumerable<Objects> GetItemQueue()
    {
        return itemQueue;
    }

    public IEnumerable<Objects> GetPowerStack()
    {
        return powerStack;
    }
}