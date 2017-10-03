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
		private static CelestialBody warpBody;

		public void WarpVessel()
		{
			if(WarpIsGo())
			{
				vesPos = this.Vessel.GetWorldPosition3D();
				vesBody = this.Vessel.mainBody;
				vesHeight = (this.Vessel.alitiude + vesBody.Radius);
				warpBody = GUIChosenBody();
				
				
			}
		}
		private double GetVesselLongPos(Vector3d pos)
		{
			return this.Vessel.GetLongitude(pos, false);
		}
		private double GetVesselLatPos(Vector3d pos)
		{
			return this.Vessel.GetLatitude(pos, false);
		}
		private void CalculateGravPot(CelestialBody cb)
		{
			cb.
		}
	}
}
