using System;

namespace KerbalFuture.Superluminal.FrameShift
{
	[Flags]
	public enum Error { ClearForWarp = 0, DrivesNotFound = 1, VesselTooLarge = 2, InsufficientResources = 4 }
}