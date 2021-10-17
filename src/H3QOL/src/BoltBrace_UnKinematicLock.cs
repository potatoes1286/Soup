using System;
using FistVR;
using UnityEngine;

namespace Plugin
{
	public class BoltBrace_UnKinematicLock : MonoBehaviour
	{
		private ClosedBoltWeapon cb;

		public void Start()
		{
			cb = GetComponent<ClosedBoltWeapon>();
		}

		public void Update()
		{
			if (!cb.IsHeld && !cb.IsAltHeld && !cb.Bolt.IsHeld)
			{
				cb.SetIsKinematicLocked(false);
				Destroy(this);
			}
		}
	}
}