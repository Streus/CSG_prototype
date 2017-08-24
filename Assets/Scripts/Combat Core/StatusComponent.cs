using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

[Serializable]
public class StatusComponent : ISerializable
{
	/* Instance Vars */
	public int stacks;

	/* Constructors */
	public StatusComponent()
	{
		stacks = 1;
	}
	public StatusComponent(int stacks)
	{
		this.stacks = stacks;
	}
	public StatusComponent(StatusComponent other) : this(other.stacks) { }
	public StatusComponent(SerializationInfo info, StreamingContext context)
	{
		stacks = info.GetInt32 ("stacks");
	}

	/* Instance Methods */

	// Called when this Status is first added to an Entity
	public virtual void OnApply(Entity subject){ }

	// Called when this Status is removed from its subject
	public virtual void OnRevert(Entity subject){ }

	// Called every update cycle by the subject
	public virtual void OnUpdate(Entity subject, float time){ }

	// Called when the subject dies
	public virtual void OnDeath(Entity subject){ }

	// Called when the subject enters a stunned state
	public virtual void OnStunned(Entity subject){ }

	// Called when the subject enters a rooted state
	public virtual void OnSleeping(Entity subject){ }

	// For serialization
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue ("stacks", stacks);
	}
}
