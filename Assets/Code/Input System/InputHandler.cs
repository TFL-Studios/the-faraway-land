using UnityEngine;
using UnityEngine.InputSystem;

public interface IEntrada
{
    public void RegistrarCallbacks();
    public void Habilitar();
    public void Desabilitar();
}

public class EntradaCustomizada<T> : IEntrada where T : struct
{
    private InputAction _acao;

    private T _valor;
    public T Valor { get { return this._valor; } }

    public bool FoiPressionada { get { return this._acao.WasPressedThisFrame(); } }
    public bool EstaPressionada { get { return this._acao.IsPressed(); } }
    public bool FoiSolta { get { return this._acao.WasReleasedThisFrame(); } }

    public EntradaCustomizada(InputAction acao)
    {
        this._acao = acao;
    }

    public void RegistrarCallbacks()
    {
        if (typeof(T) == typeof(bool)) return;
        this._acao.performed += context => this._valor = context.ReadValue<T>();
        this._acao.canceled += context => this._valor = default(T);
    }

    public void Habilitar() => this._acao.Enable();
    public void Desabilitar() => this._acao.Disable();
}

public class InputHandler : PersistentSingleton<InputHandler>
{
    [SerializeField] private InputActionAsset _assetDeAcoesDeEntrada;

    /* Entradas Customizadas */
    private IEntrada[] _entradas;
    // Interface
    private EntradaCustomizada<bool> _entradaMenu;
    private EntradaCustomizada<bool> _entradaInventario;
    private EntradaCustomizada<bool> _entradaDiario;
    private EntradaCustomizada<Vector2> _entradaNavegacao;
    private EntradaCustomizada<bool> _entradaSelecao;
    // Jogador
    private EntradaCustomizada<Vector2> _entradaMovimento;
    private EntradaCustomizada<bool> _entradaCorrida;
    private EntradaCustomizada<bool> _entradaAgachamento;
    private EntradaCustomizada<bool> _entradaInteracao;
    private EntradaCustomizada<bool> _entradaLanterna;
    private EntradaCustomizada<Vector2> _entradaVisao;
    private EntradaCustomizada<bool> _entradaPOV;

    /* Acesso */
    // Interface
    public EntradaCustomizada<bool> EntradaMenu { get { return this._entradaMenu; } }
    public EntradaCustomizada<bool> EntradaInventario { get { return this._entradaInventario; } }
    public EntradaCustomizada<bool> EntradaDiario { get { return this._entradaDiario; } }
    public EntradaCustomizada<Vector2> EntradaNavegacao { get { return this._entradaNavegacao; } }
    public EntradaCustomizada<bool> EntradaSelecao { get { return this._entradaSelecao; } }
    // Jogador
    public EntradaCustomizada<Vector2> EntradaMovimento { get { return this._entradaMovimento; } }
    public EntradaCustomizada<bool> EntradaCorrida { get { return this._entradaCorrida; } }
    public EntradaCustomizada<bool> EntradaAgachamento { get { return this._entradaAgachamento; } }
    public EntradaCustomizada<bool> EntradaInteracao { get { return this._entradaInteracao; } }
    public EntradaCustomizada<bool> EntradaLanterna { get { return this._entradaLanterna; } }
    public EntradaCustomizada<Vector2> EntradaVisao { get { return this._entradaVisao; } }
    public EntradaCustomizada<bool> EntradaPOV { get { return this._entradaPOV; } }

    protected override void Awake()
    {
        base.Awake();

        InputActionMap mapaDeAcoesDeEntradaDeInterface = this._assetDeAcoesDeEntrada.FindActionMap("Interface");
        InputActionMap mapaDeAcoesDeEntradaDoJogador = this._assetDeAcoesDeEntrada.FindActionMap("Jogador");

        this._entradas = new IEntrada[]
        {
            this._entradaMenu = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDeInterface.FindAction("Menu")),
            this._entradaInventario = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDeInterface.FindAction("Inventario")),
            this._entradaDiario = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDeInterface.FindAction("Diario")),
            this._entradaNavegacao = new EntradaCustomizada<Vector2>(mapaDeAcoesDeEntradaDeInterface.FindAction("Navegar")),
            this._entradaSelecao = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDeInterface.FindAction("Selecionar")),
            this._entradaMovimento = new EntradaCustomizada<Vector2>(mapaDeAcoesDeEntradaDoJogador.FindAction("Mover")),
            this._entradaCorrida = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDoJogador.FindAction("Correr")),
            this._entradaAgachamento = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDoJogador.FindAction("Agachar")),
            this._entradaInteracao = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDoJogador.FindAction("Interagir")),
            this._entradaLanterna = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDoJogador.FindAction("Lanterna")),
            this._entradaVisao = new EntradaCustomizada<Vector2>(mapaDeAcoesDeEntradaDoJogador.FindAction("Visao")),
            this._entradaPOV = new EntradaCustomizada<bool>(mapaDeAcoesDeEntradaDoJogador.FindAction("TrocarPOV"))
        };

        this.RegistrarCallbacksDasEntradas();
    }

    private void OnEnable()
    {
        for (int i = 0; i < this._entradas.Length; i++)
        {
            this._entradas[i].Habilitar();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < this._entradas.Length; i++)
        {
            this._entradas[i].Desabilitar();
        }
    }

    private void RegistrarCallbacksDasEntradas()
    {
        for (int i = 0; i < this._entradas.Length; i++)
        {
            this._entradas[i].RegistrarCallbacks();
        }
    }
}
