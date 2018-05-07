using System.Collections.Generic;
using System.Linq;
using KerbalFuture.Utils;
using UnityEngine;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class SpaceFolderWarpChecks
	{
        // Checks if the warp is avalible, returning a bitwise encoding of any errors encountered. 
        // If the returned int is equal to 0 (zero), WarpAvalible has encountered no problems with warping. 
		public static Error WarpAvailable(Vessel v)
		{
			Error retval = Error.ClearForWarp;
			// To warp, vessel needs SFD of correct size and resources
			if (!VesselContainsSpaceFolderDrive(v))
			{
                retval = Error.DrivesNotFound;
			}
			//Creates a ship construct from the ship and parts
			//Launch building doesn't matter because all we need is the box, the orientation of it doesn't matter
			ShipConstruct sc = new ShipConstruct(v.GetName(), EditorFacility.VAB, v.Parts);
			List<Part> sfdList = new List<Part>();
			sfdList = VesselSpaceFolderDrives(v);
			// Checks the vessel size vs the max warp hole size
			if (VesselDiameterCalc(ShipConstruction.CalculateCraftSize(sc), ShipConstruction.FindCraftCenter(sc)) > MaxWarpHoleSize(sfdList))
			{
				retval = Error.VesselTooLarge | retval;
			}
			VesselResourceSimulation vrs = new VesselResourceSimulation(v, true);
			if (vrs.Status != SimulationStatus.Succeeded)
            {
                retval = Error.InsufficientResources | retval;
            }
            return retval;
		}
		//Gets the diameter of the vessel
		public static double VesselDiameterCalc(Vector3 boundingBoxSize, Vector3 vesselCenter)
		{
			double xrad, yrad, zrad;
			xrad = boundingBoxSize.x / 2;
			yrad = boundingBoxSize.y / 2;
			zrad = boundingBoxSize.z / 2;
			//Gets the radius of the sphere
			//Works by getting the box dimensions and then calculating the distance
			//from a virtual center
			double radius = WarpHelp.Distance(0, 0, 0, xrad, yrad, zrad);
			return radius * 2;
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
				if (p.Modules.Contains("ModuleSpaceFolderEngine"))
				{
					return true;
				}
			}
			return false;
		}
        // Returns a list of SpaceFolderDrives
		public static List<Part> VesselSpaceFolderDrives(Vessel v)
		{
			List<Part> outList = new List<Part>();
			foreach(Part p in v.Parts)
			{
				if (p.Modules.Contains("ModuleSpaceFolderEngine"))
				{
					outList.Add(p);
				}
			}
			return outList;
		}
	}
}