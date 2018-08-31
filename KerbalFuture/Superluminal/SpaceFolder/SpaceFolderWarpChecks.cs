using System;
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
			Debug.Log("[KF] Checking warp avalibility for vessel " + v.GetDisplayName());
			Error retval = Error.ClearForWarp;
			// To warp, vessel needs SFD of correct size and resources
			if (!VesselContainsSpaceFolderDrive(v))
			{
                retval = Error.DrivesNotFound;
			}
			//Creates a ship construct from the ship and parts
			//Launch building doesn't matter because all we need is the box, the orientation of it doesn't matter
			List<Part> sfdList = new List<Part>();
			sfdList = SFWarpHelp.PartsWithModuleSFD(v);
			// Checks the vessel size vs the max warp hole size
			if (WarpHelp.VesselDiameterCalc(v) > MaxWarpHoleSize(sfdList))
			{
				retval = Error.VesselTooLarge | retval;
			}
			//Checks the vessel resource amounts against the needed resource amounts
			VesselResourceSimulation vrs = new VesselResourceSimulation(v, true);
			if (vrs.Status != SimulationStatus.Succeeded)
            {
                retval = Error.InsufficientResources | retval;
            }
            return retval;
		}
        // Calculates the maximum warp hole size producable for a set of engines
		public static double MaxWarpHoleSize(IEnumerable<Part> engines)
		{
			Debug.Log("[KF] Calculating the max warp hole size for a vessel");
			IEnumerable<double> diamList = from part in engines
										   select SFWarpHelp.SFDModuleFromPart(part).PartDriveData.Diameter;
			double squaredVals = 0;
			foreach(double diam in diamList)
			{
				squaredVals += Math.Pow(diam, 2);
			}
			return Math.Pow(squaredVals, 0.5);
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
	}
}