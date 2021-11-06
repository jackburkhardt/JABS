using System;
using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{

    public float value;
    public bool safe;
    public bool beenChecked;
    public GameHandler GameManager;
    public Rigidbody2D boxRB;

    // Start is called before the first frame update
    void Start()
    {
        boxRB = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // switch to timer system?
        
        if (!beenChecked && !GameManager.holdingBox)
        {
            beenChecked = true;
            StartCoroutine(WaitForSafety());
            Debug.Log(boxRB.mass);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DestroyPlane"))
        {
            Destroy(this.gameObject);
            GameManager.RegisterFall();
        }
    }

    IEnumerator WaitForSafety()
    {
        while (!safe)
        {
            yield return new WaitForSeconds(3);
            //Debug.Log("velo: " + GetComponent<Rigidbody2D>().velocity.magnitude);
            if (boxRB.velocity.magnitude < 0.2)
            {
                GameManager.UpdateScore(value);
                boxRB.mass *= 3;
                safe = true;
            }
        }
        
    }
}
