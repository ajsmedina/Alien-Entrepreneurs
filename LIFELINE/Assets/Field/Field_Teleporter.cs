using UnityEngine;
using System.Collections;

public class Field_Teleporter : MonoBehaviour {
	public GameObject target;

	/**
	 * Initialize rotation of teleporters.
	 */
	void Start () {
		this.gameObject.GetComponent<Rigidbody2D> ().angularVelocity = 20.0f;
	
	}
}
