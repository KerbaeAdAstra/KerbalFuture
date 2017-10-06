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
		private static Vector3d cbPos;
		private static double vesHeight;
		private static CelestialBody vesBody;
		private static CelestialBody warpBody;
		private static double warpLong, warpLat;
		private static double bodyGravPot;

		public void WarpVessel()
		{
			if(WarpIsGo())
			{
				vesBody = this.Vessel.mainBody;
				vesHeight = (this.Vessel.alitiude + vesBody.Radius);
				warpBody = GUI.ChosenBody();
				warpLong = GUI.ChosenLat();
				warpLat = GUI.ChosenLong();
				bodyGravPot = CalculateGravPot(vesBody);
				cbPos = warpBody.position;
				Vector3dHelper CBVector = new Vector3dHelper;
				Vector3dHelper VesPosition = new Vector3dHelper;
				CBVector.ConvertVector3dToXYZCoords(cbPos);
				VesPosition.SetX(CBVector.Vector3dX() + XFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetY(CBVector.Vector3dY() + YFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetZ(CBVector.Vector3dZ() + ZFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				this.Vessel.SetPosition(VesPosition.ConvertXYZCoordsToVector3d(VesPosition.Vector3dX, VesPosition.Vector3dY, VesPosition.Vector3dZ), true);
				
			}
		}
		private static double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
		    if(includePlanetRadius)
		    {
		        return v.altitude + v.mainBody.Radius();
		    }
		    return v.altitude;
		}
		private static double GetVesselLongPos(Vector3d pos)
		{
			return this.Vessel.GetLongitude(pos, false);
		}
		private static double GetVesselLatPos(Vector3d pos)
		{
			return this.Vessel.GetLatitude(pos, false);
		}
		private static double CalculateGravPot(CelestialBody cb)
		{
			return cb.gravParameter / Math.Pow(GetVesselAltitude(true, this.Vessel), 2);
		}
	}
}
