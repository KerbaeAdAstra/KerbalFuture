namespace StutterWarp
{
	class StutterWarpWaveGenerator : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float spatiofibrinNeeded;
		
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityNeeded;
	}
}