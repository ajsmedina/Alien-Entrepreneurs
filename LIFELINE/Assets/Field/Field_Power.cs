using UnityEngine;
using System.Collections;

public class Field_Power : MonoBehaviour {
	public bool and = false;
	public GameObject[] buttons;
	public bool isOn = false;
	public bool defOn = false;
	public Button_Types type;

	public bool down=false;

	// Use this for initialization
	void Start () {
		isOn = defOn;
	}
	
	// Update is called once per frame
	void Update () {
		bool pressed;

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
