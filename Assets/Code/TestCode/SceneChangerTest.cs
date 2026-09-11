using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangerTest : MonoBehaviour
{
    [SerializeField] private Button _buttonPrefab;

    private void Start()
    {
        for (int index = 0; index < SceneManager.sceneCountInBuildSettings; index++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(index);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            Button sceneButton = GameObject.Instantiate(this._buttonPrefab, this.transform);
            sceneButton.onClick.AddListener(() => GameManager.Instance.TriggerSceneChange(sceneName));
            sceneButton.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;
            sceneButton.gameObject.SetActive(true);
        }

        GameObject.DontDestroyOnLoad(this.transform.parent.gameObject);
    }
}
