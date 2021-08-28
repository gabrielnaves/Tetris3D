using System.Collections.Generic;
using UnityEngine;

public class AdjustChildrenPosWithDimensions : MonoBehaviour {

    readonly List<Pair<Transform, Vector3>> originalChildPosInfo = new List<Pair<Transform, Vector3>>();

    void Start() {
        GetInfo();
        UpdatePositions();
    }

    void GetInfo() {
        foreach (Transform child in transform)
            originalChildPosInfo.Add(new Pair<Transform, Vector3>(child, child.position));
    }

    void UpdatePositions() {
        var widthRatio = ScreenDimensions.instance.widthRatio;
        foreach (var child in originalChildPosInfo) {
            var pos = child.second;
            if (widthRatio > 1)
                pos.x *= widthRatio;
            child.first.position = pos;
        }
    }

#if UNITY_EDITOR
    void Update() {
        UpdatePositions();
    }
#endif
}
