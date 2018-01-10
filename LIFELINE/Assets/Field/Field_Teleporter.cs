using UnityEngine;
using System.Collections;

public class Field_Teleporter : MonoBehaviour {
	public GameObject target;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Rigidbody2D> ().angularVelocity = 20.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
