using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public bool interactable;
	public virtual bool activated{ get; protected set; }

	public virtual void OnInteract()
	{
		if (!interactable)
			return;

		activated = !activated;
	}
}