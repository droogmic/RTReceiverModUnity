using UnityEngine;
using System.Collections;

public class BodyPart: MonoBehaviour {

	public GameObject playercameraPrefab;

	// Use this for initialization
	void Start () {
	
		if (name == "Head")
		{
			if (GetComponentInParent<NetworkView>().isMine == true)
			{
				GameObject cameraObj = (GameObject)Instantiate(playercameraPrefab, (transform.position + new Vector3(0f, 0.214167f, 0f)), transform.rotation);
				cameraObj.transform.parent = transform;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 incolor = GetComponentInParent<PlayerManager>().colour;
		renderer.material.color = new Color(incolor.x, incolor.y, incolor.z, 1f);

	}
}
