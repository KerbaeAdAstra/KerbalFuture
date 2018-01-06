using System;
using System.Array;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hlpr

namespace StutterWarp
{
	class WarpChecks : MonoBehaviour
	{
		public bool CheckWarp(Vessel v)
		{
			FlightGlobals fgs = new FlightGlobals();
			VesselData vesData = new VesselData(fgs.activeVessel);
		}
		float[] GetWaveAndWeaverValues(Vessel v)//WaveGen, then Weaver. Weaver starts at index 6.
		{
			float[] arr = new float[8];
			GetWaveGenValues(v).CopyTo(arr, 0);
			GetWeaverValues(v).CopyTo(arr, 6);
			return arr;
		}
		float[] GetWaveGenValues(Vessel v)//in order of sfNeeded, ecNeeded, maxWavelength, minWavelength, maxAmplitude, minAmplitude
		{
			List<Part> list = new List<Part>();
			list = v.Parts;//there exist both Vessel.parts and Vessel.Parts, we are using the second one because the first one becomes unloaded when the focus is not on the vessel. 
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("WaveGenerator"))
				{
					return list[i].Modules["WaveGenerator"].ValueList();
				}
			}
		}
		float[] GetWeaverValues(Vessel v)
		{
			List<Part> list = new List<Part>();
			list = v.Parts;
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("Weaver"))
				{
					return list[i].Modules["Weaver"].ValueList();
				}
			}
		}
	}
}