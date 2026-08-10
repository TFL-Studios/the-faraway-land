using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private Canvas _blackoutCanvasPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    public void TriggerSceneChange(string targerScene)
    {
        StartCoroutine(ChangeScene(targerScene));
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
