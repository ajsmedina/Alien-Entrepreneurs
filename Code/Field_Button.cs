using UnityEngine;
using System.Collections;

/**
 * Button_Types
 * TOGGLE: Hit once to turn on, hit again to turn off.
 * ONE_SHOT: Hit once to turn on. Cannot turn off.
 * PRESS: Stay on button to keep it on. Turns off when object leaves.
 */
public enum Button_Types {TOGGLE, ONE_SHOT, PRESS};

public class Field_Button : MonoBehaviour {
	public Button_Types button_type;
	public bool isOn = false;
	public bool defOn = false;

	public Sprite buttonup;
	public Sprite buttondown;

	/**
	 * Toggle or turn on button when it collides with another object.
	 * coll: collider of other object
	 */
	void OnCollisionEnter2D (Collision2D coll){
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object") {
			if (button_type == Button_Types.TOGGLE) {
				isOn = !isOn;
			}  else {
				isOn = true;
			}
		}
	}

	/**
	 * Unless the button is set to toggle, keep it on.
	 * Ths function is necessary otherwise if one object leaves but one stays, it would turn off the button.
	 * coll: collider of other object
	 */
	void OnCollisionStay2D (Collision2D coll){
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object") {
			if (button_type != Button_Types.TOGGLE) {
				isOn = true;
			}
		}
	}

	/**
	 * Turn off the button when let go if button is set to press.
	 * Ths function is necessary otherwise if one object leaves but one stays, it would turn off the button.
	 * coll: collider of other object
	 */
	void OnCollisionExit2D (Collision2D coll){
		if ((coll.gameObject.tag == "Player" || coll.gameObject.tag == "Object") && button_type == Button_Types.PRESS) {
			isOn = false;
		}
			
	}
	
	/**
	 * Change sprite of button based on current power value.
	 */
	void Update () {
		if (isOn) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = buttondown;
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = buttonup;
		}
	}
}
