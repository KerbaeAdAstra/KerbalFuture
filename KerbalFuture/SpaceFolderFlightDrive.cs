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
		private static double gravPot;

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
		private double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
		    if(includePlanetRadius)
		    {
		        return v.altitude + v.mainBody.Radius();
		    }
		    return v.altitude;
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
			gravPot = cb.gravParameter / GetVesselAltitude(true, this.Vessel)^2;
			

		//	gravPot = 
		}
	}
}
