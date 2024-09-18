using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bike : MonoBehaviour
{
    public event Action<Vector2, Vector2> OnPositionChanged;

    private Vector2 _direction = Vector2.right;
    private List<Transform> _trail;
    private GridArea _gridArea;  // Reference to the GridArea component
    public Transform trailPrefab;
    public int initialTrailLength = 3;  // Starting length of the trail set to 3

    private Vector2 _previousPosition;
    public float TotalDistanceMoved { get; private set; } = 0f;

    public bool IsOutOfFuel { get; set; } = false;
    

    private void Start()
    {
        _trail = new List<Transform>();
        _trail.Add(this.transform);

        // Find and initialize the GridArea component
        _gridArea = FindObjectOfType<GridArea>();
        if (_gridArea == null)
        {
            Debug.LogError("GridArea component not found in the scene.");
            return;
        }

        // Initialize previous position
        _previousPosition = new Vector2(
            Mathf.Round(this.transform.position.x),
            Mathf.Round(this.transform.position.y)
        );

        // Initialize the trail with the starting length
        for (int i = 0; i < initialTrailLength; i++)
        {
            Grow();
        }

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

    private void FixedUpdate()
    {
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

    private void Grow()
    {
        Transform trail = Instantiate(this.trailPrefab);
        trail.position = _trail[_trail.Count - 1].position;
        _trail.Add(trail);
    }
    
    private void ResetState()
    {
        for (int i = 1; i < _trail.Count; i++)
        {
            Destroy(_trail[i].gameObject);
        }
        _trail.Clear();
        _trail.Add(this.transform);

        this.transform.position = Vector3.zero;

        // Reinitialize the trail with the starting length
        for (int i = 0; i < initialTrailLength; i++)
        {
            Grow();
        }

        // Reset the distance moved
        TotalDistanceMoved = 0f;
        _previousPosition = new Vector2(
            Mathf.Round(this.transform.position.x),
            Mathf.Round(this.transform.position.y)
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
            SceneManager.LoadScene(3);
        }
    }
}