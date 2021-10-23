using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Plugin
{
	public class DecockingRevolver : MonoBehaviour
	{
		[HarmonyPatch(typeof(Revolver), "Fire")]
		[HarmonyPrefix]
		public static bool DecockRevolver(Revolver __instance)
		{
			if (__instance.m_hand != null)
			{
				if (__instance.m_hand.Input.TouchpadPressed)
				{
					//un-fire the chamber round lol
					__instance.Chambers[__instance.CurChamber].IsSpent = false;
					__instance.Chambers[__instance.CurChamber].UpdateProxyDisplay();
					return false;
				}
			}
			return true;
		}
		
		[HarmonyPatch(typeof(Revolver), "UpdateRecocking")]
		[HarmonyPrefix]
		public static bool SlowDecocking_Prefix_Patch(Revolver __instance)
		{
			if (__instance.IsHeld)
			{
				if (__instance.m_hand.Input.TouchpadPressed)
				{
					__instance.RecockingSpeeds.x *= 0.1f;
				}
			}
			return true;
		}
				
		[HarmonyPatch(typeof(Revolver), "UpdateRecocking")]
		[HarmonyPostfix]
		public static void SlowDecocking_Postfix_Patch(Revolver __instance)
		{
			if (__instance.IsHeld)
			{
				if (__instance.m_hand.Input.TouchpadPressed)
				{
					__instance.RecockingSpeeds.x *= 10f;
				}
			}
		}
		
		
		//Look Ma! My first transpile!
		[HarmonyPatch(typeof(Revolver), "UpdateTriggerHammer")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> DecockRevolver_UpdateTriggerHammerPatch(IEnumerable<CodeInstruction> instructions)
		{
			var startIndex = -1;
			
			var codes = new List<CodeInstruction>(instructions);
			for (var i = 0; i < codes.Count; i++) //is there a nicer way? yes. do i care? no.
			{
				#region disgusting check
				if (i + 4 >= (codes.Count - 1)) break;
				if (codes[i].opcode == OpCodes.Ldarg_0)
				{
					if (codes[i + 1].opcode == OpCodes.Ldfld)
					{
						if (codes[i + 2].opcode == OpCodes.Ldflda)
						{
							if (codes[i + 3].opcode == OpCodes.Ldfld)
							{
								if (codes[i + 4].opcode == OpCodes.Brtrue)
								{
									startIndex = i;
								}
							}
						}
					}
				}
				#endregion
			}
			
			if (startIndex > -1)
			{
				// we cannot remove the first code of our range since some jump actually jumps to
				// it, so we replace it with a no-op instead of fixing that jump (easier).
				for (int i = 0; i < 5; i++)
				{
					//Console.WriteLine("Replacing " + codes[startIndex + i].opcode.ToString() + " " + codes[startIndex + i].operand.ToString());
					codes[startIndex + i].opcode = OpCodes.Nop;
				}
				//codes.RemoveRange(startIndex, 5);
			}

			return codes.AsEnumerable();
		}
	}
}