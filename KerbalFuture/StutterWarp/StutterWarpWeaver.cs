namespace StutterWarp
{
	class StutterWarpWaveGenerator : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityConsumption;
		
		[KSPField(isPersistant = true, guiActive = false)]
		public float segfaultMultiplier;
	}
}