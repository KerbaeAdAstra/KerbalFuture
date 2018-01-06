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
		public float minWavelength;
		[KSPField(isPersistant = true, guiActive = false)]
		public float maxAmplitude;
		[KSPField(isPersistant = true, guiActive = false)]
		public float minAmplitude;
		
		public float[] ValueList()
		{
			float[] array = new float[6] {this.spatiofibrinNeeded, this.electricityNeeded, this.maxWavelength, this.minWavelength, this.maxAmplitude, this.minAmplitude};
			return array;
		}
	}
}