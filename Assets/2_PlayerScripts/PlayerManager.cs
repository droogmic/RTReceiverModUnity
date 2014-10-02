using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	//private float lastSynchronizationTime = 0f;
	//private float syncDelay = 0f;
	//private float syncTime = 0f;
	//private Vector3 syncStartPosition = Vector3.zero;
	//private Vector3 syncEndPosition = Vector3.zero;

	[HideInInspector]
	public Vector3 colour;

	void Awake()
	{
		name = "playerObj";
	}
	
	// Use this for initialization
	void Start () {
		//syncStartPosition = transform.position;
		//syncEndPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (networkView.isMine)
		{
			InputColorChange();
		}
		else
		{
			//SyncedMovement();
		}
	}
	
	
	private void InputColorChange()
	{
		if (Input.GetKeyDown(KeyCode.R))
			ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}
	
	[RPC] void ChangeColorTo(Vector3 incolor)
	{
		//renderer.material.color = new Color(color.x, color.y, color.z, 1f);
		colour = incolor;
		
		if (networkView.isMine)
			networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, incolor);
	}
	/*	
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = transform.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = transform.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncStartPosition = transform.position;
			//syncStartPosition = syncPosition;
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
		}
	}
	*/
}
