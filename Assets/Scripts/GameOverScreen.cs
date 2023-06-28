using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Singleton instance
    public TextMeshProUGUI pointsText;

    public void Setup(int level)
    {
        gameObject.SetActive(true);
        pointsText.text = "Score: " + level;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
