using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : PersistentSingleton<InputHandler>
{
    [SerializeField] private InputActionAsset _assetDeAcoesDeEntrada;

    private InputAction[] _acoesDeEntrada;

    /* Acoes de Input */
    // Interface
    private InputAction _acaoDeEntradaDeAbrirMenu;
    private InputAction _acaoDeEntradaDeAbrirInventario;
    private InputAction _acaoDeEntradaDeAbrirDiario;
    private InputAction _acaoDeEntradaDeNavegar;
    private InputAction _acaoDeEntradaDeSelecionar;
    // Jogador
    private InputAction _acaoDeEntradaDeMover;
    private InputAction _acaoDeEntradaDeCorrer;
    private InputAction _acaoDeEntradaDeAgachar;
    private InputAction _acaoDeEntradaDeInteragir;
    private InputAction _acaoDeEntradaDeLanterna;
    private InputAction _acaoDeEntradaDeVisao;
    private InputAction _acaoDeEntradaDeTrocarPOV;

    /* Valores */
    // Interface
    private bool _entradaDeAbrirOMenuDaInterface;
    public bool EntradaDeAbrirOMenuDaInterface { get { return this._entradaDeAbrirOMenuDaInterface; } }

    private bool _entradaDeAbrirOInventarioDaInterface;
    public bool EntradaDeAbrirOInventarioDaInterface { get { return this._entradaDeAbrirOInventarioDaInterface; } }

    private bool _entradaDeAbrirODiarioDaInterface;
    public bool EntradaDeAbrirODiarioDaInterface { get { return this._entradaDeAbrirODiarioDaInterface; } }

    private Vector2 _entradaDeNavegacaoDaInterface;
    public Vector2 EntradaDeNavegacaoDaInterface { get { return this._entradaDeNavegacaoDaInterface; } }

    private bool _entradaDeSelecaoDaInterface;
    public bool EntradaDeSelecaoDaInterface { get { return this._entradaDeSelecaoDaInterface; } }

    // Jogador
    private Vector2 _entradaDeMovimentoDoJogador;
    public Vector2 EntradaDeMovimentoDoJogador { get { return this._entradaDeMovimentoDoJogador; } }

    private bool _entradaDeCorridaDoJogador;
    public bool EntradaDeCorridaDoJogador { get { return this._entradaDeCorridaDoJogador; } }

    private bool _entradaDePuloDoJogador;
    public bool EntradaDePuloDoJogador { get { return this._entradaDePuloDoJogador; } }

    private bool _entradaDeAgacharDoJogador;
    public bool EntradaDeAgacharDoJogador { get { return this._entradaDeAgacharDoJogador; } }

    private bool _entradaDeInteracaoDoJogador;
    public bool EntradaDeInteracaoDoJogador { get { return this._entradaDeInteracaoDoJogador; } }

    private bool _entradaDeLanternaDoJogador;
    public bool EntradaDeLanternaDoJogador { get { return this._entradaDeLanternaDoJogador; } }

    private Vector2 _entradaDeVisaoDoJogador;
    public Vector2 EntradaDeVisaoDoJogador { get { return this._entradaDeVisaoDoJogador; } }

    private bool _entradaDeTrocaDePOVDoJogador;
    public bool EntradaDeTrocaDePOVDoJogador { get { return this._entradaDeTrocaDePOVDoJogador; } }

    protected override void Awake()
    {
        base.Awake();

        InputActionMap mapaDeAcoesDeEntradaDeInterface = this._assetDeAcoesDeEntrada.FindActionMap("Interface");
        InputActionMap mapaDeAcoesDeEntradaDoJogador = this._assetDeAcoesDeEntrada.FindActionMap("Jogador");

        this._acoesDeEntrada = new InputAction[]
        {
            // Interface
            this._acaoDeEntradaDeAbrirMenu = mapaDeAcoesDeEntradaDeInterface.FindAction("Menu"),
            this._acaoDeEntradaDeAbrirInventario = mapaDeAcoesDeEntradaDeInterface.FindAction("Inventario"),
            this._acaoDeEntradaDeAbrirDiario = mapaDeAcoesDeEntradaDeInterface.FindAction("Diario"),
            this._acaoDeEntradaDeNavegar = mapaDeAcoesDeEntradaDeInterface.FindAction("Navegar"),
            this._acaoDeEntradaDeSelecionar = mapaDeAcoesDeEntradaDeInterface.FindAction("Selecionar"),
            // Jogador
            this._acaoDeEntradaDeMover = mapaDeAcoesDeEntradaDoJogador.FindAction("Mover"),
            this._acaoDeEntradaDeCorrer = mapaDeAcoesDeEntradaDoJogador.FindAction("Correr"),
            this._acaoDeEntradaDeAgachar = mapaDeAcoesDeEntradaDoJogador.FindAction("Agachar"),
            this._acaoDeEntradaDeInteragir = mapaDeAcoesDeEntradaDoJogador.FindAction("Interagir"),
            this._acaoDeEntradaDeLanterna = mapaDeAcoesDeEntradaDoJogador.FindAction("Lanterna"),
            this._acaoDeEntradaDeVisao = mapaDeAcoesDeEntradaDoJogador.FindAction("Visao"),
            this._acaoDeEntradaDeTrocarPOV = mapaDeAcoesDeEntradaDoJogador.FindAction("TrocarPOV"),
        };

        this.RegistrarAcoesDeEntrada();
    }

    private void OnEnable()
    {
        for (int i = 0; i < this._acoesDeEntrada.Length; i++)
        {
            this._acoesDeEntrada[i].Enable();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < this._acoesDeEntrada.Length; i++)
        {
            this._acoesDeEntrada[i].Disable();
        }
    }

    private void RegistrarAcoesDeEntrada()
    {
        // Interface
        this._acaoDeEntradaDeAbrirMenu.performed += context => this._entradaDeAbrirOMenuDaInterface = true;
        this._acaoDeEntradaDeAbrirMenu.canceled += context => this._entradaDeAbrirOMenuDaInterface = false;

        this._acaoDeEntradaDeAbrirInventario.performed += context => this._entradaDeAbrirOInventarioDaInterface = true;
        this._acaoDeEntradaDeAbrirInventario.canceled += context => this._entradaDeAbrirOInventarioDaInterface = false;

        this._acaoDeEntradaDeAbrirDiario.performed += context => this._entradaDeAbrirODiarioDaInterface = true;
        this._acaoDeEntradaDeAbrirDiario.canceled += context => this._entradaDeAbrirODiarioDaInterface = false;

        this._acaoDeEntradaDeNavegar.performed += context => this._entradaDeNavegacaoDaInterface = context.ReadValue<Vector2>();
        this._acaoDeEntradaDeNavegar.canceled += context => this._entradaDeNavegacaoDaInterface = Vector2.zero;

        this._acaoDeEntradaDeSelecionar.performed += context => this._entradaDeSelecaoDaInterface = true;
        this._acaoDeEntradaDeSelecionar.canceled += context => this._entradaDeSelecaoDaInterface = false;

        // Jogador
        this._acaoDeEntradaDeMover.performed += context => this._entradaDeMovimentoDoJogador = context.ReadValue<Vector2>();
        this._acaoDeEntradaDeMover.canceled += context => this._entradaDeMovimentoDoJogador = Vector2.zero;

        this._acaoDeEntradaDeCorrer.performed += context => this._entradaDeCorridaDoJogador = true;
        this._acaoDeEntradaDeCorrer.canceled += context => this._entradaDeCorridaDoJogador = false;

        this._acaoDeEntradaDeAgachar.performed += context => this._entradaDeAgacharDoJogador = true;
        this._acaoDeEntradaDeAgachar.canceled += context => this._entradaDeAgacharDoJogador = false;

        this._acaoDeEntradaDeInteragir.performed += context => this._entradaDeInteracaoDoJogador = true;
        this._acaoDeEntradaDeInteragir.canceled += context => this._entradaDeInteracaoDoJogador = false;

        this._acaoDeEntradaDeLanterna.performed += context => this._entradaDeLanternaDoJogador = true;
        this._acaoDeEntradaDeLanterna.canceled += context => this._entradaDeLanternaDoJogador = false;

        this._acaoDeEntradaDeVisao.performed += context => this._entradaDeVisaoDoJogador = context.ReadValue<Vector2>();
        this._acaoDeEntradaDeVisao.canceled += context => this._entradaDeVisaoDoJogador = Vector2.zero;

        this._acaoDeEntradaDeTrocarPOV.performed += context => this._entradaDeTrocaDePOVDoJogador = true;
        this._acaoDeEntradaDeTrocarPOV.canceled += context => this._entradaDeTrocaDePOVDoJogador = false;
    }
}
