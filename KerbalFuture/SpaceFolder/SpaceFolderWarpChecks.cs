using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceFolder
{
	class SpaceFolderWarpChecks : MonoBehaviour
	{
		static bool goodToGo;
		static double vesselDiameter;
		static double maxWarpHoleSize;
		static double spaciofibrinNeeded;
		
		public static double SpaciofibrinWarpCalc(List<double>[] engineSizes)
		{
			double returnAmount = 0;
			foreach(double d in engineSizes)
			{
				returnAmount += Math.Pow(Math.E, d/5);//TODO, fix values
			}
			return returnAmount;
		}
		private static double MaxWarpHoleSize(List<double> engineSizes)
		{
			List<double> dividers = new List<double>(){0.8, 0.6, 0.4, 0.2};
			double divider = 0.1;
			int engineCount = 0;
			double totSize = 0;
			double realSize = 0;
			foreach(double d in engineSizes)
			{
				totSize += d;
			}
			engineCount = engineSizes.Count();
			engineSizes = BigToSmallSortedDoubleList(engineSizes);
			for(int i = 0; i <= engineCount; i++)
			{
				if (i == 0)
				{
					realSize += engineSizes[i];
				}
				else if(i > 0 && i < 5)
				{
					realSize += (engineSizes[i] * dividers[i+1]);
				}
				else
				{
					realSize += engineSizes[i] * divider;
					divider = divider/2;
				}
			}
			return realSize;
		}
		public static List<double> BigToSmallSortedDoubleList(List<double> list)
		{
			List<double> sortedList = new List<double>();
			while(list.Count() != 0)
			{
				sortedList.Add(list.Min());
				list.Remove(list.Min());
			}
			sortedList.Reverse();
			return sortedList;
		}
		public static List<double> SmallToBigSortedDoubleList(List<double> list)
		{
			List<double> sortedList = new List<double>();
			while(list.Count() != 0)
			{
				sortedList.Add(list.Min());
				list.Remove(list.Min());
			}
			return sortedList;
		}
		public static double ElectricityWarpCalc(List<double>[] engineInfo)
		{
			double returnAmount = 0;
			foreach(double d in engineInfo[0])
			{
				returnAmount += Math.Pow(Math.E, d/5);
			}
			return returnAmount;
		}
		public static bool GoodToGo()
		{
			return goodToGo;
		}
		public static void InitiateWarpCheck(List<double>[] engineSizes, float vesDiameter) //called by GUI, sets bool goodToGo
		{
			FlightGlobals fgs = new FlightGlobals();
			vesselDiameter = vesDiameter;
			goodToGo = false;
			//constructs a new VesselData class
			VesselData vesData = new VesselData(fgs.activeVessel);
			//If vessel !have Spatiofibrin, return
			spaciofibrinNeeded = SpaciofibrinWarpCalc(engineSizes);
			if(vesData.ResourceAmountOnVessel("spatiofibrin", fgs.activeVessel) < spaciofibrinNeeded)
			{
				return;
			}
			// If warpDrive.diameter < vesselSize, return
			if(MaxWarpHoleSize(engineSizes) < vesselDiameter)
			{
				return;
			}
			goodToGo = true;
			FlightDrive.WarpVessel(fgs.activeVessel);
		}
	}
}
