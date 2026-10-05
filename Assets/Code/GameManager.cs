using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct Quest
{
    public bool complete;
    public QuestData questData;
}

public class GameManager : PersistentSingleton<GameManager>
{
    public int activeQuestIndex = 0; // ph
    public Quest[] quests;

    [SerializeField] private Canvas _blackoutCanvasPrefab;

    protected override void Awake()
    {
        base.Awake();
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
