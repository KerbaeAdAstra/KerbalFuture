using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;

namespace KerbalFuture
{
	class FlightDrive : VesselModule
	{
		private Vector3d vesPos;
		private double vesHeight;
		private CelestialBody vesBody; 
		public void FixedUpdate()
		{
			if((HighLogic.LoadedSceneIsFlight) && (SpaceFolderWarpCheck()))
			{
				vesPos = this.Vessel.GetWorldPosition3D();
				vesBody = this.Vessel.mainBody;
				vesHeight = this.Vessel.alitiude + vesBody.Radius;
			}
		}
	}
}
