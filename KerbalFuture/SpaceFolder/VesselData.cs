using System.Collections.Generic;
using KerbalFuture;
using UnityEngine;

namespace SpaceFolder
{
	class VesselData : MonoBehaviour
	{
		private var vesselParts;
		private var vesselPartHashSet;
		
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
		public bool VesselContainsModule(Vessel vessel, string className)
		{
			UpdateVesselData(vessel);
			var p;
			for(int i = 0; i < vesselPartHashSet.Count(); i++)
			{
				p = vesselPartHashSet[i];
				var partModuleList = new PartModuleList(p);
				if(partModuleList.Contains(className)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		public bool VesselContainsModule(Vessel vessel, int classID)
		{
			UpdateVesselData(vessel);
			var p;
			for(int i = 0; i < vesselPartHashSet.Count(); i++)
			{
				p = vesselPartHashSet[i];
				var partModuleList = new PartModuleList(p);
				if(partModuleList.Contains(classID)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		
	}
}
