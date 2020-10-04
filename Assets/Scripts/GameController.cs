using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Control game state and events
    /// </summary>

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Ball bounce object")]
    public float initialBallSpeed = 1f;
    public GameObject ball;

    [Header("Pause menu")]
    public GameObject pauseMenu;

    private int score = 0;
    private static bool applicationIsQuitting = false;
    private Animation scoreAnim;

    // Singleton instance
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }

    private void OnEnable()
    {
        RefreshScore();
        applicationIsQuitting = false;
    }

    private void Start()
    {
        scoreAnim = scoreText.gameObject.GetComponent<Animation>();
    }

    // Initialize ball bounce to initial speed
    public void initBall()
    {
        ball = Instantiate(ball, Vector3.zero, Quaternion.identity);
        ball.GetComponent<Bounce>().InitialVelocity = new Vector2(Random.Range(0f, 1f), Random.Range(0, 1f)).normalized * initialBallSpeed;
    }

    // Add point to score and UI
    public void AddPoint()
    {
        score++;
        scoreAnim.Play();
        RefreshScore();
    }

    // Game is done
    public void GameOver()
    {
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
    }

    // Refresh UI text score
    private void RefreshScore()
    {
        if (scoreText)
            scoreText.text = score.ToString();
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
