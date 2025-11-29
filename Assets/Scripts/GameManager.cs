using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float timeLimit = 300f;
    private float currentTime;
    private int puzzleStep = 0;

    private bool frameActivated = false;
    private int lightCount = 0;
    private Light[] allLights = new Light[3];
    private FrameActivatedDrawer autoDrawer;

    public bool IsFrameActivated => frameActivated;
    public int CurrentPuzzleStep => puzzleStep;
    public float CurrentTime => currentTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentTime = timeLimit;
    }

    void Update()
    {
        HandleTimer();
    }

    void HandleTimer()
    {
        if (puzzleStep >= 7) return;
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            // 더 이상 씬 로드 안 함 (GameProgress가 처리)
        }
    }

    public bool IsCorrectStep(int requiredStep)
    {
        return puzzleStep == requiredStep;
    }

    public void CompleteStep()
    {
        puzzleStep++;
        if (puzzleStep >= 7)
        {
            LoadGoodEnding(); // ← EndingScene만 로드
        }
    }

    public void RegisterLight(Light light)
    {
        if (lightCount < 3)
        {
            allLights[lightCount] = light;
            lightCount++;
            if (lightCount == 3)
            {
                TurnOnAllLights();
                CompleteStep();
            }
        }
    }

    void TurnOnAllLights()
    {
        foreach (Light l in allLights)
        {
            l.gameObject.SetActive(true);
        }
    }

    public void RegisterAutoDrawer(FrameActivatedDrawer drawer)
    {
        autoDrawer = drawer;
    }

    public void ActivateFrame()
    {
        if (puzzleStep != 1) return;
        frameActivated = true;
        if (autoDrawer != null)
        {
            autoDrawer.AutoOpen();
        }
        Note[] allNotes = FindObjectsOfType<Note>();
        foreach (Note note in allNotes)
        {
            note.gameObject.SetActive(true);
        }
        CompleteStep();
    }

    // ← 유지: EndingScene만 로드
    public void LoadGoodEnding()
    {
        SceneManager.LoadScene("EndingScene");
    }
}