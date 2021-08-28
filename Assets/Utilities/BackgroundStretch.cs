using UnityEngine;

public class BackgroundStretch : MonoBehaviour {

    Vector3 originalScale;

	void Start () {
        originalScale = transform.localScale;
        CalculateStretching();
	}

#if UNITY_EDITOR
    void Update () {
        CalculateStretching();
	}
#endif

    void CalculateStretching() {
        if (ScreenDimensions.instance.currentWidth > ScreenDimensions.instance.originalWidth) {
            transform.localScale = new Vector3(originalScale.x * ScreenDimensions.instance.widthRatio,
                                               originalScale.y, originalScale.z);
        }
        else {
            transform.localScale = originalScale;
        }
    }
}
