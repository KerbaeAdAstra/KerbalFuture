namespace StutterWarp
{
	class Weaver : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false)]
		public float electricityConsumption;
		[KSPField(isPersistant = true, guiActive = false)]
		public float segfaultMultiplier;
	}
}