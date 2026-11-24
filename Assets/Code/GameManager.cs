using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private Canvas _blackoutCanvasPrefab;

    private SaveSystem _saveSystem;
    public SaveSystem SaveSystem { get { return this._saveSystem; } }

    private GeneralUIController _uiController;

    public ControllMode CurrentControlMode { get { return this._uiController.CurrentControlMode; } }

    [SerializeField] public LevelSaveData _currentLevel;

    private void OnEnable()
    {
        DialogController.OnEndDialog += OnEndDialog_Callback;
    }

    private void OnDisable()
    {
        DialogController.OnEndDialog -= OnEndDialog_Callback;
    }

    protected override void Awake()
    {
        base.Awake();

        this._uiController = GameObject.FindFirstObjectByType<GeneralUIController>();
    }

    private void Start()
    {
        this._saveSystem = new SaveSystem();

        int currentLevelID = this._saveSystem.FetchCurrentLevelID();
        this._currentLevel = this._saveSystem.FetchLevel(currentLevelID);

        // ph test
        //this._currentLevel.levelID = 2;

        //this._saveSystem.SaveCurrentLevelID(this._currentLevel.levelID);
        //this._saveSystem.SaveLevel(this._currentLevel);
        //this._saveSystem.PersistSavedData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (this._saveSystem == null) return;
        this._saveSystem.PersistSavedData();
    }

    private void OnApplicationQuit()
    {
        if (this._saveSystem == null) return;
        this._saveSystem.PersistSavedData();
    }

    public void StartDialog(DialogBlock dialogBlock)
    {
        this._uiController.EnableDialogUI(dialogBlock);
    }

    public void OnEndDialog_Callback(DialogBlock dialogBlock, int dialogOutcome)
    {
        this._uiController.DisableDialogUI();
    }

    public void TriggerSceneChange(string targerScene)
    {
        this.StartCoroutine(this.ChangeScene(targerScene));
    }

    private IEnumerator ChangeScene(string targetScene)
    {
        Canvas blackoutCanvas = GameObject.Instantiate(this._blackoutCanvasPrefab);
        GameObject.DontDestroyOnLoad(blackoutCanvas.gameObject);
        RawImage blackoutImage = blackoutCanvas.GetComponentInChildren<RawImage>();
        yield return StartCoroutine(FadeImage(blackoutImage, 1f));
        yield return SceneManager.LoadSceneAsync(targetScene);
        yield return StartCoroutine(FadeImage(blackoutImage, 0f));
        GameObject.Destroy(blackoutCanvas.gameObject);
    }

    private IEnumerator FadeImage(RawImage image, float targerAlpha)
    {
        if (image.color.a < targerAlpha)
        {
            while (image.color.a < targerAlpha)
            {
                Color colorBuffer = image.color;
                colorBuffer.a += Time.deltaTime;
                image.color = colorBuffer;
                yield return new WaitForEndOfFrame();
            }
        }
        else if (image.color.a > targerAlpha)
        {
            while (image.color.a > targerAlpha)
            {
                Color colorBuffer = image.color;
                colorBuffer.a -= Time.deltaTime;
                image.color = colorBuffer;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
