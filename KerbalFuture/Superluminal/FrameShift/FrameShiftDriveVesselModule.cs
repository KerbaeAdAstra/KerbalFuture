using UnityEngine;
using KerbalFuture.Superluminal.FrameShift; 

namespace KerbalFuture.Superluminal.FrameShift
{
	public class FrameShiftDriveVesselModule : VesselModule
	{
		//Fires on Vessel startup
		protected override void OnStart()
		{
			base.OnStart();
			Debug.Log("[KF] FSD Vessel module starting for " + Vessel.GetDisplayName());
		}
		public void WarpVessel(FrameShiftWarpData warpData, out Error err)
		{
			Debug.Log("[KF] FSD Warp called for vessel " + Vessel.GetDisplayName());
			Error internFault = err = FrameShiftWarpChecks.WarpAvalible(Vessel);
		}
	}
}