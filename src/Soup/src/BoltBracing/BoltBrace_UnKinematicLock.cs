using FistVR;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace PotatoesSoup
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
			if (physobj is Handgun) bolt = (physobj as Handgun).Slide;
		}

		public void Update()
		{
			if (!physobj.IsHeld && !physobj.IsAltHeld && !bolt.IsHeld && physobj.TimeSinceInQuickbelt != 0)
			{
				physobj.SetIsKinematicLocked(false);
				Destroy(this);
			}
		}
	}
}