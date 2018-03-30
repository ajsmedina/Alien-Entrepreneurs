using UnityEngine;
using System.Collections;

public class Field_Insulator : MonoBehaviour {
	public bool alwaysOn;
	public bool flip;
	public bool insulate;

	Field_Power pow;

	/**
	 * Initialize pow 
	 */
	void Start () {
		pow = this.gameObject.GetComponent<Field_Power> ();
	}

	/**
	 * Modify whether or not this object is an insulator
	 */
	void Update () {
		if (alwaysOn) {
			insulate = true;
		} else if (flip) {
			insulate = !pow.isOn;
		} else {
			insulate = pow.isOn;
		}

		this.gameObject.layer = insulate ? 8 : 0;
	
	}
}
