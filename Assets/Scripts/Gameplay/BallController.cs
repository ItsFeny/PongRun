using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BallController : MonoBehaviourPunCallbacks
{
    public float initialSpeed = 10f;
    public float speedIncrement = 1f;
    public float maxSpeed = 20f;
    private float currentSpeed;
    private Vector2 direction;
    public GameObject leftPaddle, rightPaddle;
    public int leftScore, rightScore;
    public TMP_Text scoreText;
    public GameObject winnerCanvas;
    public TMP_Text winnerText;
    public TMP_Text pointsText;
    public AudioSource hit, goal;
    private bool gameStarted = false;
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        ResetBall();
        UpdateScoreText();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && gameStarted)
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime);
            photonView.RPC("SyncPosition", RpcTarget.All, transform.position);
        }
        if (!gameStarted && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            gameStarted = true;
        }
    }

    [PunRPC]
    void SyncPosition(Vector3 position)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.position = position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            hit.Play();
            if (collision.gameObject.CompareTag("Wall"))
            {
                direction.y = -direction.y;
            }
            else if (collision.gameObject.CompareTag("SideWall") || collision.gameObject.CompareTag("Paddle"))
            {
                direction.x = -direction.x;
                direction = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * direction;
                currentSpeed = Mathf.Min(currentSpeed + speedIncrement, maxSpeed); 
            }
            photonView.RPC("SyncDirection", RpcTarget.All, direction, currentSpeed);
        }
    }

    [PunRPC]
    void SyncDirection(Vector2 newDirection, float newSpeed)
    {
        direction = newDirection;
        currentSpeed = newSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            goal.Play();
            if (other.CompareTag("LeftGoal"))
            {
                rightScore++;
            }
            else if (other.CompareTag("RightGoal"))
            {
                leftScore++;
            }
            photonView.RPC("UpdateScore", RpcTarget.All, leftScore, rightScore);
            CheckWinner();
            ResetBall();
            gameStarted = false;
        }
    }

    [PunRPC]
    void UpdateScore(int newLeftScore, int newRightScore)
    {
        leftScore = newLeftScore;
        rightScore = newRightScore;
        UpdateScoreText();
    }

    void ResetBall()
    {
        transform.position = Vector2.zero;
        currentSpeed = initialSpeed;
        direction = Random.value > 0.5f ? Vector2.right : Vector2.left;
        direction = Quaternion.Euler(0, 0, Random.Range(-15f, 15f)) * direction;
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncResetBall", RpcTarget.All, transform.position, direction, currentSpeed);
        }
    }

    [PunRPC]
    void SyncResetBall(Vector3 position, Vector2 newDirection, float newSpeed)
    {
        transform.position = position;
        direction = newDirection;
        currentSpeed = newSpeed;
    }

    void UpdateScoreText()
    {
        scoreText.text = $"{leftScore} - {rightScore}";
    }

    void CheckWinner()
    {
        if (leftScore >= 5)
        {
            photonView.RPC("ShowWinner", RpcTarget.All, "Jugador 1", leftScore);
            Time.timeScale = 0f;
        }
        else if (rightScore >= 5)
        {
            photonView.RPC("ShowWinner", RpcTarget.All, "Jugador 2", rightScore);
            Time.timeScale = 0f;
        }
    }

    [PunRPC]
    void ShowWinner(string playerName, int score)
    {
        pointsText.text = $"Puntos: {score}";
        winnerText.text = $"El {playerName} gana la partida";
        winnerCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        photonView.RPC("RestartGameRPC", RpcTarget.All);
    }

    [PunRPC]
    void RestartGameRPC()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        photonView.RPC("QuitGameRPC", RpcTarget.All);
    }

    [PunRPC]
    void QuitGameRPC()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(leftPaddle.name, leftPaddle.transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate("MenuPause", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(rightPaddle.name, rightPaddle.transform.position, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Main Menu");
    }
}
