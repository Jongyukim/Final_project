using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("final_project");
    }
}