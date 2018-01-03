namespace StutterWarp
{
	class WaveGenerator : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float spatiofibrinNeeded;
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityNeeded;
		[KSPField(isPersistant = true, guiActive = false)]
		public float maxWavelength;
		[KSPField(isPersistant = true, guiActive = false)]
		public float minWaveLength;
		[KSPField(isPersistant = true, guiActive = false)]
		public float maxAmplitude;
		[KSPField(isPersistant = true, guiActive = false)]
		public float minAmplitude;
	}
}