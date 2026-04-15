using UnityEngine;
using UnityEngine.InputSystem;

public class Player2D : MonoBehaviour
{
    [SerializeField] private bool isHolding;
     private bool isMoving;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float gridSize = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(!isMoving) {
            
            System.Func<KeyCode, bool> inputFunction;
            if (isHolding)
            {
                inputFunction = Input.GetKey;
            }
            else
            {
                inputFunction = Input.GetKeyDown;
            }

            if (inputFunction(KeyCode.W))
            {
                StartCoroutine(Move(Vector2.up));
            } else if (inputFunction(KeyCode.S))
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (inputFunction(KeyCode.A))
            {
                StartCoroutine(Move(Vector2.left));
            }
            else if (inputFunction(KeyCode.D))
            {
                StartCoroutine(Move(Vector2.right));
            }

        }
    }
    private System.Collections.IEnumerator Move(Vector2 direction)
    {
        isMoving = true;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            
            yield return null;
        }
        transform.position = endPosition;
        isMoving = false;
    }

}
