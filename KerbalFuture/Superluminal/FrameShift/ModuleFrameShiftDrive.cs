namespace KerbalFuture.Superluminal.FrameShift
{
	public class ModuleFrameShiftDrive : PartModule
	{
		[KSPField(guiActiveEditor = true, guiName = "Drive Capacity")]
		public float capacity;
		[KSPField]
		public string mainResource = "ElectricCharge";
		[KSPField]
		public string catalyst = "ExoticMatter";
		[KSPField]
		public float efficiency = 0.2f;

		public FrameShiftDriveData PartDriveData => new FrameShiftDriveData(part, capacity, efficiency, mainResource, catalyst);
	}
}