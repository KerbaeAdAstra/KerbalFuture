/*
The following code is provided courtesy of Allis Tauri

The MIT License (MIT)

Copyright (c) 2016 Allis Tauri

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*******************************************************************************

Redistributed by the Kerbae ad Astra group in the project Kerbal Future

Copyright (c) 2017 the Kerbae ad Astra group

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the “Software”), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

namespace KerbalFuture.Utils
{
	public static class BoundsUtils
	{
		public static Vector3[] BoundCorners(Bounds b)
		{
			Vector3[] corners = new Vector3[8];
			Vector3 min = b.min;
			Vector3 max = b.max;
			corners[0] = new Vector3(min.x, min.y, min.z); //left-bottom-back
			corners[1] = new Vector3(min.x, min.y, max.z); //left-bottom-front
			corners[2] = new Vector3(min.x, max.y, min.z); //left-top-back
			corners[3] = new Vector3(min.x, max.y, max.z); //left-top-front
			corners[4] = new Vector3(max.x, min.y, min.z); //right-bottom-back
			corners[5] = new Vector3(max.x, min.y, max.z); //right-bottom-front
			corners[6] = new Vector3(max.x, max.y, min.z); //right-top-back
			corners[7] = new Vector3(max.x, max.y, max.z); //right-top-front
			return corners;
		}
		static Bounds Bounds(this Part p, Transform refT, ref Bounds b, ref bool inited)
		{
			Quaternion part_rot = p.partTransform.rotation;
			p.partTransform.rotation = Quaternion.identity;
			foreach (Renderer rend in p.FindModelComponents<Renderer>())
			{
				if (rend.gameObject == null
					   || !(rend is MeshRenderer || rend is SkinnedMeshRenderer))
				{
					continue;
				}
				Vector3[] verts = BoundCorners(rend.bounds);
				for (int j = 0, len = verts.Length; j < len; j++)
				{
					Vector3 v = p.partTransform.position + part_rot * (verts[j] - p.partTransform.position);
					if (refT != null)
					{
						v = refT.InverseTransformPoint(v);
					}
					if (inited)
					{
						b.Encapsulate(v);
					}
					else
					{
						b.center = v;
						inited = true;
					}
				}
			}
			p.partTransform.rotation = part_rot;
			return b;
		}
		public static Bounds Bounds(this IShipconstruct vessel, Transform refT = null)
		{
			//update physical bounds
			Bounds b = new Bounds();
			bool inited = false;
			System.Collections.Generic.List<Part> parts = vessel.Parts;
			for (int i = 0, partsCount = parts.Count; i < partsCount; i++)
			{
				Part p = parts[i];
				if (p != null)
				{
					p.Bounds(refT, ref b, ref inited);
				}
			}
			return b;
		}
	}
}