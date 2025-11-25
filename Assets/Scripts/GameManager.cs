using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool frameActivated = false;
    private int lightCount = 0;
    private Light[] allLights = new Light[3];

    // 🔥 추가: 액자 레버가 여는 서랍 (자동 오픈)
    private FrameActivatedDrawer autoDrawer;


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

    public bool IsFrameActivated => frameActivated;
    public int LightCount => lightCount;

    // 🔥 추가: FrameActivatedDrawer에서 자기 자신 등록
    public void RegisterAutoDrawer(FrameActivatedDrawer drawer)
    {
        autoDrawer = drawer;
    }

    // 🔥 수정: 액자 활성화 시 자동으로 서랍 열기
    public void ActivateFrame()
    {
        frameActivated = true;

        if (autoDrawer != null)
        {
            autoDrawer.AutoOpen();
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
            }
        }
    }

    void TurnOnAllLights()
    {
        for (int i = 0; i < allLights.Length; i++)
        {
            allLights[i].gameObject.SetActive(true);
        }
    }
}
