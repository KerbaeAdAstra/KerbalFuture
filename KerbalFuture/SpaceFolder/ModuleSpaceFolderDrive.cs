namespace KerbalFuture.Superluminal.SpaceFolder
{
	public class ModuleSpaceFolderEngine : PartModule
	{
		//If anything, a part that uses ModuleSpaceFolderEngine needs to define warpDriveDiameter, everything else has default values
		
		//Max diamater that this warp drive alone can encapsulate
		[KSPField]
		public float warpDriveDiameter;
		//Efficiency of engine
		[KSPField]
		public float engineMultiplier = 1;
		//Main resource that is used
		[KSPField]
		public string mainResource = "ElectricCharge";
		//Resource needed that is not used
		[KSPField]
		public string catalyst = "Spatiofibrin";
		
		//Returns a readonly SpaceFolderDriveData with the part info
		public SpaceFolderDriveData PartDriveData
		{
			get
			{
				return SpaceFolderDriveData(this.part
					warpDriveDiameter,
					engineMultiplier,
					mainResource,
					catalyst);
			}
		}
	}
}