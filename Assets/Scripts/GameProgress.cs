using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Transform codeInputPanel;
    [SerializeField] private Transform phoneDetailPanel;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Transform endingPanel;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private float fadeDuration = 3f;

    [SerializeField] private int currentStage = 0;
    private float timeRemaining = 300f;
    private bool gameStarted = false;
    private bool gameEnded = false;

    private int bulbCount = 0;
    private bool frameLeverUsed = false;
    private bool postitsCollected = false;
    private bool deskUnlocked = false;
    private bool phoneUnlocked = false;
    private bool usbCollected = false;
    private bool computerSolved = false;

    public int CurrentStage => currentStage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameStarted || gameEnded)
            return;

        if (hintText == null || timerText == null)
            return;

        if ((codeInputPanel != null && codeInputPanel.gameObject.activeSelf) ||
            (phoneDetailPanel != null && phoneDetailPanel.gameObject.activeSelf))
        {
            hintText.gameObject.SetActive(false);
        }
        else
        {
            hintText.gameObject.SetActive(true);
        }

        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        currentStage = 0;
        ShowHint("방이 어두워 먼저 밝혀야겠다..");
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ShowHint(string text)
    {
        if (hintText == null) return;

        hintText.text = text;
    }

    public void CompleteBulbPuzzle()
    {
        if (currentStage != 0) return;

        bulbCount++;

        if (bulbCount >= 3)
        {
            currentStage = 1;
            ShowHint("이제 주변을 제대로 볼 수 있다. 무언가 이상한 액자가 보인다...");
        }
    }

    public void CompleteLeverPuzzle()
    {
        if (currentStage != 1) return;

        frameLeverUsed = true;
        currentStage = 2;
        ShowHint("어딘가 열리는 소리가 들렸다. 확인해보자.");
    }

    public void CompletePostitPuzzle()
    {
        if (currentStage != 2) return;

        postitsCollected = true;
        currentStage = 3;
        ShowHint("찢어진 종이 조각들... 날짜가 적혀있다. 뭔가의 비밀번호일까?");
    }

    public void CompleteDeskPuzzle()
    {
        if (currentStage != 3) return;

        deskUnlocked = true;
        currentStage = 4;
        ShowHint("책상 서랍 안에 핸드폰이 있다. 비밀번호를 입력해보자.");
    }

    public void CompletePhonePuzzle()
    {
        if (currentStage != 4) return;

        phoneUnlocked = true;
        currentStage = 5;
        ShowHint("핸드폰의 메시지... USB가 숨겨져 있다고?");
    }

    public void CompleteUSBCollection()
    {
        if (currentStage != 5) return;

        usbCollected = true;
        currentStage = 6;
        ShowHint("USB를 찾았다. 컴퓨터에 연결해서 뭐가 있는지 확인해보자.");
    }

    public void CompleteComputerPuzzle()
    {
        if (currentStage != 6) return;

        computerSolved = true;
        GameComplete();
    }

    void GameComplete()
    {
        gameEnded = true;
        ShowHint("게임 완료! 축하합니다!");
        StartCoroutine(FadeOutAndShowEnding(true));
    }

    void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        ShowHint("시간이 다 되었습니다. 게임 오버!");
        StartCoroutine(FadeOutAndShowEnding(false));
    }

    IEnumerator FadeOutAndShowEnding(bool isSuccess)
    {
        if (fadeImage == null)
        {
            ShowEndingPanel(isSuccess);
            yield break;
        }

        Color fadeColor = fadeImage.color;
        fadeColor.a = 0f;
        fadeImage.color = fadeColor;
        fadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                fadeImage.color = fadeColor;
            }
            yield return null;
        }

        if (fadeImage != null)
        {
            fadeColor.a = 1f;
            fadeImage.color = fadeColor;
        }

        ShowEndingPanel(isSuccess);
    }

    void ShowEndingPanel(bool isSuccess)
    {
        if (endingPanel != null)
        {
            endingPanel.gameObject.SetActive(true);

            if (endingText != null)
            {
                if (isSuccess)
                {
                    endingText.text = "그 영상이 담은 진실을 마주했다.\n사건의 전말을 알게 되었고,\n나는 이 방을 떠날 수 있게 되었다.\n\n모든 것이 끝났다.";
                }
                else
                {
                    endingText.text = "시간이 부족했다.\n증거를 찾지 못했고,\n사건은 미궁 속에 사라졌다.\n\n모든 것이 끝나버렸다.";
                }
            }
        }
    }

    public bool CanProgressBulb() => currentStage == 0;
    public bool CanProgressLever() => currentStage == 1;
    public bool CanProgressPostits() => currentStage == 2;
    public bool CanProgressDesk() => currentStage == 3;
    public bool CanProgressPhone() => currentStage == 4;
    public bool CanProgressUSB() => currentStage == 5;
    public bool CanProgressComputer() => currentStage == 6;

    public bool IsGameActive() => gameStarted && !gameEnded;
}