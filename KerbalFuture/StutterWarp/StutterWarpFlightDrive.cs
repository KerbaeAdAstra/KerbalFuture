using System;
using Hlpr;

namespace StutterWarp
{
	class FlightDrive : VesselModule
	{
		//this may not be the right way to do it, but it's my way, so suck it.
		static double frequency, amplitude;
		static double probabilityOfFault;
		static CelestialBody warpBody, currentBody;
		static double warpLat, warpLong, warpAlt;
		static double currX, currY, currZ;
		static Vector3d warpVector, currVector;
		static internal double distance;
		public static void WarpVessel(Vessel v)
		{
			double xPos, yPos, zPos;
//TODO
			//warpLong = KFGUI.Stutter.warpLong;
			//warpLat = KFGUI.Stutter.warpLat;
			//warpAlt = KFGUI.Stutter.warpAlt;
			//frequency = KFGUI.Stutter.frequency;
			//amplitude = KFGUI.Stutter.amplitude;
			LatLongHelper LLH = new LatLongHelper();
			xPos = LLH.XFromLatLongAlt(warpLat, warpLong, LLH.GetVesselAltitude(true, v));
			yPos = LLH.YFromLatLongAlt(warpLat, warpLong, LLH.GetVesselAltitude(true, v));
			zPos = LLH.ZFromLatAlt(warpLat, LLH.GetVesselAltitude(true, v));
			warpVector = Vector3dHelper.ConvertXYZCoordsToVector3d(xPos, yPos, zPos);
			currVector = v.GetWorldPos3D();
			ConvertVector3dToXYZCoords(currVector, ref currX, ref currY, ref currZ);
			distance = Distance(warpVector, currVector);
			v.SetPostition(warpVector, true);
		}
	}
}