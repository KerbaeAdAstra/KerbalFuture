using System.Collections.Generic;

namespace KerbalFuture.Utils
{
	class VesselData //not really used except for the function ResourceAmountOnVessel
	{
		private PartSet vesselParts;
		HashSet<Part> vesselPartHashSet;
		Vessel vesDataVessel { get; private set; } //private set so it can be set when constructed, but not by anything else

		public VesselData(Vessel vessel)
		{
			vesselParts = new PartSet(vessel);
			vesselPartHashSet = vesselParts.GetParts();
			vesDataVessel = vessel;
		}
		public void UpdateVesselData()
		{
			vesselParts = new PartSet(vesDataVessel);
			vesselPartHashSet = vesselParts.GetParts();
		}
		public double ResourceAmountOnVessel(string resource) => 
	    	FinePrint.Utilities.VesselUtilities.VesselResourceAmount(resource, vesDataVessel);
	}
}