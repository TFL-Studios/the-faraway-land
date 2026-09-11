using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    [SerializeField] private Image _menu;
    [SerializeField] private Image _inv;
    [SerializeField] private Image _diario;
    [SerializeField] private TextMeshProUGUI _log;

    private bool _toggleSelecao = false;
    private bool _toggleLanterna = false;
    private bool _togglePOV = false;

    private readonly string _ON = "<color=#00a000>ON</color>";
    private readonly string _OFF = "<color=#a00000>OFF</color>";
    private readonly string _PRESSED = "<color=#a0a000>PRESSED </color>";
    private readonly string _RELEASED = "<color=#ffff00>RELEASED</color>";

    private void Update()
    {
        if (InputHandler.Instance.PauseMenuInput.WasPressed) { this._menu.gameObject.SetActive(!this._menu.gameObject.activeSelf); }
        if (InputHandler.Instance.InventoryInput.WasPressed) { this._inv.gameObject.SetActive(!this._inv.gameObject.activeSelf); } 
        if (InputHandler.Instance.JournalInput.WasPressed) { this._diario.gameObject.SetActive(!this._diario.gameObject.activeSelf); }

        if (InputHandler.Instance.SelectionInput.WasPressed) { this._toggleSelecao = !this._toggleSelecao; }
        if (InputHandler.Instance.FlashlightInput.WasPressed) { this._toggleLanterna = !this._toggleLanterna; }
        if (InputHandler.Instance.PointOfViewInput.WasPressed) { this._togglePOV = !this._togglePOV; }

        this._log.text = string.Concat
        (
            "Interface\n",
            "(ESC) Menu\n",
            "(TAB) Inventario\n",
            "(Q) Diario\n",
            $"(E) Selecionar: {(InputHandler.Instance.SelectionInput.IsPressed ? _PRESSED : _RELEASED)} ({(this._toggleSelecao ? _ON : _OFF)})\n",
            "Jogador\n",
            $"(WASD) Direcao Movimento: {InputHandler.Instance.MovementInput.Value}\n",
            $"(L SHIFT) Corrida: {(InputHandler.Instance.SprintInput.IsPressed ? _ON : _OFF)}\n",
            $"(L CONTROL) Agachar: {(InputHandler.Instance.CrouchInput.IsPressed ? _ON : _OFF)}\n",
            $"(E) Interagir: {(InputHandler.Instance.InteractionInput.IsPressed ? _PRESSED : _RELEASED)}\n",
            $"(F) Lanterna: {(InputHandler.Instance.FlashlightInput.IsPressed ? _PRESSED : _RELEASED)} ({(this._toggleLanterna ? _ON : _OFF)})\n",
            $"(MOUSE) Camera Delta: {InputHandler.Instance.CameraInput.Value}\n",
            $"(L ALT) POV: {(InputHandler.Instance.PointOfViewInput.IsPressed ? _PRESSED : _RELEASED)} ({(this._togglePOV ? _ON : _OFF)})\n"
        );
    }
}
