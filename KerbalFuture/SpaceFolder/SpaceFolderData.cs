using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FinePrint.Utilities;
using UnityEngine;
using KerbalFuture;

namespace SpaceFolder
{
	class SpaceFolderData : MonoBehaviour
	{
		private string activeVessel;
		private string dllPath;
		
		public static string DLLPath()
		{
			dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			return dllPath;
		}
		public static double ResourceAmountOnVessel(string resource, Vessel vessel)
		{
			return VesselUtilities.VesselResourceAmount(resource, vessel);
		}
		public static Vessel CurrentVessel()
		{
			activeVessel = FlightGlobals.ActiveVessel;
			return activeVessel;
		}
	}
}