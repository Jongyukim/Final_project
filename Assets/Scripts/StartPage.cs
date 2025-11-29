using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "final_project";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료 (에디터에서는 안 꺼짐)");
    }
}
