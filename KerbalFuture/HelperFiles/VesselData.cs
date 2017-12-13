using System.Collections.Generic;
using UnityEngine;

namespace Fuck
{
	class VesselData : MonoBehaviour
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
		FinePrint.Utilities.VesselUtilities.VesselResourceAmount
				 (resource, vessel);
	}
}