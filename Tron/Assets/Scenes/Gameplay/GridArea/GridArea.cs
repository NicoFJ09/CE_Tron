using System.Collections.Generic;
using UnityEngine;

// Node class for linked list
public class Node
{
    public Vector2Int position;  // Position on the grid
    public Node next;            // Reference to the next node in the list
    public Color color;          // Color of the node
    public GameObject tileObject; // GameObject representing the tile

    public Node(Vector2Int position, Color color)
    {
        this.position = position;
        this.color = color;
        this.next = null;
        this.tileObject = null;
    }
}

// LinkedList class
public class LinkedList
{
    public Node head;

    public LinkedList()
    {
        head = null;
    }

    // Method to add a new node to the linked list
    public void AddNode(Vector2Int position, Color color)
    {
        Node newNode = new Node(position, color);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node current = head;
            while (current.next != null)
            {
                current = current.next;
            }
            current.next = newNode;
        }
    }

    // Method to print all positions in the linked list
    public void PrintAllNodes()
    {
        Node current = head;
        while (current != null)
        {
            Debug.Log($"Position: {current.position}, Color: {current.color}");
            current = current.next;
        }
    }
}


// GridArea class that uses a linked list array
public class GridArea : MonoBehaviour
{
    public int Width { get; private set; }  // Width in grid units
    public int Height { get; private set; } // Height in grid units

    private LinkedList[,] gridArray;  // Array of linked lists
    private int offsetX;              // Offset to handle negative coordinates
    private int offsetY;              // Offset to handle negative coordinates

    // Method to initialize grid using BoxCollider2D dimensions
    public void InitializeGridFromCollider()
    {
        // Get the BoxCollider2D component attached to the Grid GameObject
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        // Check if the BoxCollider2D is not null to avoid potential null reference errors
        if (boxCollider != null)
        {
            // Calculate the width and height in world units, considering the object's scale
            float worldWidth = boxCollider.size.x * transform.lossyScale.x;
            float worldHeight = boxCollider.size.y * transform.lossyScale.y;

            // Convert world units to grid units (assuming 1 unit in the game world equals 1 grid cell)
            Width = Mathf.RoundToInt(worldWidth) + 1;  // Adding 1 for extra space
            Height = Mathf.RoundToInt(worldHeight) + 1; // Adding 1 for extra space

            // Calculate offsets to handle negative coordinates
            offsetX = Width / 2;
            offsetY = Height / 2;

            // Initialize the grid array with the calculated dimensions
            gridArray = new LinkedList[Width, Height];

            // Initialize each linked list in the grid
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    gridArray[x, y] = new LinkedList();
                }
            }

            Debug.Log($"Grid initialized with Width: {Width}, Height: {Height}, OffsetX: {offsetX}, OffsetY: {offsetY}");
        }
        else
        {
            Debug.LogError("BoxCollider2D component is missing on the Grid GameObject.");
        }
    }

    private void Start()
    {
        // Initialize the grid based on the BoxCollider2D when the game starts
        InitializeGridFromCollider();
    }

    // Method to convert negative coordinates to grid array indices
    public Vector2Int CoordinateToIndex(Vector2Int coord)
    {
        int xIndex = coord.x + offsetX;
        int yIndex = coord.y + offsetY;
        return new Vector2Int(xIndex, yIndex);
    }

    // Method to convert grid array indices to negative coordinates
    public Vector2Int IndexToCoordinate(Vector2Int index)
    {
        int xCoord = index.x - offsetX;
        int yCoord = index.y - offsetY;
        return new Vector2Int(xCoord, yCoord);
    }

    // Method to get the linked list at a specific coordinate
    public LinkedList GetLinkedListAt(Vector2Int coord)
    {
        Vector2Int index = CoordinateToIndex(coord);
        if (index.x >= 0 && index.x < Width && index.y >= 0 && index.y < Height)
        {
            return gridArray[index.x, index.y];
        }
        else
        {
            Debug.LogError("Coordinate out of bounds");
            return null;
        }
    }

    // Method to get the grid array (for potential future use)
    public LinkedList[,] GetGridArray()
    {
        return gridArray;
    }
}