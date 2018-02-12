using System.Collections.Generic;

namespace KerbalFuture.Utils
{
	internal class VesselData //not really used except for the function ResourceAmountOnVessel
	{
		private PartSet vesselParts;
		HashSet<Part> vesselPartHashSet;

		public VesselData(Vessel vessel)
		{
			vesselParts = new PartSet(vessel);
			vesselPartHashSet = vesselParts.GetParts();
		}
		public void UpdateVesselData(Vessel vessel)
		{
			vesselParts = new PartSet(vessel);
			vesselPartHashSet = vesselParts.GetParts();
		}
		public double ResourceAmountOnVessel(string resource, Vessel vessel) => 
	    	FinePrint.Utilities.VesselUtilities.VesselResourceAmount(resource, vessel);
	}
}