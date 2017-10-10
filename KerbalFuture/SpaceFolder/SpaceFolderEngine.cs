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
		[KSPField(isPersistant = true, guiName = "Warp Drive Size", guiActive = true, guiActiveEditor = true)]
		private int warpDriveDiameter;
		
		public override void OnLoad(ConfigNode node)
		{
			base.OnLoad(node);
		}
	}
}