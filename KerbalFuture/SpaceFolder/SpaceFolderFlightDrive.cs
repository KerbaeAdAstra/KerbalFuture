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
		
		public static void WarpVessel(Vessel v)
		{
			double cbx = 0, cby = 0, cbz = 0;
			if (SpaceFolderWarpChecks.GoodToGo())
			{
				vesBody = v.mainBody;
//TODO
				//warpBody = KFGUI.ChosenBody();
				//warpLong = KFGUI.ChosenLat();
				//warpLat = KFGUI.ChosenLong();
				bodyGravPot = CalculateGravPot(vesBody, v);
				cbPos = warpBody.position;
				Vector3dHelper VesPosition = new Vector3dHelper();
				LatLongHelper LLH = new LatLongHelper();
				Vector3dHelper.ConvertVector3dToXYZCoords(cbPos, ref cbx, ref cby, ref cbz);
				VesPosition.SetX(cbx + LLH.XFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetY(cby + LLH.YFromLatLongAlt(warpLat, warpLong, bodyGravPot));
				VesPosition.SetZ(cbz + LLH.ZFromLatAlt(warpLat,  bodyGravPot));
				v.SetPosition(Vector3dHelper.ConvertXYZCoordsToVector3d(VesPosition.Vector3dX(), VesPosition.Vector3dY(), VesPosition.Vector3dZ()), true);
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
			return v.longitude;
		}
		//Unused
		static double GetVesselLatPos(Vector3d pos, Vessel v)
		{
			return v.latitude;
		}
		static double CalculateGravPot(CelestialBody cb, Vessel v)
		{
			return cb.gravParameter / Math.Pow(GetVesselAltitude
											   (true, v), 2);
		}
	}
}
