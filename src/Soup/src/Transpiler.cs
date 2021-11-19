using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using OpCode = System.Reflection.Emit.OpCode;

namespace PotatoesSoup
{
	public class Transpiler : MonoBehaviour
	{
		public static int TranspilerMatchCode(List<CodeInstruction> codes, OpCode[] match, bool log = false)
		{
			//this iterates through an ienumerable codeinstruction and matches it to opcode list
			var startIndex = -1;
			
			for (var i = 0; i < codes.Count; i++)
			{
				if (i + (match.Length - 1) >= (codes.Count - 1)) break;
				bool doesnotmatch = false;
				for (int c = 0; c < match.Length; c++)
				{
					if (codes[i + c].opcode != match[c])
					{
						if(log) Debug.Log("attempting check at " + i + "- failed at " + c + "; expected " + match[c].ToString() + ", got " + codes[i + c].opcode.ToString());
						doesnotmatch = true;
						break;
					}
				}
				if (!doesnotmatch) startIndex = i;
			}

			return startIndex;
		}
		
	}
}