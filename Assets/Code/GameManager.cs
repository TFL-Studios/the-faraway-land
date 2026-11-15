using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private Canvas _blackoutCanvasPrefab;

    private GeneralUIController _uiController;

    public ControllMode CurrentControlMode { get { return this._uiController.CurrentControlMode; } }

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
