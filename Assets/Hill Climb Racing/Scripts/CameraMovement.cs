using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject car;
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (car.transform.position.x, car.transform.position.y, -10f);;
	}
}
