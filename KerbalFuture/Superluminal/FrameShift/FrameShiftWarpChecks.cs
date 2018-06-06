using System.Collections.Generic;
using System;
using KerbalFuture.Utils;

/********************************************************\
 * EQUATIONS
 * _______________________________________________________
 * Field Energy Equation
 * U_F = k_A * r^4 + k_c * v^2
 * -------------------------------------------------------
 * U_F is field energy in Joules		** 1 EC is 1 kJ **
 * k_A is Alcubierre's constant
 * r is radius of the warp bubble
 * k_c is the Spacetime Contraction Constant
 * v is the velocity of the spacecraft/warp bubble
 * ________________________________________________________
 * Drag Energy Equation
 * dU_F/dt = k_d * v^3 * r^3
 * -------------------------------------------------------
 * dU_F is the change in energy from the field
 * dt is the change in time
 * k_d is the Hyperspace Drag Constant
 * v is the velocity of the spacecraft/warp bubble
 * r is the radius of the warp bubble
 * ________________________________________________________
\*********************************************************/
namespace KerbalFuture.Superluminal.FrameShift
{
	public class FrameShiftWarpChecks
	{
		public const double ALCUBIERRE_CONSTANT_OF_SPACETIME = 2000.0;
		public const double CONTRACTION_CONSTANT_OF_SPACETIME = 25.0;
		public const double HYPERSPACE_DRAG_CONSTANT = 80.0;
		public const double DRIVE_USAGE_CONSTANT_OF_XM = 400.0;
		public static Error WarpAvalible(Vessel v, double velocity, out VesselResourceSimulation outVRS)
		{
			Error retErr = Error.ClearForWarp;
			//Creates and fills in one line! Marvelous!
			List<FrameShiftDriveData> dd = new List<FrameShiftDriveData>(FSWarpHelp.DriveDataList(v));
			//No drives
			if(dd.Count == 0)
			{
				retErr = retErr | Error.DrivesNotFound;
				outVRS = new VesselResourceSimulation();
				//We'll return after this because you can't check anything else without drives
				return retErr;
			}
			//Ship size calc
			double radius = WarpHelp.VesselDiameterCalc(v) / 2;
			//Ship is too big for drives
			if (MaxWarpHoleSize((IEnumerable<Part>)dd, velocity) > radius)
			{
				retErr = retErr | Error.VesselTooLarge;
			}
			//Res calc
			double U_F = FieldEnergyCalc(velocity, radius);
			//Insufficient resources on ship
			VesselResourceSimulation vrs = new VesselResourceSimulation(v, velocity, true);
			outVRS = vrs;
			if(vrs.Status == SimulationStatus.Failed)
			{
				retErr = retErr | Error.InsufficientResources;
			}
			return retErr;
		}
		public static double MaxWarpHoleSize(IEnumerable<Part> engines, double velocity)
		{
			double U_F_max = MaxFieldEnergy(engines);
			return Math.Pow(((U_F_max - (CONTRACTION_CONSTANT_OF_SPACETIME * Math.Pow(velocity, 2))) / ALCUBIERRE_CONSTANT_OF_SPACETIME), 1 / 4);
		}
		public static double MaxFieldEnergy(IEnumerable<Part> engines)
		{
			double retVal = 0;
			foreach(Part p in engines)
			{
				retVal += FSWarpHelp.FSDModuleFromPart(p).capacity;
			}
			return retVal;
		}
		public static double FieldEnergyCalc(double velocity, double radius) 
			=> ALCUBIERRE_CONSTANT_OF_SPACETIME * Math.Pow(radius, 4) + CONTRACTION_CONSTANT_OF_SPACETIME * Math.Pow(velocity, 2);
	}
}