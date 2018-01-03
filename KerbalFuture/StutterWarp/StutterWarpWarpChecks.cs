using System;

namespace StutterWarp
{
	class WarpChecks
	{
		public bool CheckWarp(Vessel v)
		{
			FlightGlobals fgs = new FlightGlobals();
			VesselData vesData = new VesselData(fgs.activeVessel);
			
		}
	}
}