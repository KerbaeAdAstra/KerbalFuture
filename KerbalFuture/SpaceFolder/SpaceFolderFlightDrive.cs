using System;
using System.Tuples;
using Hlpr;
//using KFGUI; TODO

namespace SpaceFolder
{
	class FlightDrive : PartModule
	{
		Vector3d cbPos;
		double vesHeight;
		CelestialBody vesBody;
		CelestialBody warpBody;
		double warpLong, warpLat;
		double bodyGravPot;
		SpaceFolderWarpChecks insWarpChecks = new SpaceFolderWarpChecks();
		
		internal void WarpVessel(List<Tuple(Part, double, double)> partValueList)//the tuple is a 3 item dictionary. Here it is representing a list of parts and two doubles. The doubles are, in order, electricity and spatiofibrin usage
		{
			//It is a list of Tuples where, the Part is the warp drive that is requesting the fuel
			//The two doubles are the fuel amounts requested
			double cbx = 0, cby = 0, cbz = 0;
			vesBody = this.vessel.mainBody;
//TODO
			//warpBody = KFGUI.ChosenBody();
			//warpLong = KFGUI.ChosenLong();
			//warpLat = KFGUI.ChosenLat();
			bodyGravPot = CalculateGravPot(vesBody, this.vessel);
			cbPos = warpBody.position;
			Vector3dHelper VesPosition = new Vector3dHelper();
			LatLongHelper LLH = new LatLongHelper();
			Vector3dHelper.ConvertVector3dToXYZCoords(cbPos, ref cbx, 
				ref cby, ref cbz);
			VesPosition.SetX(cbx + LLH.XFromLatLongAlt(warpLat, warpLong, 
				bodyGravPot));
			VesPosition.SetY(cby + LLH.YFromLatLongAlt(warpLat, warpLong, 
			   bodyGravPot));
			VesPosition.SetZ(cbz + LLH.ZFromLatAlt(warpLat,  bodyGravPot));
			for(int i = 0; i < partValueList.Count; i++)
			{
				UseElectricity(partValueList[i].item1, partValueList[i].item2);
			}
			this.vessel.SetPosition(Vector3dHelper.ConvertXYZCoordsToVector3d(
				VesPosition.Vector3dX(), VesPosition.Vector3dY(),
				VesPosition.Vector3dZ()), true);
		}
		double CalculateGravPot(CelestialBody cb, Vessel v)
		{
			LatLongHelper CGPLLH = new LatLongHelper();
			return cb.gravParameter / CGPLLH.GetVesselAltitude(true, v);
		}
		void UseElectricity(Part part, double amount)
		{
			part.RequestResource("Electricity", amount);
		}
	}
}