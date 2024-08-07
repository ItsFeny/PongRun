using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Animator animator;
    public bool faceLeft = false;
    public float leftLimit = -4f;
    public float rightLimit = 4f;

    private bool isMovingX = false;
    private Vector3 originalScale;
    private PhotonView photonView;

    void Start()
    {
        originalScale = transform.localScale;
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError("PhotonView component missing on player prefab!");
        }

        // Ajustar la escala inicial según faceLeft
        if (faceLeft)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);
        transform.position = newPosition;

        UpdateAnimation(moveX);
    }

    void UpdateAnimation(float moveX)
    {
        if (moveX != 0)
        {
            isMovingX = true;
            animator.SetBool("IsMoveX", true);

            // Ajustar la escala del sprite según el movimiento
            if (moveX > 0)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
            else if (moveX < 0)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
        }
        else
        {
            isMovingX = false;
            animator.SetBool("IsMoveX", false);

            // Mantener la dirección de la mirada según faceLeft cuando no se mueve
            if (faceLeft)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
            else
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isMovingX);
            stream.SendNext(transform.localScale); // Enviar la escala también
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            isMovingX = (bool)stream.ReceiveNext();
            animator.SetBool("IsMoveX", isMovingX);
            transform.localScale = (Vector3)stream.ReceiveNext(); // Recibir la escala también
        }
    }
}
