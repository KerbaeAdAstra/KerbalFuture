using UnityEngine;
using System;

namespace KerbalFuture.Utils
{
	//Math from the resources at http://euclideanspace.com/maths/algebra/realNormedAlgebra/quaternions/index.htm
	public class Direction
	{
		private Vector3d euler; // represents the euler portion of the quaternion
		private Quaternion rotation; // quaternion representing rotation of the vector
		private Vector3d vector; // the actual vector
		
		// Constructors
		public Direction(Quaternion q)
		{
			rotation = q;
			euler = q.eulerAngles;
			vector = rotation * Vector3d.forward;
		}
		public Direction(Vector3d v3d, bool isEuler)
		{
			if (isEuler)
			{
				Euler = v3d;
			}
			else
			{
				Vector = v3d;
			}
		}
		//Properties
		public Vector3d Vector
		{
			get
			{
				return vector;
			}
			set
			{
				vector = value.normalized;
				rotation = Quaternion.LookRotation(value);
				euler = rotation.eulerAngles;
			}
		}
		public Vector3d Euler
		{
			get
			{
				return euler;
			}
			set
			{
				euler = value;
				rotation = Quaternion.Euler(value);
				vector = rotation * Vector3d.forward;
			}
		}
		public Quaternion Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
				euler = value.eulerAngles;
				vector = rotation * Vector3d.forward;
			}
		}
		//Overridden methods
		public override bool Equals(object obj)
		{
			Type compareType = typeof(Direction);
			if (compareType.IsInstanceOfType(obj))
			{
				Direction d = obj as Direction;
				return rotation.Equals(d.rotation);
			}
			return false;
		}
		public override int GetHashCode()
		{
			return rotation.GetHashCode();
		}
	}
}