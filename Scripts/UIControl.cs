using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Needed for restarting the scene

public class UIControl : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreLabel;
    [SerializeField] SettingsPopup settingsPopup;

    // End-game panels for win and game over
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject youWinPanel;

    private int score;

    private void OnEnemyHit()
    {
        score++;
        ScoreLabel.text = score.ToString();
    }

    private void OnEnable()
    {
        // Subscribe to the enemy hit event
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    public void Start()
    {
        score = 0;
        ScoreLabel.text = score.ToString();
        settingsPopup.Close();

        // Hide end-game panels at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (youWinPanel != null)
            youWinPanel.SetActive(false);
    }

    void Update()
    {
        // No pause/resume functionality is included in this version.
    }

    public void OnOpenSettings()
    {
        // Simply open the settings popup without pausing the game.
        settingsPopup.Open();
    }

    // Call this method when the player loses
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Call this method when the player wins
    public void ShowYouWin()
    {
        if (youWinPanel != null)
        {
            youWinPanel.SetActive(true);
        }
    }

    // Restart the game (simply reloads the scene)
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
