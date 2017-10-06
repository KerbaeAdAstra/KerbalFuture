using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FinePrint.Utilities;
using UnityEngine;

namespace KerbalFuture
{
	class SpaceFolderData : MonoBehaviour
	{
		public static List<uint> vesselParts = VesselUtilities.GetPartIDList(CurrentVessel());
		
		public static string DLLPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		public static double ResourceAmountOnVessel(string resource, Vessel vessel)
		{
			return VesselUtilities.VesselResourceAmount(resource, vessel);
		}

		public static Vessel CurrentVessel()
		{
			return FlightGlobals.ActiveVessel;
		}

		public static void ResourceAmountNeeded(Vessel vessel)
		{
			throw new NotImplementedException();
		}
	}
}