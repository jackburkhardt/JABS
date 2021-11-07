using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveBox : MonoBehaviour
{
    // Start is called before the first frame update
    public bool direction = true;
    public int range;
    private Vector3 Position;
    public bool moving;
    public GameHandler GameManager;
    
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!moving) return;
        
        Position = transform.position;
        if (direction)
        {
            transform.position = new Vector3(Position.x + 0.05f * (GameManager.difficulty/2), Position.y, Position.z);
            if (Position.x >= range) { direction = false; }
        }
        else
        {
            transform.position = new Vector3(Position.x - 0.05f * (GameManager.difficulty /2), Position.y, Position.z);
            if (Position.x <= -range) { direction = true; }
        }
    }

    public void EnableMovingPlatform()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        moving = true;
    }
}
