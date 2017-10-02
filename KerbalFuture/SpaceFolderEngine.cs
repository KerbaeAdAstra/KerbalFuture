using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class SpaceFolderEngine : PartModule
	{
		public static double HoleDiameter()
		{
			this.PartModule.Fields.GetValue(SpaceFolderEngine);
		}
		public void OnFixedUpdate()
		{
			Debug.Log("[BenK]" + this.PartModule.Fields.GetValue(SpaceFolderEngine).ToString);
		}
	}
}