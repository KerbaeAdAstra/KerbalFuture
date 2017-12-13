namespace StutterWarp
{
	class WaveGenerator : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float spatiofibrinNeeded;
		
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityNeeded;
	}
}