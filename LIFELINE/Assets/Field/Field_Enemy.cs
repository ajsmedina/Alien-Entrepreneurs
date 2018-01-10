using UnityEngine;
using System.Collections;

public enum Field_Movement {Stay};

public class Field_Enemy : MonoBehaviour {
	public int encounterid;
	public bool fought;
	public Field_Movement move;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Set_Fought(){
		GetComponent<BoxCollider2D> ().isTrigger = fought;
		GetComponent<SpriteRenderer> ().enabled = !fought;
	}
}
