using UnityEngine;
using System.Collections;

public class Field_Sprite : MonoBehaviour {
	public Sprite spr_water;
	public Sprite spr_metal;
	public Sprite spr_ice;
	public Sprite spr_sand;
	public Sprite spr_metwater;

	GameObject obj;
	SpriteRenderer spr;

	// Use this for initialization
	void Start () {
		obj = this.gameObject;
		spr = this.gameObject.GetComponent<SpriteRenderer>();
	}

	
	// Update is called once per frame
	void Update () {

		if (obj.GetComponent<Field_Hazard>() != null) {
			if (obj.GetComponent<Field_Hazard> ().isHazard) {
				if (obj.layer == 8 || (obj.GetComponent<Field_Insulator>() != null && obj.GetComponent<Field_Insulator>().insulate)) {
					spr.sprite = spr_metwater;
					
				} else {
					spr.sprite = spr_water;
				}
			} else {
				spr.sprite = spr_ice;
			}
		}	else if (obj.layer == 8|| (obj.GetComponent<Field_Insulator>() != null && obj.GetComponent<Field_Insulator>().insulate)) {
			spr.sprite = spr_metal;
		}  else {
			spr.sprite = spr_sand;
		}

		if (obj.GetComponent<Moving_Platform> () != null) {

			if (obj.GetComponent<Field_Door> () != null) {

				if (obj.GetComponent<Field_Door> ().open) {

					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 0.5f, 0.25f);
				} else {
					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.5f, 1.0f, 0.5f, 0.9f);
				}

			} else {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.5f, 1.0f, 0.5f, 1.0f);
			}
		} else if (obj.GetComponent<Field_Door> () != null) {

			if (obj.GetComponent<Field_Door> ().open) {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.6f, 0.7f, 0.25f);
			} else {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.6f, 0.7f, 0.9f);
			}
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}

	
	}
}
