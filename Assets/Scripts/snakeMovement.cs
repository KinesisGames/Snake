using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class snakeMovement : MonoBehaviour {

	// Current Movement Direction.
    Vector2 dir;

    bool gameStart = false;

    // Keep Track of Tail.
	List<Transform> tails = new List<Transform>();

	// Did the snake eat something?
	bool ate = false;

	// Tag of the 'food' prefab that gets instantiated.
	public string foodTag = "food";

	// Tag of the 'tail' prefab that gets instantiated.
	public string tailTag = "tail";

	// Tail Prefab.
	public GameObject tailPrefab;

	// Arrows for changing direction.
	// Up -> Right -> Down -> Left
	public Button[] arrows;

	// Amount to set alpha of movement buttons when they should be 'disabled'.
	public float transparency = 0.65f;

	// Game Over gameObject.
	public GameObject gameOver;

	// Current score.
	private int score = 0;

	// High score.
	private int highScore;

	// Current Score Text.
	public Text scoreText;

	// High Score Text.
	public Text highScoreText;

	void Awake() {
		// Reset the value of gameOver.
		PlayerPrefs.SetInt("gameOver", 0);
	}

    public void manualMove(string choice) {
    	// Don't move if the game is still in state of game over.
    	if (gameOver.activeSelf) {
    		return;
    	}

    	// Indicate that the game has started.
    	if (!gameStart) {
    		gameStart = true;

    		// Move the Snake every 0.4s as soon as the game starts.
    		InvokeRepeating("snakeMove", 0.4f, 0.4f);
    	}

    	// Enable all the movement buttons.
    	foreach (Button arrow in arrows) {
    		arrow.enabled = true;
    		arrow.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
    	}

    	// Move in a new Direction?
    	// Also, disable the appropriate movement button.
	    if (choice == "up") {
	        dir = Vector2.up;
	        arrows[0].enabled = false;
	        arrows[2].enabled = false;
	        arrows[0].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	        arrows[2].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	    } else if (choice == "right") {
	        dir = Vector2.right;
	        arrows[1].enabled = false;
	        arrows[3].enabled = false;
	        arrows[1].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	        arrows[3].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	    } else if (choice == "down") {
	        dir = -Vector2.up;
	        arrows[2].enabled = false;
	        arrows[0].enabled = false;
	        arrows[2].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	        arrows[0].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	    } else if (choice == "left") {
	        dir = -Vector2.right;
	    	arrows[3].enabled = false;
	    	arrows[1].enabled = false;
	        arrows[3].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	        arrows[1].GetComponent<CanvasRenderer>().SetAlpha(transparency);
	    }
    }

    void OnTriggerEnter2D(Collider2D col) {
	    // Food?
	    if (col.gameObject.CompareTag(foodTag)) {
	        // Get longer in next Move call.
	        ate = true;
	        // Remove the Food.
	        Destroy(col.gameObject);

	        // Increment and Display Score.
	        score++;
	        scoreText.text = score.ToString();
	    }
	    // Collided with Tail or Border.
	    else {
	    	// Player lost.
	    	playerLose();
	    }
	}

    void snakeMove() {
        // Save current position (gap will be here).
	    Vector2 pos = transform.position;

	    // Move head into new direction (now there is a gap).
	    transform.Translate(dir);

	    // Ate something? Then insert new Element into gap.
	    if (ate) {
	        // Load Prefab into the scene.
	        GameObject newTail = (GameObject)Instantiate(tailPrefab, pos, Quaternion.identity);

	        // Keep track of it in our tail list.
	        tails.Insert(0, newTail.transform);

	        // Reset the flag
	        ate = false;
	    } 
	    // Do we have a Tail?
	    else if (tails.Count > 0) {
	        // Move last Tail Element to where the Head was
	        tails.Last().position = pos;

	        // Add to front of list, remove from the back
	        tails.Insert(0, tails.Last());
	        tails.RemoveAt(tails.Count-1);
	    }
    }

    void playerLose() {
    	// Indicate that the game is over.
    	PlayerPrefs.SetInt("gameOver", 1);

    	// Delete all the instantiated objects.
    	multiDestroy();

        // Stop snake from moving and reset its position.
        gameStart = false;
        CancelInvoke();
        this.transform.position = new Vector3(0, 5, 0);

        // Activate the Game Over gameObject.
        gameOver.SetActive(true);

        // Get the high score.
        highScore = PlayerPrefs.GetInt("highScore", 0);

        // Check if there is a new high score.
        if (score > highScore) {
        	// Set and store the high score as the value of score.
        	highScore = score;
        	PlayerPrefs.SetInt("highScore", highScore);
        }

        // Give a value to the High Score Text.
        highScoreText.text = highScore.ToString();

        // Reset the current score.
        score = 0;
    }

    void multiDestroy() {
    	// Get a list of all the instantiated 'food' in the scene.
    	GameObject[] allFood = GameObject.FindGameObjectsWithTag(foodTag);
    	foreach (GameObject food in allFood) {
    		// Destroy all the instantiated 'food' in the scene.
    		Destroy(food);
    	}

    	// Get a list of all the instantiated 'tail' in the scene.
    	GameObject[] allTail = GameObject.FindGameObjectsWithTag(tailTag);
    	foreach (GameObject tail in allTail) {
    		// Destroy all the instantiated 'tail' in the scene.
    		Destroy(tail);
    	}

    	// Reset the tails list.
    	tails = new List<Transform>();
    }
}
