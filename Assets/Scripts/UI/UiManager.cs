using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private Text ScoreText;

        public void OnScoreChangeHandler(int score)
        {
            ScoreText.text = score.ToString();
        }
    }
}