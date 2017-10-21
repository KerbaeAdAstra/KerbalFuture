using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;

namespace SpaceFolder
{
	class SpaceFolderEngine : PartModule
	{
		//Do not access directly, method below for that. 
		[KSPField(isPersistant = true, guiActive = false)]
		public float warpDriveDiameter;
		
		//Returns the ClassID of this PartModule.
		public int ModuleClassID()
		{
			return this.ClassID;
		}
		//Method to access warp drive diameter.
		public float WarpDriveDiameter()
		{
			return warpDriveDiameter;
		}
	}
}
