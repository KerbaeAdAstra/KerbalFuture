namespace KerbalFuture.Superluminal.FrameShift
{
	public struct FrameShiftWarpData
	{
		public FrameShiftWarpData(Vessel v, double velocity)
		{
			warpVessel = v;
			warpVelocity = velocity;
		}
		public Vessel warpVessel { get; }
		public double warpVelocity { get; }
	}
}