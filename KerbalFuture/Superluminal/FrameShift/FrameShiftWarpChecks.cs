using System.Collections.Generic;
using System;

namespace KerbalFuture.Superluminal.FrameShift
{
	public class FrameShiftWarpChecks
	{
		public const double ALCUBIERRE_CONSTANT_OF_SPACETIME = 2000.0;
		public static Error WarpAvalible(Vessel v, double velocity)
		{
			//Creates and fills in one line! Marvelous!
			List<FrameShiftDriveData> dd = new List<FrameShiftDriveData>(FSWarpHelp.DriveDataList(v));
			//Creates the sum of all the capacities
			double capSum = 0;
			foreach(FrameShiftDriveData d in dd)
			{
				capSum += d.Capacity;
			}
			capSum = Math.Pow(capSum, 1 / 4);
			capSum *= ALCUBIERRE_CONSTANT_OF_SPACETIME;
		}
	}
}