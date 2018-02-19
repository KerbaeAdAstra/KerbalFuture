using System.Collections.Generic;
using FinePrint.Utilities;
using KerbalFuture.Utils;

// using KFGUI; TODO

namespace KerbalFuture.SpaceFolder
{
	internal class FlightDrive : VesselModule
	{
		private Vector3d cbPos;
		double vesHeight;
		private CelestialBody vesBody;
		CelestialBody warpBody;
		double warpLong, warpLat;
		private double bodyGravPot;
		SpaceFolderWarpChecks insWarpChecks = new SpaceFolderWarpChecks();
		
		internal void WarpVessel(List<Tuple<Part, double, string, string>> driveList, double ecToUse)//second double in the tuple list is the percentege of ec that the specific engine used
		{
			// Checks to make sure that the vessel actually has a spacefolder drive
			if (!VesselUtilities.VesselHasModuleName("SpaceFolderEngine", vessel)) return;
			double cbx = 0, cby = 0, cbz = 0;
			vesBody = vessel.mainBody;
			// TODO
			// warpBody = KFGUI.ChosenBody();
			// warpLong = KFGUI.ChosenLong();
			// warpLat = KFGUI.ChosenLat();
			bodyGravPot = CalculateGravPot(vesBody, vessel);
			cbPos = warpBody.position;
			Vector3dHelper VesPosition = new Vector3dHelper();
			LatLongHelper LLH = new LatLongHelper();
			Vector3dHelper.ConvertVector3dToXYZCoords(cbPos, ref cbx, ref cby, ref cbz);
			VesPosition.Vector3dX = cbx + LLH.XFromLatLongAlt(warpLat, warpLong, bodyGravPot);
			VesPosition.Vector3dY = cby + LLH.YFromLatLongAlt(warpLat, warpLong, bodyGravPot);
			VesPosition.Vector3dZ = cbz + LLH.ZFromLatAlt(warpLat,  bodyGravPot);
			// Use electricity
			for(int i = 0; i < driveList.Count; i++)
			{
				WarpHelp.UseElectricity(driveList[i].item1, driveList[i].item2*ecToUse, driveList[i].item3);
			}
			vessel.SetPosition(Vector3dHelper.ConvertXYZCoordsToVector3d(
				VesPosition.Vector3dX, VesPosition.Vector3dY, VesPosition.Vector3dZ), true);
		}
		private static double CalculateGravPot(CelestialBody cb, Vessel v)
			=> cb.gravParameter / new LatLongHelper().GetVesselAltitude(true, v);
	}
}