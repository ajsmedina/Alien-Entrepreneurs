using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;




public class Data_Field : MonoBehaviour {

	int unitcount;
	public string levelname;
	public int points = 0;
	public GameObject pointstext;
	public GameObject[] players;
	Color[] colors = { Color.cyan, Color.red, Color.green, Color.yellow };
	public Sprite[] sprites;

	string[] key1 = { "w", "a", "s", "d" };
	string[] key2 = { "i", "j", "k", "l" };
	string[] key3 = { "0", "1", "2", "3" };
	string[] key4 = { "t", "f", "g", "h" };
	string[][] keys;



	// Use this for initialization
	void Start(){
		keys = new string[4][];
		keys [0] = key1;
		keys [1] = key2;
		keys [2] = key3;
		keys [3] = key4;

		List<int> lst = new List<int> { 1, 2, 0, 3 };
		lst = lst.OrderBy(i => Random.value).ToList();
		Debug.Log (lst.ToString ());

		players [0].GetComponent<SpriteRenderer> ().sprite = sprites [lst [0]];
		players [0].GetComponent<SpriteRenderer> ().color = Color.white;
		players [0].GetComponent<LineRenderer> ().SetColors (colors [lst [0]], colors [lst [1]]);
		players [0].GetComponent<Player_Move> ().keybind = keys [lst [0]];

		players [1].GetComponent<SpriteRenderer> ().sprite = sprites [lst [1]];
		players [1].GetComponent<SpriteRenderer> ().color = Color.white;
		players [1].GetComponent<LineRenderer> ().SetColors (colors [lst [1]], colors [lst [0]]);
		players [1].GetComponent<Player_Move> ().keybind = keys [lst [1]];

		players [2].GetComponent<SpriteRenderer> ().sprite = sprites [lst [2]];
		players [2].GetComponent<SpriteRenderer> ().color = Color.white;
		players [2].GetComponent<LineRenderer> ().SetColors (colors [lst [2]], colors [lst [3]]);
		players [2].GetComponent<Player_Move> ().keybind = keys [lst [2]];

		players [3].GetComponent<SpriteRenderer> ().sprite = sprites [lst [3]];
		players [3].GetComponent<SpriteRenderer> ().color = Color.white;
		players [3].GetComponent<LineRenderer> ().SetColors (colors [lst [3]], colors [lst [2]]);
		players [3].GetComponent<Player_Move> ().keybind = keys [lst [3]];
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
		} else if (Input.GetKeyDown ("z"))
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void addPoint(){
		points++;
	}




}
