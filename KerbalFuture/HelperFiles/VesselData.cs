using System.Collections.Generic;
using UnityEngine;

namespace Hlpr
{
	class VesselData
	{
		PartSet vesselParts;
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