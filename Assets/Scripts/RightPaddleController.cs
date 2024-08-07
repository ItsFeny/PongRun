using UnityEngine;

public class RightPaddleController : MonoBehaviour
{
    public GameObject ball;
    public float speed; 

    void Update()
    {
        Vector3 ballPosition = ball.transform.position;
        Vector3 paddlePosition = transform.position;

        paddlePosition.y = Mathf.MoveTowards(paddlePosition.y, ballPosition.y, speed * Time.deltaTime);
        transform.position = paddlePosition;
    }
}
