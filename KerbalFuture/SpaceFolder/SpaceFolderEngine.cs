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

		[KSPField(isPersistant = true, guiActive = false)]
		public int engineMultiplier;
		
		//Returns the ClassID of this PartModule.
		public static int ModuleClassID()
		{
			SpaceFolderEngine sfe = new SpaceFolderEngine();
			return sfe.ClassID;
		}
		//Method to access warp drive diameter.
		public float WarpDriveDiameter()
		{
			return warpDriveDiameter;
		}
		//Method to accesss Tier
		public int EngineMultiplier()
		{
			return engineMultiplier;
		}
	}
}
