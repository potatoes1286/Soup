using System;
using FistVR;
using UnityEngine;

namespace Plugin
{
	public class BoltBrace_UnKinematicLock : MonoBehaviour
	{
		private FVRPhysicalObject physobj;
		private FVRInteractiveObject bolt;

		public void Start()
		{
			physobj = GetComponent<FVRPhysicalObject>();

			if (physobj is ClosedBoltWeapon) bolt = (physobj as ClosedBoltWeapon).Bolt;
			if (physobj is BoltActionRifle) bolt = (physobj as BoltActionRifle).BoltHandle;
		}

		public void Update()
		{
			if (!physobj.IsHeld && !physobj.IsAltHeld && !bolt.IsHeld)
			{
				physobj.SetIsKinematicLocked(false);
				Destroy(this);
			}
		}
	}
}