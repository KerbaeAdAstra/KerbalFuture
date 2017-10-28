using System.Collections.Generic;
using UnityEngine;

namespace SpaceFolder
{
	class VesselData : MonoBehaviour
	{
		private PartSet vesselParts;
		private HashSet<Part> vesselPartHashSet;
		
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
		public double ResourceAmountOnVessel(string resource, Vessel vessel)
		{
			return FinePrint.Utilities.VesselUtilities.VesselResourceAmount(resource, vessel);
		}
	}
}