using UnityEngine;
using System.Collections;

public class Field_Insulator : MonoBehaviour {
	public bool alwaysOn;
	public bool flip;
	public bool insulate;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (alwaysOn) {
			insulate = true;
		} else if (flip) {
			insulate = !this.gameObject.GetComponent<Field_Power> ().isOn;
		} else {
			insulate = this.gameObject.GetComponent<Field_Power> ().isOn;
		}

		this.gameObject.layer = insulate ? 8 : 0;
	
	}
}
