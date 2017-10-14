using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace SpaceFolder
{
	class SpaceFolderEngine : PartModule
	{
		//Do not access directly, method below for that. 
		[KSPField(isPersistant = true, guiActive = false)]
		public float warpDriveDiameter;
		
		//Returns the ClassID of this PartModule.
		public static int ModuleClassID
		{
			return this.ClassID;
		}
		//Method to access warp drive diameter.
		public static float WarpDriveDiameter()
		{
			return warpDriveDiameter;
		}
	}
}
