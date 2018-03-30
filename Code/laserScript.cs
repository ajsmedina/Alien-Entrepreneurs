using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class laserScript : MonoBehaviour {
	public LineRenderer laserLineRenderer;
	public GameObject partner;
	public float laserWidth = 0.1f;

	/**
	 * Initialize laser values
	 */
	void Start() {
		Vector3[] initLaserPositions = new Vector3[ 2 ] { Vector3.zero, Vector3.zero };
		laserLineRenderer.SetPositions( initLaserPositions );
		laserLineRenderer.SetWidth( laserWidth, laserWidth );
	}

	/**
	 * Update laser properties
	 */
	void Update() 
	{
		ShootLaser ();
		
	}

	/**
	 * Shoot laser from current position to target
	 */
	void ShootLaser()
	{
		Vector3 endPosition = partner.transform.position;
		Vector3 initialpos = transform.position;

		//Set Raycast position. Only hit insulators and players
		RaycastHit2D hit =  Physics2D.Raycast(transform.position, (endPosition - initialpos).normalized, (endPosition - initialpos).magnitude, LayerMask.GetMask("Insulator", "Player"));

		if (hit.collider != null) {

			//Lose level if raycast hits another player that is not the target.
			//If it hits an insulator, set that as the endpoint of the laser.
			if (hit.collider.gameObject.tag == "Player") {
				if(hit.collider.gameObject.Equals(partner) == false)
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}else {
				endPosition = hit.point;
			}
		}
	

		laserLineRenderer.SetPosition( 0, initialpos );
		laserLineRenderer.SetPosition( 1, endPosition );
	}

}
