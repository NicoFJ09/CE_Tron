using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Bike : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    private List<Transform> _trail;

    public Transform trailprefab;
    private void Start()
    {
        _trail = new List<Transform>();
        _trail.Add(this.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)){
            _direction = Vector2.up;
            }
        else if (Input.GetKeyDown(KeyCode.S)){
            _direction = Vector2.down;
            }
        else if (Input.GetKeyDown(KeyCode.A)){
            _direction = Vector2.left;
            }
        else if (Input.GetKeyDown(KeyCode.D)){
            _direction = Vector2.right;
            }
        }

    private void FixedUpdate()
    {
        for (int i = _trail.Count - 1; i>0; i--) {
            _trail[i].position = _trail[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
        }

    private void Grow()
    {
        Transform trail = Instantiate(this.trailprefab);
        trail.position = _trail[_trail.Count - 1].position;
        _trail.Add(trail);
    }
    
    private void Resetstate()
    {
        for (int i=1; i<_trail.Count; i++)
        {
            Destroy(_trail[i].gameObject);
        }
        _trail.Clear();
        _trail.Add(this.transform);

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item"){
            Grow();
        }
        else if (other.tag == "Obstacle"){
            Resetstate();
        }
    }
}
