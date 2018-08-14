using UnityEngine;

namespace KerbalFuture.Superluminal.FrameShift
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
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
		[KSPField]
		public float xmMultiplier = 0.005f; //Default of 0.5%

		public FrameShiftDriveData PartDriveData => new FrameShiftDriveData(part, capacity, efficiency, mainResource, catalyst, xmMultiplier);
	}
}