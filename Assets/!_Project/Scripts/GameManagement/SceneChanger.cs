using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private string _mainLevelSceneName = "MainScene";
    [SerializeField] private string _victorySceneName = "VictoryScreen";
    [SerializeField] private string _defeatSceneName = "DefeatScreen";

    public static SceneChanger Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SceneChanger");
                _instance = go.AddComponent<SceneChanger>();
            }

            return _instance;
        }
    }

    private static SceneChanger _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeSceneTo(SceneType scene)
    {
        switch (scene)
        {
            case SceneType.MainMenu:
                SceneManager.LoadScene(_mainMenuSceneName);
                break;
            case SceneType.MainLevel:
                SceneManager.LoadScene(_mainLevelSceneName);
                break;
            case SceneType.VictoryScene:
                SceneManager.LoadScene(_victorySceneName);
                break;
            case SceneType.DefeatScene:
                SceneManager.LoadScene(_defeatSceneName);
                break;
            default:
                Debug.LogError($"Scene {scene} is not supported.");
                break;
        }
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
