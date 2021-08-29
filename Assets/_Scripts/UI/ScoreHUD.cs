using UnityEngine;
using TMPro;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private string prefix = "Score: ";
    [SerializeField] private TextMeshProUGUI text;

    private void LateUpdate()
    {
        text.text = $"{prefix}{GameScore.instance.score}";
    }
}
