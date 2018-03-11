using System.Collections.Generic;
using FinePrint.Utilities;
using KerbalFuture.Utils;
using SpaceFolder;

// using KFGUI; TODO

namespace KerbalFuture.SpaceFolder
{
	class SpaceFolderFlightDrive : VesselModule
	{
		private Vector3d cbPos;
		private double vesHeight;
		private CelestialBody vesBody;
		private CelestialBody warpBody;
		private double warpLong, warpLat;
		private double bodyGravPot;
		SpaceFolderWarpChecks insWarpChecks = new SpaceFolderWarpChecks();
		
		internal void WarpVessel(IEnumerable<Tuple<Part, double, string, string>> driveList, double ecToUse)//second double in the tuple list is the percentege of ec that the specific engine used
		{
			// Checks to make sure that the vessel actually has a spacefolder drive
			if (!VesselUtilities.VesselHasModuleName("SpaceFolderEngine", vessel))
			{
				return;
			}
			double cbx = 0, cby = 0, cbz = 0;//gives meaningless values so they can be used in ref statements
			vesBody = vessel.mainBody;//sets the current body to the vessel's body
			// TODO
			// warpBody = KFGUI.ChosenBody();
			// warpLong = KFGUI.ChosenLong();
			// warpLat = KFGUI.ChosenLat();
			bodyGravPot = CalculateGravPot(vesBody, vessel);
			cbPos = warpBody.position;//sets the warp body position in memory
			LatLongHelper LLH = new LatLongHelper();//new instance of LatLongHelper
			Vector3dHelper.ConvertVector3dToXYZCoords(cbPos, ref cbx, ref cby, ref cbz);
			double warpX = cbx + LLH.XFromLatLongAlt(warpLat, warpLong, bodyGravPot);
			double warpY = cby + LLH.YFromLatLongAlt(warpLat, warpLong, bodyGravPot);
			double warpZ = cbz + LLH.ZFromLatAlt(warpLat,  bodyGravPot);
			// Use electricity
			foreach (Tuple<Part, double, string, string> t in driveList)
			{
				WarpHelp.UseElectricity(t.item1, t.item2 * ecToUse, t.item3);
			}
			vessel.SetPosition(Vector3dHelper.ConvertXYZCoordsToVector3d(
				warpX, warpY, warpZ), true);
		}
		private static double CalculateGravPot(CelestialBody cb, Vessel v)
			=> cb.gravParameter / new LatLongHelper().GetVesselAltitude(true, v);
	}
}