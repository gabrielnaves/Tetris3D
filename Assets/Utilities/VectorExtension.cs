using UnityEngine;

static public class VectorExtension {

    static public bool Approx(this Vector3 a, Vector3 b) {
        return (a - b).sqrMagnitude < 2f / 16f;
    }
}
