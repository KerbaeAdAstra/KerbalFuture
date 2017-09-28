using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;

namespace KerbalFuture
{
	class SpaceFolderVslChecks : MonoBehavior
	{
		public bool SpaceFolderWarpCheck()
		{
			List<Part> vslParts = this.Vessel.Parts;
			for(vslParts[i])
			{
				if(partContainsModule(vslParts[i], "SpaceFolderDrive"))
				{
					
				}
			}
		}
	}
}