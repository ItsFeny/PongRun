using UnityEngine;

public class LeftPaddleController : MonoBehaviour
{
    bool isDragging = false;
    Vector2 startPosition;
    [SerializeField] float paddleHeight = 4f;
    [SerializeField] GameObject tutorial;  

    void Start()
    {
        if (tutorial != null)
        {
            tutorial.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (startPosition.x < 0)
            {
                isDragging = true;
            }

            if (tutorial != null)
            {
                tutorial.SetActive(false);
            }
        }

        if (isDragging)
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float clampedY = Mathf.Clamp(currentPosition.y, -paddleHeight, paddleHeight);
            transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
