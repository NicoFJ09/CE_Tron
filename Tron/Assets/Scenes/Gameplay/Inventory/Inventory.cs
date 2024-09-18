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
    private const float popPowerCooldown = 0.5f; // Cooldown en segundos

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
        return itemQueue.Dequeue();
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
        Debug.Log("Cantidad de poderes en la pila: " + powerStack.Count);

        // Imprimir el contenido de la pila de poderes en una sola línea
        Debug.Log("Poderes en la pila: " + string.Join(", ", powerStack));
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
            Debug.Log($"Power stack count reduced from {initialCount} to {finalCount}");

            // Iniciar el temporizador para evitar llamadas repetidas
            canPopPower = false;
            FunctionTimer.Create(() => {
                canPopPower = true;
            }, popPowerCooldown);

            return poppedPower;
        }

        return null;
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