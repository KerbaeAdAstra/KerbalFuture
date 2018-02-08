using System;
using System.Collections.Generic;
using System.Linq;
using Hlpr;
using FinePrint;
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
		SpaceFolderWarpChecks insWarpChecks = new SpaceFolderWarpChecks();
		
		internal void WarpVessel(List<Tuple<Part, double/*percentage of ec usage*/>> driveList, double ecToUse)
		{
			if(!FinePrint.Utilities.VesselUtilities.VesselHasModuleName("SpaceFolderEngine", this.vessel)) //Checks to make sure that the vessel actually has a spacefolder drive
				return;
			double cbx = 0, cby = 0, cbz = 0;
			vesBody = this.vessel.mainBody;
//TODO
			//warpBody = KFGUI.ChosenBody();
			//warpLong = KFGUI.ChosenLong();
			//warpLat = KFGUI.ChosenLat();
			bodyGravPot = CalculateGravPot(vesBody, this.vessel);
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
			//Use electricity
			for(int i = 0; i < driveList.Count; i++)
			{
				UseElectricity(driveList[i].item1, driveList[i].item2*ecToUse);
			}
			this.vessel.SetPosition(Vector3dHelper.ConvertXYZCoordsToVector3d(
				VesPosition.Vector3dX(), VesPosition.Vector3dY(),
				VesPosition.Vector3dZ()), true);
		}
		double CalculateGravPot(CelestialBody cb, Vessel v)
		{
			LatLongHelper CGPLLH = new LatLongHelper();
			return cb.gravParameter / CGPLLH.GetVesselAltitude(true, v);
		}
		void UseElectricity(Part part, double amount)
		{
			part.RequestResource("Electricity", amount);
		}
	}
}