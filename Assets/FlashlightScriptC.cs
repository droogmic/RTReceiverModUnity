using UnityEngine;
using System.Collections;

public class FlashlightScriptC : MonoBehaviour {

	public AnimationCurve batteryCurve;

	private bool flashlightOn = false;
	private float initial_pointlight_intensity;
	private float initial_spotlight_intensity;
	private static float maxBatteryLife = 60.0f*5.0f;
	private float batteryLifeRemaining = maxBatteryLife;

	private Light pointLight;
	private Light spotLight;

	void Awake ()
	{
		flashlightOn = false;
	}

	// Use this for initialization
	void Start () 
	{
		pointLight = transform.Find("Head").Find ("PointLight").GetComponent <Light>();
		spotLight = transform.Find("Head").Find ("SpotLight").GetComponent <Light>();

		initial_pointlight_intensity = pointLight.intensity;
		initial_spotlight_intensity = spotLight.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		if(flashlightOn){
			batteryLifeRemaining -= Time.deltaTime;
			if(batteryLifeRemaining <= 0.0f){
				batteryLifeRemaining = 0.0f;
			}
			float battery_curve_eval = batteryCurve.Evaluate(1.0f - batteryLifeRemaining / maxBatteryLife);
			pointLight.intensity = initial_pointlight_intensity * battery_curve_eval * 8.0f;
			spotLight.intensity = initial_spotlight_intensity * battery_curve_eval * 3.0f;
			pointLight.enabled = true;
			spotLight.enabled = true;
		} else {
			pointLight.enabled = false;
			spotLight.enabled = false;
		}
//		if(rigidbody){
//			pointLight.enabled = true;
//			pointLight.intensity = 1.0f + Mathf.Sin(Time.time * 2.0f);
//			pointLight.range = 1.0f;
//		} else {
//			pointLight.light.range = 10.0f;
//		}
	}

	public void TurnOn ()
	{
		if (!flashlightOn) 
		{
			flashlightOn = true;
		}
	}
	public void TurnOff ()
	{
		if (flashlightOn) 
		{
			flashlightOn = false;
		}
	}
	public void Toggle ()
	{
		if (flashlightOn) 
			TurnOff ();
		else
			TurnOn();
	}

	public float FlashlightRemainingBattery()
	{
		return batteryLifeRemaining;
	}

	public bool FlashlightState()
	{
		return flashlightOn;
	}

	public void SetFlashlightRemainingBattery(float newBatteryRemaining)
	{
		batteryLifeRemaining = newBatteryRemaining;
	}

	public void SetFlashlightState(bool newFlashlightState)
	{
		flashlightOn = newFlashlightState;
	}
}

