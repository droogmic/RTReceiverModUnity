//#pragma strict

using UnityEngine;
using System.Collections;

//@RequireComponent(NetworkView)
public class CubePlayerManager : MonoBehaviour {

	public float speed = 5f;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;


	void Awake()
	{
		name = "playerObj";
	}

	// Use this for initialization
	void Start () {
		syncStartPosition = rigidbody.position;
		syncEndPosition = rigidbody.position;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (networkView.isMine)
		{
			InputMovement();
			InputColorChange();
		}
		else
		{
			SyncedMovement();
		}
	}


	private void InputColorChange()
	{
		if (Input.GetKeyDown(KeyCode.R))
			ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}
	
	[RPC] void ChangeColorTo(Vector3 color)
	{
		renderer.material.color = new Color(color.x, color.y, color.z, 1f);
		
		if (networkView.isMine)
			networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
	}

	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	void InputMovement()
	{
		if (Input.GetKey (KeyCode.W))
			//rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);
			rigidbody.AddForce (Vector3.forward * speed);
		
		if (Input.GetKey(KeyCode.S))
			//rigidbody.MovePosition(rigidbody.position - Vector3.forward * speed * Time.deltaTime);
			rigidbody.AddForce (-Vector3.forward * speed);

		if (Input.GetKey(KeyCode.D))
			//rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);
			rigidbody.AddForce (Vector3.right * speed);
		
		if (Input.GetKey(KeyCode.A))
			//rigidbody.MovePosition(rigidbody.position - Vector3.right * speed * Time.deltaTime);
			rigidbody.AddForce (-Vector3.right * speed);
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncStartPosition = rigidbody.position;
			//syncStartPosition = syncPosition;
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
		}
	}

	//void OnCollisionEnter(Collision info)
	//{
	//	DebugConsole.Log ("Collision");
	//}

}
