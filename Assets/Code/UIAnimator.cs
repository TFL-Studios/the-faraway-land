using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController _animatorController;

    private Image _image;
    private SpriteRenderer _bufferSpriteRenderer;

    private void Start()
    {
        this._image = this.GetComponent<Image>();

        GameObject uiAnimatorBuffer = new GameObject($"UIAnimatorBuffer-{this.gameObject.name}");
        this._bufferSpriteRenderer = uiAnimatorBuffer.AddComponent<SpriteRenderer>();
        uiAnimatorBuffer.AddComponent<Animator>().runtimeAnimatorController = this._animatorController;
        uiAnimatorBuffer.transform.position = Camera.main.transform.position - Vector3.forward;
        uiAnimatorBuffer.transform.localScale = Vector3.zero;
        uiAnimatorBuffer.transform.parent = Camera.main.transform;
    }

    private void Update()
    {
        if (this._image == null) return;

        this._image.sprite = this._bufferSpriteRenderer.sprite;
    }
}
