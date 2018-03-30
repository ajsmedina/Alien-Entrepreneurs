using UnityEngine;
using System.Collections;

public class Data_Global : MonoBehaviour {
	public int[] hats = { 0, 0, 0, 0 };

	/**
	 * Sets this object to be persistent across all levels.
	 */
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * Updates the current hat for the player
	 */
	public void SetHat(int player, int hat){
		hats [player] = hat;
	}
}
