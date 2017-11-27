namespace FrameShift
{
	class FrameShiftEngine : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float warpDriveDiameter;
		
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityNeededForWarp;
	}
}
