using UnityEngine;
using TMPro;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void LateUpdate()
    {
        text.text = $"Score: {GameScore.instance.score}";
    }
}
