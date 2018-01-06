namespace SpaceFolder
{
	class SpaceFolderEngine : PartModule
	{
		//Do not access directly, method below for that. 
		[KSPField(isPersistant = true, guiActive = false)]
		public float warpDriveDiameter;

		[KSPField(isPersistant = true, guiActive = false)]
		public float engineMultiplier;
		
		public double[] SFDEngineValues()
		{
			double[] array = new double[2] {(double)this.warpDriveDiameter, (double)this.engineMultiplier};
			return array;
		}
	}
}
