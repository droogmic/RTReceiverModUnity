using UnityEngine;
using System.Collections;

public class PlayerSyncData : MonoBehaviour {

	//private float syncTime = 0f;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	
	private Vector3 objPos;
	private Quaternion objRot;
	private Quaternion objHeadRot;
	public Vector3 objLinVel;
	private float flashlightRemainingBattery;
	public bool flashlightOn;
	//public Vector3 objRotVel;
	
	// Use this for initialization
	void Start () 
	{
		SetUpVariables ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	//The OnSerializeNetworkView function in a script referenced by the NetworkView component of a GameObject will fire itself every fixed period (default at 15Hz).
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		//Vector3 syncAngularVelocity = Vector3.zero;
		

		if (stream.isWriting)
		{
			//set sync variables to current values
			SetUpVariables ();
			//syncs the variables to other clients
			SerializeVariables(stream);

		}
		else
		{
			//pulls current values from other clients
			SerializeVariables(stream);
			//sets local variables to values pulled from other clients.
			SetDownVariables();
		}

	}
	void SetUpVariables()
	{
		objPos = gameObject.transform.position;
		objRot = gameObject.transform.rotation;
		objHeadRot = gameObject.transform.Find ("Head").rotation;
		objLinVel = gameObject.rigidbody.velocity;
		//objRotVel = gameObject.rigidbody.angularVelocity;
		
		flashlightOn = gameObject.GetComponent<FlashlightScriptC> ().FlashlightState ();
		flashlightRemainingBattery =gameObject.GetComponent<FlashlightScriptC> ().FlashlightRemainingBattery ();
	}
	void SetDownVariables()
	{
		gameObject.transform.position = objPos;
		gameObject.transform.rotation = objRot;
		gameObject.transform.Find ("Head").rotation = objHeadRot;
		gameObject.rigidbody.velocity = objLinVel;
		//objRotVel = gameObject.rigidbody.angularVelocity;
		
		gameObject.GetComponent<FlashlightScriptC> ().SetFlashlightState (flashlightOn);
		gameObject.GetComponent<FlashlightScriptC> ().SetFlashlightRemainingBattery (flashlightRemainingBattery);
	}
	void SerializeVariables(BitStream stream)
	{
		stream.Serialize (ref objPos);
		stream.Serialize (ref objLinVel);
		stream.Serialize (ref objRot);
		stream.Serialize (ref objHeadRot);
		stream.Serialize (ref flashlightOn);
		stream.Serialize (ref flashlightRemainingBattery);
	}
}
