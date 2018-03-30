using UnityEngine;
using System.Collections;

public class Field_Collectable : MonoBehaviour {
	private Data_Field data;

	/**
	 * Find the fieldmanager that gets called.
	 */
	void Start () {
		data = GameObject.Find ("FieldManager").GetComponent<Data_Field> ();
	
	}

	/**
	 * Called when collected by a player. Used to end the level.
	 */
	public void Collected(){
			data.addPoint ();
			GameObject.Destroy (this.gameObject);
	}
}
