using UnityEngine;
using System.Collections;

public class Field_Door : MonoBehaviour {
	public bool open = false;
	public bool defaultopen = false;
	public bool flip = false;
	GameObject player;

	// Use this for initialization
	void Start () {
		defaultopen = this.gameObject.GetComponent<Field_Power> ().defOn;
		open = defaultopen;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color(1.0f, 0.5f, 0.5f );
	}
	
	// Update is called once per frame
	void Update () {
		open = this.gameObject.GetComponent<Field_Power> ().isOn;

		if (flip) {
			open = !open;
		}
		this.gameObject.GetComponent<BoxCollider2D> ().isTrigger = open;


	}

}
