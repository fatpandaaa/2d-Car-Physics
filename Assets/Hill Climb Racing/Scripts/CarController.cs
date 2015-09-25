using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	//public float targetSpeed = 1000f;
	public Transform centerOfMass;
	public float acceleration = 600f;
	public float amount = 200f;

	private GameObject[] wheels = new GameObject[2];
	//private WheelJoint2D[] motorWheel = new WheelJoint2D[2];
	//private JointMotor2D motor;
	//private Vector2 preTransform;
	private bool isGameStarted = false;
	private float v = 0f;
	private bool backWheelUp = false;
	private bool frontWheelUp = false;
	private float radius = 0f;
	//public float v = 0f;

	void Awake(){
		wheels[0] = GameObject.Find("WheelBack");
		wheels[1] = GameObject.Find("WheelForward");
		radius = wheels [0].GetComponent<CircleCollider2D> ().radius;
		//rigidbody2D.centerOfMass = centerOfMass;
		//rigidbody2D.centerOfMass = new Vector2 (centerOfMass.transform.position.x, centerOfMass.transform.position.y);
		//motorWheel = gameObject.GetComponentsInChildren<WheelJoint2D> ();
		//motor.motorSpeed = 0f;
		//motor.maxMotorTorque = 10000f;
		//motor.maxMotorTorque = 10000f;
		//motorWheel[0].motor = motor;
		//motorWheel[1].motor = motor;
		//preTransform = new Vector2(transform.position.x, transform.position.y);
		//transform.position = new Vector2 (preTransform.x, preTransform.y + 1);
		isGameStarted = false;
	}
	// Update is called once per frame
	void Update () {

		RaycastHit2D hit1 = Physics2D.Raycast (new Vector2(wheels[0].transform.position.x - 0.001f, wheels[0].transform.position.y - radius) , -Vector2.up);
		//RaycastHit2D hit1 = Physics2D.Raycast (wheels[0].transform.localPosition , -Vector2.up);
		if(hit1 == GameObject.Find("hill1") && hit1 == GameObject.Find("TruckChassisSprite")){
			//Debug.Log(hit1.point.magnitude - wheels[0].transform.position.magnitude);
			//Debug.Log (Vector2.Distance(hit1.point, wheels[0].transform.position));
			float dis = Vector2.Distance(hit1.point, new Vector2(wheels[0].transform.position.x - 0.001f, wheels[0].transform.position.y - radius));
			//if(dis > 0.96f && dis < 1.00f)
			if(dis <= radius + 0.05f)
				Debug.Log(" On Ground");
			else
				Debug.Log(" On Air");
		}
		Vector3 start = new Vector3 (wheels[0].transform.position.x - 0.001f, wheels[0].transform.position.y, 0);
		Vector3 end = new Vector3 (hit1.point.x, hit1.point.y, 0);
		//Debug.Log(Vector2.Distance(hit1.point, wheels[0].transform.position) - 0.98f);
		Debug.DrawLine (start,hit1.point,Color.red);
		//Debug.Log(Vector2.Distance(hit1.point, transform.position) - wheels[0].GetComponent<CircleCollider2D>().radius);
		//Debug.Log (hit1.point);

		//Debug.Log (wheels[0].rigidbody2D.inertia);
		/*
		if (transform.position.x == preTransform.x && transform.position.y == preTransform.y)
						Application.LoadLevel (0);
		else
			preTransform = new Vector2(transform.position.x, transform.position.y);
		*/

		/*
		float v = Input.GetAxis ("Horizontal");
		if(v != 0){

			isGameStarted = true;

			motor.motorSpeed += (v * acceleration);
			
			motorWheel[0].motor = motor;
			motorWheel[1].motor = motor;
			//motorWheel[0].jointSpeed += (acceleration * v);
			//motorWheel[1].jointSpeed += (acceleration * v);

			
			//Debug.Log (motorWheel[0].jointSpeed+"   "+motorWheel[0].jointTranslation);
			//Debug.Log (motorWheel[0].motor.motorSpeed);
		}
		else if(motor.motorSpeed > 0f){
			isGameStarted = true;
			motor.motorSpeed -= (2 * acceleration);
			motorWheel[0].motor = motor;
			motorWheel[1].motor = motor;
		}
		*/
		//Debug.Log (motorWheel[0].motor.motorSpeed);
		//Debug.Log (Input.GetAxis ("Horizontal") + "   " + Input.GetAxis("Vertical"));
	}

	void FixedUpdate(){
		v = CrossPlatformInput.GetAxis ("Horizontal") * amount * (-1f);

		//rigidbody2D.AddForce(v * (-transform.right) * amount, ForceMode2D.Force);
		//if(v > 0)
			wheels[0].rigidbody2D.AddTorque (v);
		//wheels [0].rigidbody2D.AddForce (transform.right * (-1) * v, ForceMode2D.Force);
		//else if(v < 0)
			wheels[1].rigidbody2D.AddTorque (v);
		//wheels [1].rigidbody2D.AddForce (transform.right * (-1) * v, ForceMode2D.Force);

		//rigidbody2D.AddForce (transform.right * (-1) * v, ForceMode2D.Force);
		//rigidbody2D.AddTorque((wheels [0].rigidbody2D.angularVelocity + wheels [1].rigidbody2D.angularVelocity) / rigidbody2D.mass);
		//Debug.Log ((wheels[0].rigidbody2D.angularVelocity + wheels[1].rigidbody2D.angularVelocity) / rigidbody2D.mass);
		//Debug.Log (wheels[0].rigidbody2D.angularVelocity);
	}

	/*
	void FixedUpdate(){
		float v = Input.GetAxis ("Horizontal");




		if(motor.motorSpeed > 0f && v == 0){
			isGameStarted = true;
			//motor.motorSpeed -= (2 * acceleration);
			//motorWheel[0].motor = motor;
			//motorWheel[1].motor = motor;
		}

		else if(v > 0f){
			
			isGameStarted = true;
			
			motor.motorSpeed += (v * acceleration);
			
			motorWheel[0].motor = motor;
			motorWheel[1].motor = motor;
			//motorWheel[0].jointSpeed += (acceleration * v);
			//motorWheel[1].jointSpeed += (acceleration * v);
			
			
			//Debug.Log (motorWheel[0].jointSpeed+"   "+motorWheel[0].jointTranslation);
			//Debug.Log (motorWheel[0].motor.motorSpeed);
		}
		else if(v < 0f){
			
			isGameStarted = true;
			
			//motor.motorSpeed -= (2 * acceleration);
			
			//motorWheel[0].motor = motor;
			//motorWheel[1].motor = motor;
			//motorWheel[0].jointSpeed += (acceleration * v);
			//motorWheel[1].jointSpeed += (acceleration * v);
			
			
			//Debug.Log (motorWheel[0].jointSpeed+"   "+motorWheel[0].jointTranslation);
			//Debug.Log (motorWheel[0].motor.motorSpeed);
		}

	}
	*/

	void OnGUI(){
		GUI.skin.button.fontSize = Screen.width / 50;
		if (GUI.Button (new Rect (0, 0, Screen.width / 10, Screen.height / 8), "Replay"))
			Application.LoadLevel (Application.loadedLevel);

		if (GUI.Button (new Rect (Screen.width - Screen.width / 10, 0, Screen.width / 10, Screen.height / 8), "Quit"))
			Application.Quit();
		//if()
		//v = GUI.RepeatButton (new Rect (Screen.width / 12f, Screen.height / 1.3f, Screen.width / 10, Screen.height / 8), "Replay");
	}
	/*
	void FixedUpdate(){
		if (transform.position.x == preTransform.x && transform.position.y == preTransform.y && isGameStarted)
			Application.LoadLevel (0);
		else
			preTransform = new Vector2(transform.position.x, transform.position.y);
	}
	*/
}
