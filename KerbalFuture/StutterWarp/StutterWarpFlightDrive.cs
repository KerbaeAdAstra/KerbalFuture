using System;
using Hlpr;

namespace StutterWarp
{
	class FlightDrive : VesselModule
	{
		//this may not be the right way to do it, but it's my way, so suck it.
		double frequency, amplitude;
		double probabilityOfFault;
		CelestialBody warpBody, currentBody;
		double warpLat, warpLong, warpAlt;
		double currX, currY, currZ;
		Vector3d warpVector, currVector;
		internal double distance;
		
		public void WarpVessel(Vessel v)
		{
			double xPos, yPos, zPos;
//TODO
			//warpLong = KFGUI.Stutter.warpLong;
			//warpLat = KFGUI.Stutter.warpLat;
			//warpAlt = KFGUI.Stutter.warpAlt;
			//frequency = KFGUI.Stutter.frequency;
			//amplitude = KFGUI.Stutter.amplitude;
			//Conversions from lat-long-alt coords to xyz
			LatLongHelper LLH = new LatLongHelper();
			xPos = LLH.XFromLatLongAlt(warpLat, warpLong, LLH.GetVesselAltitude(true, v));
			yPos = LLH.YFromLatLongAlt(warpLat, warpLong, LLH.GetVesselAltitude(true, v));
			zPos = LLH.ZFromLatAlt(warpLat, LLH.GetVesselAltitude(true, v));
			//End conversions
			warpVector = Vector3dHelper.ConvertXYZCoordsToVector3d(xPos, yPos, zPos); //Set warp vector
			currVector = v.GetWorldPos3D(); //Get the current position of the vessel
			ConvertVector3dToXYZCoords(currVector, ref currX, ref currY, ref currZ); //Set current position values
			distance = Distance(warpVector, currVector); //Get the distance between the current position and the final position
			v.SetPostition(warpVector, true); //Set the position of the vessel to the warpVector
		}
	}
}