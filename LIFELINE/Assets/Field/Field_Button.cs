using UnityEngine;
using System.Collections;

public enum Button_Types {TOGGLE, ONE_SHOT, PRESS};

public class Field_Button : MonoBehaviour {
	public Button_Types button_type;
	public bool isOn = false;
	public bool defOn = false;

	public Sprite buttonup;
	public Sprite buttondown;

	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter2D (Collision2D coll){
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object") {
			if (button_type == Button_Types.TOGGLE) {
				isOn = !isOn;
			}  else {
				isOn = true;
			}
		}
	}

	void OnCollisionStay2D (Collision2D coll){
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object") {
			if (button_type != Button_Types.TOGGLE) {
				isOn = true;
			}
		}
	}

	void OnCollisionExit2D (Collision2D coll){
		if ((coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object") && button_type == Button_Types.PRESS) {
			isOn = false;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = buttondown;
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = buttonup;
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.magenta;
		}
	}
}
