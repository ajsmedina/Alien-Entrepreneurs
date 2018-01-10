using UnityEngine;
using System.Collections;

public class Field_Collectable : MonoBehaviour {
	private Data_Field data;
	// Use this for initialization
	void Start () {
		data = GameObject.Find ("FieldManager").GetComponent<Data_Field> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Collected(){
			data.addPoint ();
			GameObject.Destroy (this.gameObject);
	}
}
