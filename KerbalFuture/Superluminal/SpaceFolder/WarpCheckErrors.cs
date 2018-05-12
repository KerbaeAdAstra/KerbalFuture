using System;

namespace KerbalFuture.Superluminal.SpaceFolder
{
	[Flags]
    public enum Error { ClearForWarp = 0, DrivesNotFound = 1, VesselTooLarge = 2, InsufficientResources = 4 }
}