using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int lightCount = 0;
    private Light[] allLights = new Light[3];

    private FrameActivatedDrawer autoDrawer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   // 필요 없으면 삭제해도 됨
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterLight(Light light)
    {
        // 순서 체크 (0단계일 때만 진행)
        if (GameProgress.Instance != null &&
            !GameProgress.Instance.CanProgressBulb())
            return;

        if (lightCount < 3)
        {
            allLights[lightCount] = light;
            lightCount++;

            if (lightCount == 3)
            {
                TurnOnAllLights();
                GameProgress.Instance?.CompleteBulbPuzzle();
            }
        }
    }

    private void TurnOnAllLights()
    {
        foreach (var l in allLights)
        {
            if (l != null)
                l.gameObject.SetActive(true);
        }
    }

    public void RegisterAutoDrawer(FrameActivatedDrawer drawer)
    {
        autoDrawer = drawer;
    }

    public void ActivateFrame()
    {
        // GameProgress 단계 진행
        if (GameProgress.Instance != null &&
            GameProgress.Instance.CanProgressLever())
        {
            GameProgress.Instance.CompleteLeverPuzzle();
        }

        // 자동 서랍 열기
        if (autoDrawer != null)
            autoDrawer.AutoOpen();

        // 포스트잇( Note ) 전부 활성화
        Note[] notes = GameObject.FindObjectsOfType<Note>(true);
        foreach (var n in notes)
            n.gameObject.SetActive(true);
    }

    public void LoadGoodEnding()
    {
        SceneManager.LoadScene("EndingScene");
    }

    public void ResetGame()
    {
        lightCount = 0;
        allLights = new Light[3];
        autoDrawer = null;
    }
}
