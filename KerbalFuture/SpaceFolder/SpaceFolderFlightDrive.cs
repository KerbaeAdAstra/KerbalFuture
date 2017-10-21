using System;
using UnityEngine;

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
		
		public void WarpVessel(Vessel v)
		{
			double cbx, cby, cbz;
			if (SpaceFolderWarpChecks.GoodToGo())
			{
				vesBody = v.mainBody;
				vesHeight = (v.altitude + vesBody.Radius);
				warpBody = GUI.ChosenBody();
				warpLong = GUI.ChosenLat();
				warpLat = GUI.ChosenLong();
				bodyGravPot = CalculateGravPot(vesBody, v);
				cbPos = warpBody.position;
				Vector3dHelper CBVector = new Vector3dHelper();
				Vector3dHelper VesPosition = new Vector3dHelper();
				LatLongHelper LLH = new LatLongHelper();
				CBVector.ConvertVector3dToXYZCoords(cbPos, cbx, cby, cbz);
				VesPosition.SetX(cbx + LLH.XFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetY(cby + LLH.YFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetZ(cbz + LLH.ZFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				v.SetPosition(VesPosition.ConvertXYZCoordsToVector3d(VesPosition.Vector3dX(), VesPosition.Vector3dY(), VesPosition.Vector3dZ()), true);
			}
		}
		static double GetVesselAltitude(bool includePlanetRadius, Vessel v)
		{
			if (includePlanetRadius)
			{
				return v.altitude + v.mainBody.Radius;
			}
			return v.altitude;
		}
		//Unused
		static double GetVesselLongPos(Vector3d pos, Vessel v)
		{
			return v.GetLongitude(pos, false);
		}
		//Unused
		static double GetVesselLatPos(Vector3d pos, Vessel v)
		{
			return v.GetLatitude(pos, false);
		}
		static double CalculateGravPot(CelestialBody cb, Vessel v)
		{
			return cb.gravParameter / Math.Pow(GetVesselAltitude
											   (true, v), 2);
		}
	}
}
