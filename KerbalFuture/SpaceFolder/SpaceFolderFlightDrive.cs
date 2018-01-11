using System;
using Hlpr;
//using KFGUI; TODO

namespace SpaceFolder
{
	class FlightDrive : VesselModule
	{
		Vector3d cbPos;
		double vesHeight;
		CelestialBody vesBody;
		CelestialBody warpBody;
		double warpLong, warpLat;
		double bodyGravPot;
		
		public void WarpVessel(Vessel v)
		{
			double cbx = 0, cby = 0, cbz = 0;
			vesBody = v.mainBody;
//TODO
			//warpBody = KFGUI.ChosenBody();
			//warpLong = KFGUI.ChosenLong();
			//warpLat = KFGUI.ChosenLat();
			bodyGravPot = CalculateGravPot(vesBody, v);
			cbPos = warpBody.position;
			Vector3dHelper VesPosition = new Vector3dHelper();
			LatLongHelper LLH = new LatLongHelper();
			Vector3dHelper.ConvertVector3dToXYZCoords(cbPos, ref cbx, 
				ref cby, ref cbz);
			VesPosition.SetX(cbx + LLH.XFromLatLongAlt(warpLat, warpLong, 
				bodyGravPot));
			VesPosition.SetY(cby + LLH.YFromLatLongAlt(warpLat, warpLong, 
			   bodyGravPot));
			VesPosition.SetZ(cbz + LLH.ZFromLatAlt(warpLat,  bodyGravPot));
			v.SetPosition(Vector3dHelper.ConvertXYZCoordsToVector3d(
				VesPosition.Vector3dX(), VesPosition.Vector3dY(),
				VesPosition.Vector3dZ()), true);
		}
		double CalculateGravPot(CelestialBody cb, Vessel v)
		{
			LatLongHelper CGPLLH = new LatLongHelper();
			return cb.gravParameter / CGPLLH.GetVesselAltitude(true, v);
		}
	}
}
