namespace KerbalFuture.SpaceFolder
{
	internal class SpaceFolderEngine : PartModule
	{
		// Do not access directly, method below for that.
		[KSPField(isPersistant = true, guiActive = false)]
		public float warpDriveDiameter;
		[KSPField(isPersistant = true, guiActive = false)]
		public float engineMultiplier;
		
		public double[] SFDEngineValues() => new double[] {warpDriveDiameter, engineMultiplier};
	}
}
