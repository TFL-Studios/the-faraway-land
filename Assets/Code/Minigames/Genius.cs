using UnityEngine;

public class Genius : Interactable
{
    [SerializeField] private GameObject _minigameUIPrefab;
    private GameObject _minigameUIInstance;

    private void Update()
    {
        if (this._minigameUIInstance && InputHandler.Instance.EntradaLanterna.FoiPressionada)
        {
            GameObject.Destroy(this._minigameUIInstance);
        }
    }

    public override bool Interact()
    {
        if (this._minigameUIInstance) return false;
        Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
        this._minigameUIInstance = GameObject.Instantiate(this._minigameUIPrefab, canvas.transform);
        return true;
    }
}
