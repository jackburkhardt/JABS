using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    [Header("Associated object/components")]
    public UIManager UIManager;
    public TilemapRenderer PlatformRenderer;
    public CameraManager mainCamera;
    public GameObject nextBox;
    public Object boxPrefab;
    public AudioHandler AudioManager;
    public Transform boxHolder;
    public Transform boxSpawnTransform;
    
    [Header("Managing fields")]
    public bool firstPlay = true;
    public float score;
    public bool holdingBox;
    public int lives;
    public int dropsTillRise;


    [Header("Adjustable gameplay fields")]
    public float gravity = 1;
    public float rotationAmount = 5f;
    public float transformAmount = 0.5f;
    public float difficulty = 1f;
    
    // Start is called before the first frame update
    void Awake()
    {
        this.enabled = false;
    }

    private void Start()
    {
        //boxPrefab = Resources.Load("Prefabs/BoxPrefab");
    }

    private void Update()
    {

        if (!mainCamera.gameActive || !holdingBox) return;
        
        // INPUT MANAGEMENT

        if (Input.GetKeyDown("space")){ DropBox(); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!mainCamera.gameActive || !holdingBox || nextBox == null) return;
        

        Vector3 nextBoxTransform = nextBox.transform.position;
        if (Input.GetKey("left"))
        {
            nextBox.transform.position = new Vector3(nextBoxTransform.x - transformAmount, nextBoxTransform.y);
        }
        
        if (Input.GetKey("right"))
        {
            nextBox.transform.position = new Vector3(nextBoxTransform.x + transformAmount, nextBoxTransform.y);
        }
        
        if (Input.GetKey("up"))
        {
            nextBox.transform.Rotate(transform.forward, rotationAmount);
        }
        
        if (Input.GetKey("down"))
        {
            nextBox.transform.Rotate(transform.forward, -rotationAmount);
        }


    }

    void DropBox()
    {
        holdingBox = false;
       // mainCamera.FollowTransform = nextBox.transform;
        difficulty += Random.Range(0.1f, 0.3f);
        //disconnect box from joint & spawn new box
        // TODO: add side colliders
        nextBox.GetComponent<Rigidbody2D>().gravityScale = gravity;
        StartCoroutine(SpawnBox());
    }

    // Am I happy with the amount of hardcoded values that this monstrosity now has? No.
    // But 11:59PM on Monday is getting a liiiittle too close.
    IEnumerator SpawnBox()
    {
        yield return new WaitForSeconds(2f);

        boxSpawnTransform.position = new Vector3(0, 4.5f + FindHighestBox(), 0);
        var newBox = (GameObject)Instantiate(boxPrefab, boxSpawnTransform.position, Quaternion.identity, boxHolder);
        Transform nbTransform = newBox.transform;
        nbTransform.localScale = new Vector3(Random.Range(1.2f, 8f), Random.Range(1.2f, 8f));
        newBox.GetComponent<Box>().GameManager = this;
        var newMass = (nbTransform.localScale.x * nbTransform.localScale.y) / (5 / difficulty);
        newBox.GetComponent<Rigidbody2D>().mass = newMass;
        newBox.GetComponent<Box>().value = difficulty * (newMass / 2);
        
        nextBox = newBox;
        dropsTillRise -= 1;
        if (dropsTillRise == 0)
        {
            mainCamera.FollowTransform.position = new Vector3(0,
                FindHighestBox() - 4, 0);
            dropsTillRise = 4;
        }

        holdingBox = true;

    }

    public void ReceiveStart()
    {
        if (firstPlay) firstPlay = false;
        UIManager.livesText.text = "Lives: " + lives;
        UIManager.scoreText.text = "Score: " + (int)score;
        PlatformRenderer.enabled = true;
        mainCamera.gameActive = true;
        StartCoroutine(SpawnBox());
    }

    private void GameOver()
    {
        if (!mainCamera.gameActive) return;
        AudioManager.Play("lose");
        mainCamera.gameActive = false;
        foreach (var box in FindObjectsOfType<Rigidbody2D>())
        {
            box.gravityScale = 0.2f;
        }
        UIManager.GameOver(score);
    }

    public void UpdateScore(float val)
    {
        score += val;
        AudioManager.Play("score");
        UIManager.scoreText.text = "Score: " + (int)score;
    }

    public void RegisterFall()
    {
        lives -= 1;
        UIManager.livesText.text = "Lives: " + lives;
        if (lives == 0) {GameOver();}
        else if (lives >= 1) { AudioManager.Play("lose_life"); }
    }

    public void ResetGame()
    {
        score = 0;
        lives = 3;
        dropsTillRise = 4;
        difficulty = 1;
        UIManager.scoreText.text = "Score: " + (int)score;
        UIManager.livesText.text = "Lives: " + lives;
        foreach (var box in FindObjectsOfType<Box>())
        {
            Destroy(box.gameObject);
            //Debug.Log("shit just got exploded");
        }
        
        mainCamera.FollowTransform.position = new Vector3(0, -2, 0); 
        mainCamera.gameActive = true;
        StartCoroutine(SpawnBox());
    }

    float FindHighestBox()
    {
        // LINQ expression
        return (from Transform box in boxHolder select box.position.y).Prepend(0).Max();
    }
}
