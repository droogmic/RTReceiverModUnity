//#pragma strict

using UnityEngine;
using System.Collections;

//@RequireComponent(NetworkView)
public class BallScript : MonoBehaviour {

	private NetworkViewID newnetviewID;

	// Use this for initialization
	void Start () {
		newnetviewID = Network.AllocateViewID();
		DebugConsole.Log ("Taking ID: " + newnetviewID.ToString());
	}
	
	// Update is called once per frame
	void Update () {

		if (rigidbody.position.y < 0) 
		{
			rigidbody.position = new Vector3(0f,20f,0f);
			rigidbody.velocity = Vector3.zero;
		}
	}

	void OnCollisionEnter(Collision info)
	{
		//DebugConsole.Log (info.collider.gameObject.name);

		//Check if collision with person
		if (info.collider.name == "playerObj")
		{
			//DebugConsole.Log ("Collision");
			//DebugConsole.Log(collider.gameObject.networkView.isMine.ToString());
			//DebugConsole.Log(networkView.isMine.ToString());
			//Check if the person is mine
			if (info.collider.gameObject.networkView.isMine) 
			{
				DebugConsole.Log ("Collided with my player");
				//Check if I own the ball
				if (!networkView.isMine)
				{
					TakeOwnership();
				}
			}
		}

	}

	public void TakeOwnership()
	{
		DebugConsole.Log ("Taking ownership from: " + networkView.viewID.ToString());
		networkView.RPC ("ChangeOwner", RPCMode.AllBuffered, newnetviewID);
	}
	
	[RPC] void ChangeOwner(NetworkViewID netviewID)
	{
		networkView.viewID = netviewID;
		DebugConsole.Log ("Changed owner to: " + networkView.viewID.ToString());
	}

}
