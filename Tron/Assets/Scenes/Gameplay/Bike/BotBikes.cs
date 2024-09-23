using System;
using System.Collections.Generic;
using UnityEngine;

public class BotBike : Bike
{
    private SpriteRenderer botSpriteRenderer;
    // Nueva propiedad para la dirección inicial
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // Nueva propiedad para la dirección inicial usando el enumerador
    [SerializeField]
    public Direction initialDirection = Direction.Up; // Dirección inicial por defecto hacia arriba


    private void  OnEnable()   
    {
        Item.OnBotObjectUsed += HandleBotItemUsed;
    }

    private void OnDisable()
    {
        Item.OnBotObjectUsed -= HandleBotItemUsed;
    }

    protected void Update()
    {
        if (IsOutOfFuel)
        {
            for (int i = 1; i < _trail.Count; i++)
            {
                Destroy(_trail[i].gameObject);
            }
            _trail.Clear();
        }
    
    }
    private void Start()
    {
        _direction = Vector2.down;
        _trail = new List<Transform>();
        _trail.Add(this.transform);


        // Find and initialize the GridArea component
        _gridArea = FindObjectOfType<GridArea>();
   
        // Obtén el componente SpriteRenderer del GameObject
        botSpriteRenderer = GetComponent<SpriteRenderer>();


        if (_gridArea == null)
        {
            Debug.LogError("GridArea component not found in the scene.");
            return;
        }
        if (botSpriteRenderer != null)
        {
            Debug.Log("Current Bike Color: " + botSpriteRenderer.color);
        }

        // Initialize previous position
        _previousPosition = new Vector2(
            Mathf.Round(this.transform.position.x),
            Mathf.Round(this.transform.position.y)
        );


        // Initialize the trail with the starting length
        Grow(initialTrailLength);

        // Crear una instancia de CustomFixedTimer
        customFixedTimer = gameObject.AddComponent<CustomFixedTimer>();
        customFixedTimer.Initialize(Time.fixedDeltaTime);


        // Establecer la función personalizada a ejecutar
        customFixedTimer.SetFunction(CustomFixedUpdate);
        
        // Establecer la dirección inicial
        SetDirection(initialDirection);

    }


    // Método para establecer la dirección inicial
    private void SetDirection(Direction direction)
    {
        Vector2 directionVector;

        // Convertir la selección del enumerador en un Vector2
        switch (direction)
        {
            case Direction.Up:
                directionVector = Vector2.up;
                break;
            case Direction.Down:
                directionVector = Vector2.down;
                break;
            case Direction.Left:
                directionVector = Vector2.left;
                break;
            case Direction.Right:
                directionVector = Vector2.right;
                break;
            default:
                directionVector = Vector2.up;
                break;
        }

        // Asigna la dirección inicial a la bicicleta
        _direction = directionVector;
    }
    //Ir hacia item mas cercano
    private void MoveTowardsClosestItem()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        if (items.Length == 0) return;

        GameObject closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject item in items)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = item;
            }
        }

        if (closestItem != null)
        {
            Vector2 directionToItem = (closestItem.transform.position - transform.position).normalized;
            SetDirectionBasedOnVector(directionToItem);
        }
    }

    private void SetDirectionBasedOnVector(Vector2 direction)
    {
        Vector2 desiredDirection;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                desiredDirection = Vector2.right;
            }
            else
            {
                desiredDirection = Vector2.left;
            }
        }
        else
        {
            if (direction.y > 0)
            {
                desiredDirection = Vector2.up;
            }
            else
            {
                desiredDirection = Vector2.down;
            }
        }

        // Verificar si la dirección deseada es la opuesta a la dirección actual
        if (IsOppositeDirection(desiredDirection))
        {
            // Hacer un giro en U
            MakeUTurn();
        }
        else
        {
            // Verificar si hay un obstáculo en la dirección deseada
            if (IsObstacleInDirection(desiredDirection))
            {
                ChangeDirectionToAvoidObstacle();
            }
            else
            {
                _direction = desiredDirection;
            }
        }
    }

    private bool IsOppositeDirection(Vector2 direction)
    {
        return (_direction == Vector2.up && direction == Vector2.down) ||
            (_direction == Vector2.down && direction == Vector2.up) ||
            (_direction == Vector2.left && direction == Vector2.right) ||
            (_direction == Vector2.right && direction == Vector2.left);
    }
    private void MakeUTurn()
    {
        // Hacer un giro en U en dos pasos
        if (_direction == Vector2.up)
        {
            _direction = Vector2.right;
            Invoke("CompleteUTurn", 0.5f); // Completar el giro en U después de un breve retraso
        }
        else if (_direction == Vector2.down)
        {
            _direction = Vector2.left;
            Invoke("CompleteUTurn", 0.5f);
        }
        else if (_direction == Vector2.left)
        {
            _direction = Vector2.up;
            Invoke("CompleteUTurn", 0.5f);
        }
        else if (_direction == Vector2.right)
        {
            _direction = Vector2.down;
            Invoke("CompleteUTurn", 0.5f);
        }
    }

    private void CompleteUTurn()
    {
        // Completar el giro en U
        if (_direction == Vector2.right)
        {
            _direction = Vector2.down;
        }
        else if (_direction == Vector2.left)
        {
            _direction = Vector2.up;
        }
        else if (_direction == Vector2.up)
        {
            _direction = Vector2.left;
        }
        else if (_direction == Vector2.down)
        {
            _direction = Vector2.right;
        }
    }

    private bool IsObstacleInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 3.0f);
        return hit.collider != null && hit.collider.CompareTag("W_Obstacle");
    }

    private void ChangeDirectionToAvoidObstacle()
    {
        // Intentar cambiar la dirección para evitar el obstáculo
        if (!IsObstacleInDirection(Vector2.up) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (!IsObstacleInDirection(Vector2.down) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (!IsObstacleInDirection(Vector2.left) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (!IsObstacleInDirection(Vector2.right) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    protected new void CustomFixedUpdate()
    {
        base.CustomFixedUpdate();
        MoveTowardsClosestItem();
    }

    protected override void SetCustomUpdateInterval(float newInterval)
    {
        base.SetCustomUpdateInterval(newInterval);

    }

    protected override void Grow(int growth)
    {
        base.Grow(growth);

    }

    protected new void ResetState()
    {
        base.ResetState();
    }
    private void HandleBotItemUsed(string itemData)
    {
        string[] data = itemData.Split('|');
        string itemName = data[0];
        string botName = data[1];


        Debug.Log("Item used: " + itemName);
        // Aquí puedes agregar la lógica para manejar el uso del ítem en la clase Bike
        if (itemName == "Fuel" && botName == this.gameObject.name)
        {
            if (fuelManager != null)
            {
                fuelManager.AddRandomFuel();
            }
        }
        else if (itemName == "Growth" && botName == this.gameObject.name)
        {
            int randomGrowth = UnityEngine.Random.Range(1, 11);
            Grow(randomGrowth);
        }
        else if (itemName == "Bomb" && botName == this.gameObject.name)
        {
            // Crear una instancia de Active_Bomb en la posición de la cabeza de la serpiente
            if (bombPrefab != null)
            {
                Active_Bomb bomb = Instantiate(bombPrefab);
                bomb.SetPosition(this.transform.position);
            }
        }
        else if (itemName == "Shield" && botName == this.gameObject.name)
        {
            float randomTime = UnityEngine.Random.Range(1f, 11f);
            IsShielded = true;
            botSpriteRenderer.color = new Color(0.118f, 0.565f, 1.000f, 1.000f);
            Debug.Log("Shield activated for " + randomTime + " seconds.");
            FunctionTimer.Create(() => {
                IsShielded = false;
                botSpriteRenderer.color = new Color(0.184f, 0.549f, 0.639f, 1.000f);
            }, randomTime);
        }
        else if (itemName == "Speed" && botName == this.gameObject.name)
        {
            float randomTime = UnityEngine.Random.Range(1f, 11f);
            float newInterval = UnityEngine.Random.Range(0.02f, 0.08f); // Asignar un valor aleatorio a newInterval
            botSpriteRenderer.color = new Color(1.000f, 1.000f, 1.000f, 1.000f);
            Debug.Log("Speed activated for " + randomTime + " seconds at a rate of " + newInterval);
            FunctionTimer.Create(() => {
                variableinterval = 0.08f; // Restablecer el intervalo a su valor original
                botSpriteRenderer.color = new Color(0.184f, 0.549f, 0.639f, 1.000f);
            }, randomTime);
        }
    }

    private void DestroyBot()
    {
            Destroy(gameObject);
            for (int i = 1; i < _trail.Count; i++)
            {
                Destroy(_trail[i].gameObject);
            }
            _trail.Clear();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("W_Obstacle"))
        {
            DestroyBot();
            Debug.Log("Bot Bike collided with an W obstacle.");
        }
        else if (other.CompareTag("Obstacle") && !IsShielded)
        {
            DestroyBot();
            Debug.Log("Bot Bike collided with an Normal obstacle.");
        }
        else if (other.tag == "Player" && !IsShielded)
        {
            DestroyBot();
             Debug.Log("Bot Bike collided with an Player obstacle.");
        }
        else if (other.tag == "Bot" && !IsShielded)
    {
            DestroyBot();
            Debug.Log("Bot Bike collided with an Player obstacle.");
    }

    else if (other.CompareTag("Trail") && !IsShielded)
    {
        // Verificar si el rastro pertenece a este bot
        if (other.transform.parent != transform)
        {
            DestroyBot();
            Debug.Log("Bot Bike collided with a trail.");
        }
        else
        {
            Debug.Log("Bot Bike collided with its own trail.");
        }
    }
}
}