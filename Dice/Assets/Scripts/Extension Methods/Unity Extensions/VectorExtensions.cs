using UnityEngine;
using System.Collections;

public static partial class VectorExtensions {

	public static Vector3 Add (this Vector3 v1, Vector2 v2) {

		return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z);

	}
}
