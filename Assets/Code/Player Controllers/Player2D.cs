using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Progress;

public class Player2D : MonoBehaviour
{
    [SerializeField] private bool isHolding;
     private bool isMoving;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Vector3 hitBoxUp;
    [SerializeField] private Vector3 hitBoxDown;
    [SerializeField] private Vector3 hitBoxLeft;
    [SerializeField] private Vector3 hitBoxRight;
    [SerializeField] private Transform hitBox;
    public List<GameObject> interactables;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputHandler.Instance.EntradaInteracao.FoiPressionada)
        {

            Debug.Log(interactables[0].name);

        }

        if (!this.isMoving) {
            
            System.Func<KeyCode, bool> inputFunction;
            if (this.isHolding)
            {
                inputFunction = Input.GetKeyDown;
                
            }
            else
            {
                inputFunction = Input.GetKey;
            }

            if (inputFunction(KeyCode.W))
            {
                StartCoroutine(Move(Vector2.up));
                this.hitBox.position = this.transform.position + this.hitBoxUp;
                
            } else if (inputFunction(KeyCode.S))
            {
                StartCoroutine(Move(Vector2.down));
                this.hitBox.position = this.transform.position + this.hitBoxDown;
            }
            else if (inputFunction(KeyCode.A))
            {
                StartCoroutine(Move(Vector2.left));
                this.hitBox.position = this.transform.position + this.hitBoxLeft;
            }
            else if (inputFunction(KeyCode.D))
            {
                StartCoroutine(Move(Vector2.right));
                this.hitBox.position = this.transform.position + this.hitBoxRight;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Interactable")) 
        {
            interactables.Add(collision.gameObject);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactables.Remove(collision.gameObject);
    }


    private System.Collections.IEnumerator Move(Vector2 direction)
    {
        this.isMoving = true;
        Vector2 startPosition = this.transform.position;
        Vector2 endPosition = startPosition + (direction * this.gridSize);
        float elapsedTime = 0f;
        while (elapsedTime < this.moveDuration)
        {
            elapsedTime += Time.deltaTime;
            this.transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / this.moveDuration);
            
            yield return null;
        }
        this.transform.position = endPosition;
        this.isMoving = false;
    }

}
