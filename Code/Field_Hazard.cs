using UnityEngine;
using System.Collections;

public class Field_Hazard : MonoBehaviour {
	public bool isHazard;

	public bool defaulthazard;
	public bool alwaysOn;
	public bool flip;

	Field_Power pow;

	/**
	 * Initialize pow and hazard
	 */
	void Start () {
		pow = this.gameObject.GetComponent<Field_Power> ();
		defaulthazard = pow.defOn;
		isHazard = defaulthazard;
	
	}

	/**
	 * Change hazard settings to power.
	 */
	void Update () {
		if (alwaysOn) {
			isHazard = true;
		} else if (flip) {
			isHazard = !pow.isOn;
		} else {
			isHazard = pow.isOn;
		}

	
	}
}
