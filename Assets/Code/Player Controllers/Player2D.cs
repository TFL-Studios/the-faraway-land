using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    [SerializeField] private bool _isHolding;
     private bool _isMoving;
    [SerializeField] private float _moveDuration = 0.2f;
    [SerializeField] private float _gridSize = 1f;
    [SerializeField] private Vector3 _hitBoxUp;
    [SerializeField] private Vector3 _hitBoxDown;
    [SerializeField] private Vector3 _hitBoxLeft;
    [SerializeField] private Vector3 _hitBoxRight;
    [SerializeField] private Transform _hitBox;
    [SerializeField] private List<GameObject> _interactables;

    private void Update()
    {
        if (InputHandler.Instance.InteractionInput.WasPressed)
        {
            Debug.Log(this._interactables[0].name);
        }

        if (!this._isMoving) {
            
            System.Func<KeyCode, bool> inputFunction;
            if (this._isHolding)
            {
                inputFunction = Input.GetKeyDown;
            }
            else
            {
                inputFunction = Input.GetKey;
            }

            if (inputFunction(KeyCode.W))
            {
                this.StartCoroutine(this.Move(Vector2.up));
                this._hitBox.position = this.transform.position + this._hitBoxUp;
                
            } else if (inputFunction(KeyCode.S))
            {
                this.StartCoroutine(this.Move(Vector2.down));
                this._hitBox.position = this.transform.position + this._hitBoxDown;
            }
            else if (inputFunction(KeyCode.A))
            {
                this.StartCoroutine(this.Move(Vector2.left));
                this._hitBox.position = this.transform.position + this._hitBoxLeft;
            }
            else if (inputFunction(KeyCode.D))
            {
                this.StartCoroutine(this.Move(Vector2.right));
                this._hitBox.position = this.transform.position + this._hitBoxRight;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable")) 
        {
            this._interactables.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this._interactables.Remove(collision.gameObject);
    }

    private IEnumerator Move(Vector2 direction)
    {
        this._isMoving = true;
        Vector2 startPosition = this.transform.position;
        Vector2 endPosition = startPosition + (direction * this._gridSize);
        float elapsedTime = 0f;
        while (elapsedTime < this._moveDuration)
        {
            elapsedTime += Time.deltaTime;
            this.transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / this._moveDuration);
            
            yield return null;
        }
        this.transform.position = endPosition;
        this._isMoving = false;
    }
}
