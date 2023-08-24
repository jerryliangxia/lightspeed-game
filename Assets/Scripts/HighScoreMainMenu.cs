using UnityEngine;
using TMPro;

public class HighScoreMainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    
    // Start is called before the first frame update
    private void Start()
    {
        var highScore = PlayerPrefs.GetInt(Constants.HighScore, 0);
        if (highScore != 0)
        {
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt(Constants.HighScore, 0);
        }
        else
        {
            highScoreText.text = "";
        }
    }
}