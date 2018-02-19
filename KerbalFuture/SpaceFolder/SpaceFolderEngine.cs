namespace KerbalFuture.SpaceFolder
{
	internal class SpaceFolderEngine : PartModule
	{
		[KSPField]
		public float warpDriveDiameter;
		[KSPField]
		public float engineMultiplier = 1;
		[KSPField]
		public string mainResource = "ElectricCharge";
		[KSPField]
		public string catalyst = "Spatiofibrin";
		
		public double[] SFDEngineValues() => new double[] {warpDriveDiameter, engineMultiplier};
	}
}
