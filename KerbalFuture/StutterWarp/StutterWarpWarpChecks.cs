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
			VesselData vesData = new VesselData(v);
			
			FlightDrive.WarpVessel(v);
		}
		//Calculation functions
		double SpatiofibrinCalc(List<double[]> waveGenValues)
		{
			
		}
		double ElectricityCalc(List<double[]> waveGenValues, double amplitude)
		{
			/*total energy for initialization: (Ik(x^2))/(nE)
			*where:
			*I is interference between wavegens
			*k is Hooke's constant of spacetime
			*x is amplitude (from GUI, set by player)
			*n is number of wavegens
			*E is efficiency of the wavegen
			*/
			int n = waveGenValues.GetLength(0);
			int I, E;
		}
		double SegFaultCalc(double wavelength, double amplitude, List<double[]> weaverValues)
		{
			
		}
		//Value getting functions
		List<double[]> GetWaveGenValues(Vessel v)//in order of sfNeeded, ecNeeded, maxWavelength, minWavelength, maxAmplitude, minAmplitude
		{
			List<Part> list = new List<Part>();
			list = v.Parts;//there exist both Vessel.parts and Vessel.Parts, we are using the second one because the first one becomes unloaded when the focus is not on the vessel.
			List<double[]> returnList = new List<double[]>();
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("WaveGenerator"))
				{
					returnList.Add(list[i].Modules["WaveGenerator"].ValueList());
				}
			}
			return returnList;
		}
		List<double[]> GetWeaverValues(Vessel v)
		{
			List<Part> list = new List<Part>();
			list = v.Parts;
			List<double[]> returnList = new List<double[]>();
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("Weaver"))
				{
					returnlist.Add(list[i].Modules["Weaver"].ValueList());
				}
			}
			return returnList;
		}
	}
}