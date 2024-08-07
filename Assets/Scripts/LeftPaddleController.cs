using UnityEngine;

public class LeftPaddleController : MonoBehaviour
{
    [SerializeField] float paddleWidth = 4f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject tutorial;
    [SerializeField] Animator animator;  

    private bool isMovingX = false;
    private Vector3 originalScale;

    void Start()
    {
        if (tutorial != null)
        {
            tutorial.SetActive(true);
        }

        originalScale = transform.localScale;
    }

    void Update()
    {
        if (tutorial != null && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            tutorial.SetActive(false);
        }

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, -paddleWidth, 0);
        transform.position = newPosition;

        if (moveX != 0)
        {
            isMovingX = true;
            animator.SetBool("IsMoveX", true);
            if (moveX < 0)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);  
            }
            else if (moveX > 0)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);  
            }
        }
        else
        {
            isMovingX = false;
            animator.SetBool("IsMoveX", false);
        }
    }
}
