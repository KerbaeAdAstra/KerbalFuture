using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class SpaceFolderData : MonoBehaviour
	{
		public static List<uint> vesselParts = FinePrint.Utilities.VesselUtilities.GetPartIDList(CurrentVessel());
		
		public static string DLLPath()
		{
			return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
		public static double ResourceAmountOnVessel(string resource, Vessel vessel)
		{
			return FinePrint.Utilities.VesselUtilities.VesselResourceAmount(resource, vessel);
		}
		public static Vessel CurrentVessel()
		{
			return FlightGlobals.ActiveVessel;
		}
		//public static double ResourceAmountNeeded(Vessel vessel)
	}
}