using UnityEngine;
using System.Collections;

public class Field_Power : MonoBehaviour {
	public bool and = false;
	public GameObject[] buttons;
	public bool isOn = false;
	public bool defOn = false;
	public Button_Types type;

	public bool down=false;

	/**
	 * Initialize on value
	 */
	void Start () {
		isOn = defOn;
	}
	
	/**
	 * Update power based on buttons pressed.
	 */
	void Update () {
		bool pressed;

		//Change on/off based on power type
		//and: start true, short circuit to false if one button is off
		//or: start false, short circuit to true if one button is on
		if (and) {
			pressed = true;
			for (int i = 0; i < buttons.Length; i++) {
				if (!buttons[i].GetComponent<Field_Button>().isOn) {
					pressed = false;
					break;
				}

			}
		} else {
			pressed = false;
			for (int i = 0; i < buttons.Length; i++) {
				if (buttons[i].GetComponent<Field_Button>().isOn) {
					pressed = true;
					break;
				}

			}
		}


		//Change power based on power type.
		if (type == Button_Types.PRESS) {
			isOn = pressed ? !defOn : defOn;
		} else if (type == Button_Types.TOGGLE && pressed && !down) {
			isOn = !isOn;
			down = true;
		} else if (type == Button_Types.ONE_SHOT && pressed) {
			isOn = !defOn;
		} else if (!pressed) {
			down = false;
		}



	}
}
