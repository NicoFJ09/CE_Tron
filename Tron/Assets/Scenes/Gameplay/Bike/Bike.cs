using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Bike : MonoBehaviour
{
    public event Action<Vector2, Vector2> OnPositionChanged;
    private SpriteRenderer bikeSpriteRenderer;


    protected Vector2 _direction;
    protected List<Transform> _trail;
    protected GridArea _gridArea;  // Reference to the GridArea component
    public Transform trailPrefab;
    public int initialTrailLength = 3;  // Starting length of the trail set to 3


    protected Vector2 _previousPosition;
    public float TotalDistanceMoved { get; private set; } = 0f;


    public bool IsOutOfFuel { get; set; } = false;


    public FuelManager fuelManager;


    public Active_Bomb bombPrefab;
    public bool IsShielded { get; set; } = false;
    protected CustomFixedTimer customFixedTimer;
    protected float variableinterval = 0.08f;


    private void OnEnable()
    {
        Item.OnObjectUsed += HandleItemUsed;
    }


    private void OnDisable()
    {
        Item.OnObjectUsed -= HandleItemUsed;
    }


    private void Start()
    {
        _direction = Vector2.down;
        _trail = new List<Transform>();
        _trail.Add(this.transform);


        // Find and initialize the GridArea component
        _gridArea = FindObjectOfType<GridArea>();
   
        // Obtén el componente SpriteRenderer del GameObject
        bikeSpriteRenderer = GetComponent<SpriteRenderer>();


        if (_gridArea == null)
        {
            Debug.LogError("GridArea component not found in the scene.");
            return;
        }
        if (bikeSpriteRenderer != null)
        {
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
        
    }


    private void Update()
    {
        if (IsOutOfFuel)
        {
            ResetState();
            SceneManager.LoadScene(3);
        }


        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }


    protected virtual void CustomFixedUpdate()
        
    {
        SetCustomUpdateInterval(variableinterval);
        if (_gridArea == null)
        {
            return;
        }


        // Move the trail
        for (int i = _trail.Count - 1; i > 0; i--)
        {
            _trail[i].position = _trail[i - 1].position;
        }


        // Calculate the bike's new position based on the direction
        Vector2 currentPosition = new Vector2(
            Mathf.Round(this.transform.position.x),
            Mathf.Round(this.transform.position.y)
        );


        // Calculate new position on the grid
        Vector2 newPosition = currentPosition + _direction;


        // Map newPosition to the grid bounds
        Vector2Int gridIndex = _gridArea.CoordinateToIndex(new Vector2Int(
            Mathf.RoundToInt(newPosition.x),
            Mathf.RoundToInt(newPosition.y)
        ));


        // Ensure the position stays within bounds, allowing for extra space
        gridIndex.x = Mathf.Clamp(gridIndex.x, -1, _gridArea.Width);
        gridIndex.y = Mathf.Clamp(gridIndex.y, -1, _gridArea.Height);


        // Update the bike's position
        Vector2 adjustedPosition = _gridArea.IndexToCoordinate(gridIndex);
        this.transform.position = new Vector3(adjustedPosition.x, adjustedPosition.y, 0.0f);


        // Calculate and update the total distance moved
        float distanceMoved = Vector2.Distance(_previousPosition, adjustedPosition);
        TotalDistanceMoved += distanceMoved;


        // Trigger the event
        OnPositionChanged?.Invoke(adjustedPosition, _previousPosition);


        _previousPosition = adjustedPosition;
    }

    protected virtual void SetCustomUpdateInterval(float newInterval)
    {
        if (customFixedTimer != null)
        {
            customFixedTimer.SetInterval(newInterval);
        }
    }


    protected virtual void Grow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform trail = Instantiate(this.trailPrefab);
            trail.position = _trail[_trail.Count - 1].position;
            _trail.Add(trail);
        }
    }
   
    protected void ResetState()
    {
        for (int i = 1; i < _trail.Count; i++)
        {
            Destroy(_trail[i].gameObject);
        }
        _trail.Clear();
        _trail.Add(this.transform);


        this.transform.position = Vector3.zero;


        // Reinitialize the trail with the starting length
            Grow(initialTrailLength);


        // Reset the distance moved
        TotalDistanceMoved = 0f;
        _previousPosition = new Vector2(
            Mathf.Round(this.transform.position.x),
            Mathf.Round(this.transform.position.y)
        );
    }


    private void HandleItemUsed(string itemName)
    {
        // Aquí puedes agregar la lógica para manejar el uso del ítem en la clase Bike
        if (itemName == "Fuel")
        {
            if (fuelManager != null)
            {
                fuelManager.AddRandomFuel();
            }
        }
        else if (itemName == "Growth")
        {
            int randomGrowth = UnityEngine.Random.Range(1, 11);
            Grow(randomGrowth);
        }


        else if (itemName == "Bomb")
        {
            // Crear una instancia de Active_Bomb en la posición de la cabeza de la serpiente
            if (bombPrefab != null)
            {
                Active_Bomb bomb = Instantiate(bombPrefab);
                bomb.SetPosition(this.transform.position);
            }
        }
        else if (itemName == "Shield")
        {
            float randomTime = UnityEngine.Random.Range(1f, 11f);
            IsShielded = true;
            bikeSpriteRenderer.color = new Color(0.118f, 0.565f, 1.000f, 1.000f);
            FunctionTimer.Create(() => {
                IsShielded = false;
                bikeSpriteRenderer.color = new Color(0.957f, 0.686f, 0.176f, 1.000f);
            }, randomTime);
        }
        else if (itemName == "Speed")
        {
            float randomTime = UnityEngine.Random.Range(1f, 11f);
            variableinterval = UnityEngine.Random.Range(2, 8); 
            variableinterval = variableinterval / 100f; // Asegurarse de que la división sea con un float
            bikeSpriteRenderer.color = new Color(1.000f, 1.000f, 1.000f, 1.000f);
            FunctionTimer.Create(() => {
                variableinterval = 0.08f;
                bikeSpriteRenderer.color = new Color(0.957f, 0.686f, 0.176f, 1.000f);
            }, randomTime);
        }
    }


private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("W_Obstacle"))
    {
        ResetState();
        SceneManager.LoadScene(3);
    }
    else if (other.CompareTag("Obstacle") && !IsShielded){
        ResetState();
        SceneManager.LoadScene(3);
    }
    else if (other.CompareTag("Bot") && !IsShielded){
        ResetState();
        SceneManager.LoadScene(3);
    }
    else if (other.CompareTag("Trail") && !IsShielded)
    {
        ResetState();
        SceneManager.LoadScene(3);
    }
}
}

