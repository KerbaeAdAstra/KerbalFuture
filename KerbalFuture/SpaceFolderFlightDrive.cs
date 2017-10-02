using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class FlightDrive : VesselModule
	{
		private static Vector3d vesPos;
		private static double vesHeight;
		private static CelestialBody vesBody;
		
		public static void WarpVessel()
		{
			if(WarpIsGo())
			{
				vesPos = this.Vessel.GetWorldPosition3D();
				vesBody = this.Vessel.mainBody;
				vesHeight = this.Vessel.alitiude + vesBody.Radius;
			}
		}
	}
}
