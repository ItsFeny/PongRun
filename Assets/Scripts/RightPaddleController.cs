using UnityEngine;

public class RightPaddleController : MonoBehaviour
{
    public GameObject ball;
    public float speed; // Reducida velocidad para hacer la IA m�s f�cil

    void Update()
    {
        // Obtiene la posici�n de la pelota y mueve la paleta hacia ella
        Vector3 ballPosition = ball.transform.position;
        Vector3 paddlePosition = transform.position;

        // Solo sigue la pelota verticalmente
        paddlePosition.y = Mathf.MoveTowards(paddlePosition.y, ballPosition.y, speed * Time.deltaTime);

        // Actualiza la posici�n de la paleta
        transform.position = paddlePosition;
    }
}
