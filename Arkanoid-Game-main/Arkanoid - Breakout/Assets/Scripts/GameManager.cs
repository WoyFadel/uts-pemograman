using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int score;
    public int lives;
    public Text scoreText;
    public Text livesText;
    public bool gameOver;
    public GameObject gameOverPanel;
    public GameObject loadLevelPanel;
    public GameObject mainMenuPanel;
    public int numberOfBricks;
    public Transform[] levels;
    Transform newLevel;
    public int currentLevelIndex = 0;
    public Ball ball;
    public Paddle paddle;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = true;
        MainMenu();
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;   
        numberOfBricks = GameObject.FindGameObjectsWithTag ("brick").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfBricks <= 0)
        {
            Destroy (GameObject.Find ("ExtraLife(Clone)"));   
            Destroy (GameObject.Find ("Enlarge(Clone)"));
            Destroy (GameObject.Find ("Reduce(Clone)"));
            Destroy (GameObject.Find ("Catch(Clone)"));
        }
    }

    public void UpdateLives (int changeInLives)
    {
        lives += changeInLives;

        if (lives <=0)
        {
            lives = 0;
            GameOver();
        }

        livesText.text = "Lives: " + lives;
    }

    public void UpdateScore (int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void UpdateNumberOfBricks()
    {
        numberOfBricks--;

        if (numberOfBricks <= 0)
        {
            Destroy (newLevel.gameObject);

            if (currentLevelIndex >= levels.Length)
            {
                GameOver();
            }
            else
            {
                loadLevelPanel.SetActive (true);
                loadLevelPanel.GetComponentInChildren<Text>().text = "LEVEL " + (currentLevelIndex + 1);
                gameOver = true;
            }
        }
    }

    public void MainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void Play()
    {
        gameOver = true;
        currentLevelIndex = 0;
        lives = 3;
        livesText.text = "Lives: " + lives;
        score = 0;
        scoreText.text = "Score: " + score;
        mainMenuPanel.SetActive (false);
        loadLevelPanel.SetActive (true);
        loadLevelPanel.GetComponentInChildren<Text>().text = "LEVEL  " + (currentLevelIndex + 1);
        Invoke ("LoadLevel", 3f);
    }

    public void LoadLevel ()
    {
        loadLevelPanel.SetActive (false);
        ball.speed = 500f;
        paddle.paddleChangeSize = 0.7f;
        newLevel = Instantiate (levels [currentLevelIndex], Vector2.zero, Quaternion.identity);
        currentLevelIndex++;
        paddle.transform.localScale = new Vector2 (0.7f, 1.5f); 
        numberOfBricks = GameObject.FindGameObjectsWithTag ("brick").Length;
        ball.extraLifeLimit = false;
        ball.slowLimit = false;
        ball.catchLimit = false;
        gameOver = false;
    }

    void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive (true);
    }

    public void PlayAgain()
    {
        Destroy (newLevel.gameObject);  
        currentLevelIndex = 0;
        lives = 3;
        livesText.text = "Lives: " + lives;
        score = 0;
        scoreText.text = "Score: " + score;
        gameOver = true;
        gameOverPanel.SetActive(false);
        loadLevelPanel.SetActive (true);
        loadLevelPanel.GetComponentInChildren<Text>().text = "LEVEL  " + (currentLevelIndex + 1);
        Invoke ("LoadLevel", 3f);
    }

    public void Quit()
    {
        SceneManager.LoadScene ("Arkanoid"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
