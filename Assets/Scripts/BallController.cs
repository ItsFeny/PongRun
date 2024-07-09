using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedIncrement = 1f;
    private float currentSpeed;
    private Vector2 direction;
    public GameObject leftPaddle, rightPaddle;
    public int leftScore, rightScore;
    public TMP_Text scoreText;
    public GameObject gameOverCanvas;
    public TMP_Text gameOverText;

    private bool gameStarted = false;  // Nueva bandera para verificar si el juego ha comenzado

    void Start()
    {
        ResetBall();
        UpdateScoreText();
    }

    void Update()
    {
        // Inicia el movimiento de la pelota solo cuando el juego ha comenzado
        if (gameStarted)
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime);
        }

        // Verifica si se ha tocado la pantalla para iniciar el juego
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction.y = -direction.y;
        }
        else if (collision.gameObject.CompareTag("SideWall"))
        {
            direction.x = -direction.x;
            direction = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * direction;
            currentSpeed += speedIncrement;
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            direction.x = -direction.x;
            direction = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * direction;
            currentSpeed += speedIncrement;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LeftGoal"))
        {
            rightScore++;
            CheckGameOver();
            ResetBall();
            UpdateScoreText();
            gameStarted = false;  // Reinicia el juego cuando se anota un gol
        }
        else if (other.CompareTag("RightGoal"))
        {
            leftScore++;
            CheckGameOver();
            ResetBall();
            UpdateScoreText();
            gameStarted = false;  // Reinicia el juego cuando se anota un gol
        }
    }

    void ResetBall()
    {
        transform.position = Vector2.zero;
        currentSpeed = initialSpeed;
        direction = Random.value > 0.5f ? Vector2.right : Vector2.left;
        direction = Quaternion.Euler(0, 0, Random.Range(-15f, 15f)) * direction;
    }

    void UpdateScoreText()
    {
        scoreText.text = $"{leftScore} - {rightScore}";
    }

    void CheckGameOver()
    {
        if (leftScore >= 5)
        {
            ShowGameOver("Ganaste:", leftScore);
            Time.timeScale = 0f;
        }
        else if (rightScore >= 5)
        {
            ShowGameOver("Perdiste:", leftScore);
            Time.timeScale = 0f;
        }
    }

    void ShowGameOver(string message, int score)
    {
        gameOverText.text = $"{message} {score} puntos";
        gameOverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu Futbi");
        Time.timeScale = 1f;
    }
}
