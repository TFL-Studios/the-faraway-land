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
        if (InputHandler.Instance.EntradaMenu.FoiPressionada) { this._menu.gameObject.SetActive(!this._menu.gameObject.activeSelf); }
        if (InputHandler.Instance.EntradaInventario.FoiPressionada) { this._inv.gameObject.SetActive(!this._inv.gameObject.activeSelf); } 
        if (InputHandler.Instance.EntradaDiario.FoiPressionada) { this._diario.gameObject.SetActive(!this._diario.gameObject.activeSelf); }

        if (InputHandler.Instance.EntradaSelecao.FoiPressionada) { this._toggleSelecao = !this._toggleSelecao; }
        if (InputHandler.Instance.EntradaLanterna.FoiPressionada) { this._toggleLanterna = !this._toggleLanterna; }
        if (InputHandler.Instance.EntradaPOV.FoiPressionada) { this._togglePOV = !this._togglePOV; }

        this._log.text = string.Concat
        (
            "Interface\n",
            "(ESC) Menu\n",
            "(TAB) Inventario\n",
            "(Q) Diario\n",
            $"(E) Selecionar: {(InputHandler.Instance.EntradaSelecao.EstaPressionada ? _PRESSED : _RELEASED)} ({(this._toggleSelecao ? _ON : _OFF)})\n",
            "Jogador\n",
            $"(WASD) Direcao Movimento: {InputHandler.Instance.EntradaMovimento.Valor}\n",
            $"(L SHIFT) Corrida: {(InputHandler.Instance.EntradaCorrida.EstaPressionada ? _ON : _OFF)}\n",
            $"(L CONTROL) Agachar: {(InputHandler.Instance.EntradaAgachamento.EstaPressionada ? _ON : _OFF)}\n",
            $"(E) Interagir: {(InputHandler.Instance.EntradaInteracao.EstaPressionada ? _PRESSED : _RELEASED)}\n",
            $"(F) Lanterna: {(InputHandler.Instance.EntradaLanterna.EstaPressionada ? _PRESSED : _RELEASED)} ({(this._toggleLanterna ? _ON : _OFF)})\n",
            $"(MOUSE) Camera Delta: {InputHandler.Instance.EntradaVisao.Valor}\n",
            $"(L ALT) POV: {(InputHandler.Instance.EntradaPOV.EstaPressionada ? _PRESSED : _RELEASED)} ({(this._togglePOV ? _ON : _OFF)})\n"
        );
    }
}
