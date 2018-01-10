using UnityEngine;
using System.Collections;

public class Field_Hazard : MonoBehaviour {
	public bool isHazard;

	public bool defaulthazard;
	public bool alwaysOn;
	public bool flip;

	// Use this for initialization
	void Start () {
		defaulthazard = this.gameObject.GetComponent<Field_Power> ().defOn;
		isHazard = defaulthazard;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (alwaysOn) {
			isHazard = true;
		} else if (flip) {
			isHazard = !this.gameObject.GetComponent<Field_Power> ().isOn;
		} else {
			isHazard = this.gameObject.GetComponent<Field_Power> ().isOn;
		}

		if (isHazard) {
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		} else {
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}
	
	}
}
