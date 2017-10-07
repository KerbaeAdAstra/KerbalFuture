using System;
using UnityEngine;
using KerbalFuture;

namespace SpaceFolder
{
	class FlightDrive : VesselModule
	{
		static Vector3d cbPos;
		static double vesHeight;
		static CelestialBody vesBody;
		static CelestialBody warpBody;
		static double warpLong, warpLat;
		static double bodyGravPot;
		var vesselData;
		
		public void WarpVessel()
		{
			if (WarpIsGo())
			{
				vesBody = Vessel.mainBody;
				vesHeight = (Vessel.alitiude + vesBody.Radius);
				warpBody = GUI.ChosenBody();
				warpLong = GUI.ChosenLat();
				warpLat = GUI.ChosenLong();
				bodyGravPot = CalculateGravPot(vesBody);
				cbPos = warpBody.position;
				Vector3dHelper CBVector = new Vector3dHelper();
				Vector3dHelper VesPosition = new Vector3dHelper();
				CBVector.ConvertVector3dToXYZCoords(cbPos);
				VesPosition.SetX(CBVector.Vector3dX() + XFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetY(CBVector.Vector3dY() + YFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetZ(CBVector.Vector3dZ() + ZFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				Vessel.SetPosition(VesPosition.ConvertXYZCoordsToVector3d(VesPosition.Vector3dX, VesPosition.Vector3dY, VesPosition.Vector3dZ), true);
				
			}
		}
		static double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
			if (includePlanetRadius)
			{
				return v.altitude + v.mainBody.Radius();
			}
			return v.altitude;
		}
		static double GetVesselLongPos(Vector3d pos)
		{
			return Vessel.GetLongitude(pos, false);
		}
		static double GetVesselLatPos(Vector3d pos)
		{
			return Vessel.GetLatitude(pos, false);
		}
		static double CalculateGravPot(CelestialBody cb)
		{
			return cb.gravParameter / Math.Pow(GetVesselAltitude
											   (true, Vessel), 2);
		}
	}
}
