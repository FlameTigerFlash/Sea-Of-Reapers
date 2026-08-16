using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void ToMainMenu()
    {
        SceneChanger.Instance.ChangeSceneTo(SceneType.MainMenu);
    }

    public void ToMainLevel()
    {
        SceneChanger.Instance.ChangeSceneTo(SceneType.MainLevel);
    }

    public void ToVictoryScreen()
    {
        SceneChanger.Instance.ChangeSceneTo(SceneType.VictoryScene);
    }

    public void ToDefeatScreen()
    {
        SceneChanger.Instance.ChangeSceneTo(SceneType.DefeatScene);
    }

    public void QuitGame()
    {
        SceneChanger.Instance.Quit();
    }
}
