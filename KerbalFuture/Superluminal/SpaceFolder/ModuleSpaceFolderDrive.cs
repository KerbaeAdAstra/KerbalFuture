namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class ModuleSpaceFolderDrive : PartModule
	{
		// If anything, a part that uses ModuleSpaceFolderDrive needs to define warpDriveDiameter
		// Everything else has default values

		// Max diamater that this warp drive alone can encapsulate
		[KSPField(guiActiveEditor = true, guiName = "Warpable diameter")]
		public float warpDriveDiameter;
		// Efficiency of engine
		[KSPField]
		public float engineMultiplier = 1;
		// Main resource that is used
		[KSPField(guiActiveEditor = true, guiName = "Main warp resource")]
		public string mainResource = "ElectricCharge";
		// Resource needed that is not used
		[KSPField(guiActiveEditor = true, guiName = "Warp catalyst")]
        public string catalyst = "Spatiofibrin";
		
		// Returns a readonly SpaceFolderDriveData with the part info
		public SpaceFolderDriveData PartDriveData => new SpaceFolderDriveData(part, warpDriveDiameter, engineMultiplier,
			mainResource, catalyst);
	}
}