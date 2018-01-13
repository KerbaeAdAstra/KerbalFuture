using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Tuples;
using Hlpr

namespace StutterWarp
{
	class WarpChecks : MonoBehaviour
	{
		const double HOOKES_CONSTANT_OF_SPACETIME = 2000.0;//FIXME
		
		public bool CheckWarp(Vessel v, bool warpVesselAfterCheck)//Call CheckWarp instead of calling WarpVessel. bool so that it can report the status of the warp back to the calling member
		{
			FlightGlobals fgs = new FlightGlobals();
			VesselData vesData = new VesselData(v);
			if(vesData.ResourceAmountOnVessel("ElectricCharge", v) < ElectricityCalc(v, KFGUI.Stutter.amplitude))
				return false;
			
			if(warpVesselAfterCheck)//switchable so that it can be called to check if the warp is valid without actually warping
			{
				
				FlightDrive.WarpVessel(SegFaultCalc(v, KFGUI.Stutter.wavelength, KFGUI.Stutter.amplitude, GetWeaverValues(v)));
			}
			return true;
		}
		//Calculation functions
		double SpatiofibrinCalc(Vessel v, List<double[]> waveGenValues)
		{
			
		}
		double ElectricityCalc(Vessel v, double amplitude, bool consumeCalculatedResources)
		{
			/*total energy for initialization: ((I^(n-1))k(x^2))/(nE)
			*where:
			*I is interference between wavegens
			*k is Hooke's constant of spacetime
			*x is amplitude (from GUI, set by player)
			*n is number of wavegens
			*E is efficiency of the wavegen
			*
			*this is run for each drive, and then the results are added up for the final amount. 
			*/
			int n = waveGenValues.GetLength(0);
			int x = amplitude;
			List<double[]> wgv = new List<double[]>();
			wgv = GetWaveGenValues(v);
			double returnValue = 0;
			for(int i = 0, i < wgv.GetLength(0), i++)
			{
				ElectricityCalc()
				returnValue += ((Math.Pow(wgv[i][7], n-1)*HOOKES_CONSTANT_OF_SPACETIME*Math.Pow(x,2))/(n*wgv[i][6]));
			}
			return returnValue;
		}
		double SegFaultCalc(Vessel v, double wavelength, double amplitude, List<double[]> weaverValues)
		{
			
		}
		List<Part> partsWithModuleX(string partModule)
		{
			List<Part> list = new List<Part>();
			list = v.Parts;
			List<Part> returnList = new List<Part>();
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i].Modules.Contains("WaveGenerator"))
				{
					returnList.Add(list[i]);
				}
			}
			return returnList;
		}
		//Value getting functions
		List<double[]> GetWaveGenValues(Vessel v)//in order of sfNeeded, ecNeeded, maxWavelength, minWavelength, maxAmplitude, minAmplitude
		{
			List<Part> list = new List<Part>();
			list = v.Parts;//there exist both Vessel.parts and Vessel.Parts, we are using the second one because the first one becomes unloaded when the focus is not on the vessel.
			List<double[]> returnList = new List<double[]>();
			for(int i = 0; i < list.Count; i++) //Iterates through the list of parts in the vessel
			{
				if(list[i].Modules.Contains("WaveGenerator"))
				{
					returnList.Add(list[i].Modules["WaveGenerator"].ValueList()); //this is a list of parts that you get the PartModuleList from and then use this function 'Modules["ModuleName"] to get the module information. I have the function ValueList() in my part modules that passes me a list of floats (as all numbers in this part module are floats). It would be different if you had a part module that used bools and ints, for example. You can also access specific fields in the part module the same way, but substitute 'ValueList()' for the name of the field. 
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