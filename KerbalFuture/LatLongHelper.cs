using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KerbalFuture;

namespace KerbalFuture
{
	class LatLongHelper : MonoBehaviour
	{
		public static double XFromLatLongAlt(double lat, double _long, double alt)
		{
			return alt * Math.Cos(lat) * Math.Cos(_long);
		}
		public static double YFromLatLongAlt(double lat, double _long, double alt)
		{
			return alt * Math.Cos(lat) * Math.Sin(_long);
		}
		public static double ZFromLatLongAlt(dobule lat, double alt)
		{
			return alt * Math.Cos(lat);
		}
	}
}