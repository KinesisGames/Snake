using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnFood : MonoBehaviour {

	// The food prefab.
	public GameObject foodPrefab;

	// The borders of the game.
	public Transform borderTop;
	public Transform borderRight;
	public Transform borderBottom;
	public Transform borderLeft;

	// TIme between instantiating food prefabs.
	public int interval = 4;

	public void Start () {
    	// Spawn food every <interval> seconds, starting in 3.
    	InvokeRepeating("Spawn", 3, interval);
	}

	void Update() {
		// Check if game is over.
		if (PlayerPrefs.GetInt("gameOver", 0) == 1) {
			// Stop instantiating food prefabs.
			CancelInvoke();
			// Disable this gameObject.
			this.gameObject.SetActive(false);
			// Reset the value of gameOver.
			PlayerPrefs.SetInt("gameOver", 0);
		}
	}

	// Spawn one food prefab.
	void Spawn() {
	    // x position between left & right borders.
	    int x = (int)Random.Range(borderLeft.position.x, borderRight.position.x);

	    // y position between top & bottom borders.
	    int y = (int)Random.Range(borderBottom.position.y, borderTop.position.y);

	    // Instantiate the food at (x, y).
	    Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
	}
}
