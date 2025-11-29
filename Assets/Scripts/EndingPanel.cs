using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingPanel : MonoBehaviour
{
    void OnEnable()
    {
        // 패널이 활성화되면 마우스 커서 해제
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // GameProgress 파괴
        if (GameProgress.Instance != null)
        {
            Destroy(GameProgress.Instance.gameObject);
        }

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        // 씬 로드
        SceneManager.LoadScene("Final_project");
    }
}