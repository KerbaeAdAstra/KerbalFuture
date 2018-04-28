using System.Collections.Generic;
using System.Linq;
using KerbalFuture.Utils;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class SpaceFolderWarpChecks
	{
        // Checks if the warp is avalible, returning a bitwise encoding of any errors encountered. 
        // If the returned int is equal to 0 (zero), WarpAvalible has encountered no problems with warping. 
		public static int WarpAvailable(SpaceFolderWarpData warpData, Vessel v)
		{
            int retval = 0;
			// To warp, vessel needs SFD of correct size and resources
			if (!VesselContainsSpaceFolderDrive(v))
			{
                retval += (int)Error.DrivesNotFound;
			}
			// seen on forum post: 
			// https://forum.kerbalspaceprogram.com/index.php?/topic/116071-getting-vessel-size/&do=findComment&comment=2067825
			// that the x value is the diameter of the ship. Thanks Thomas P!
			// TODO get vessel launch building and use that instead of just EditorFacilities.VAB
			ShipConstruct sc = new ShipConstruct(v.GetName(), EditorFacility.VAB, v.Parts);
			double vesselDiameter = sc.shipSize.x;
			List<Part> sfdList = new List<Part>();
			sfdList = VesselSpaceFolderDrives(v);
			// Checks the vessel size vs the max warp hole size
			if (vesselDiameter > MaxWarpHoleSize(sfdList))
			{
                retval += (int)Error.VesselTooLarge;
			}
			VesselResourceSimulation vrs = new VesselResourceSimulation(v, sfdList, true);
			vrs.RunSimulation();
			if (vrs.Status != SimulationStatus.Succeeded)
            {
                retval += (int)Error.InsufficientResources;
            }
            return retval;
		}
        // Calculates the maximum warp hole size producable for a set of engines
		public static double MaxWarpHoleSize(IEnumerable<Part> engines)
		{
			double[] dividers = { 0.8, 0.6, 0.4, 0.2 };
			double divider = 0.1;
			double realSize = 0;
			List<SpaceFolderDriveData> driveData = engines.Select(p => ((ModuleSpaceFolderEngine) 
				p.Modules["ModuleSpaceFolderEngine"]).PartDriveData).ToList();
			for (int i = 0; i < driveData.Count; i++)
			{
				if (i == 0)
				{
					realSize += driveData[i].Diameter;
				}
				else if (i > 0 && i < 5)
				{
					realSize += driveData[i].Diameter * dividers[i - 1];
				}
				else
				{
					realSize += driveData[i].Diameter * divider;
					divider /= 2;
				}
			}

			return realSize;
		}
        // Checks if the vessel has a SpaceFolderDrive
		public static bool VesselContainsSpaceFolderDrive(Vessel v)
		{
			foreach (Part p in v.Parts)
			{
				if (p.Modules.Contains("ModuleSpaceFolderDrive"))
				{
					return true;
				}
			}
			return false;
		}
        // Checks and returns an out List<Part> of parts with SpaceFolderDrive
		public static bool VesselContainsSpaceFolderDrive(Vessel v, out List<Part> partsWithModule)
		{
			List<Part> outList = new List<Part>();
			foreach (Part p in v.Parts)
			{
				if (p.Modules.Contains("ModuleSpaceFolderDrive"))
				{
					outList.Add(p);
				}
			}
			partsWithModule = outList;
			return outList.Count > 0;
		}
        // Returns a list of SpaceFolderDrives
		public static List<Part> VesselSpaceFolderDrives(Vessel v)
		{
			List<Part> outList = new List<Part>();
			foreach(Part p in v.Parts)
			{
				if (p.Modules.Contains("ModuleSpaceFolderDrive"))
				{
					outList.Add(p);
				}
			}
			return outList;
		}
	}
}