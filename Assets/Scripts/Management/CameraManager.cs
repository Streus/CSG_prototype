using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	/* Static Vars */

	/* Instance Vars */

	// The Transform of the GameObject to which the camera is attached
	private Transform cam;

	// Target stuff
	public Transform target;
	public bool isFollowingTarget;
	public bool freeFollow;

	// Shake stuff
	private float shakeDur = 0f;
	private float shakeIntensity;
	private float shakeDecay;

	/* Instance Methods */
	public void Awake()
	{
		cam = transform.GetChild (0);
	}

	public void Update()
	{
		//shake the camera
		if (shakeDur > 0f)
		{
			cam.localPosition = (Vector3)Random.insideUnitCircle * shakeIntensity;

			shakeIntensity -= shakeDecay * Time.deltaTime;
			if (shakeIntensity <= 0f)
				shakeDur = 0f;

			shakeDur -= Time.deltaTime;
			if (shakeDur <= 0f)
			{
				shakeDur = 0f;
				shakeIntensity = 0f;
				shakeDecay = 0f;

				cam.localPosition = Vector2.zero;
			}
		}

		//follow a target or transition through a path
		if (isFollowingTarget && target != null && freeFollow)
		{
			Vector3 tarPos = new Vector3 (target.position.x, target.position.y, transform.position.z);
			transform.position = Vector3.Lerp (transform.position, tarPos, Time.deltaTime);
		}
	}

	public void LateUpdate()
	{
		//stay locked to the target's position
		if (isFollowingTarget && target != null && !freeFollow)
			transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
	}

	// Assigns the target and toggles target following
	public void setTarget(Transform target)
	{
		this.target = target;
		isFollowingTarget = true;
	}

	// Unassigns the target and frees the camera
	public void freeCamera()
	{
		target = null;
		isFollowingTarget = false;
	}

	// Shakes the camera for some a duration
	public void shakeCamera(float duration, float intensity, float decayRate = 0f)
	{
		shakeDur = duration;
		shakeIntensity = intensity;
		shakeDecay = decayRate;
	}
}
