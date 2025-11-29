using UnityEngine;
using TMPro;

public class PhonePasswordUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passwordDisplay;
    [SerializeField] private TextMeshProUGUI passwordMessage;
    [SerializeField] private Transform inputPanel; // 비밀번호 입력 패널
    [SerializeField] private Transform messagePanel; // 메시지 패널

    private string inputPassword = "";
    private const int MAX_DIGITS = 4;
    private string correctPassword = "0513";
    private bool isUnlocked = false;

    void Start()
    {
        passwordDisplay.text = "* * * *";
        passwordMessage.text = "";

        if (messagePanel != null)
        {
            messagePanel.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Q키로 닫기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClosePanel();
        }
    }

    public void OnNumberButtonClicked(int number)
    {
        if (!isUnlocked && inputPassword.Length < MAX_DIGITS)
        {
            inputPassword += number.ToString();
            UpdateDisplay();
            passwordMessage.text = "";
        }
    }

    public void OnBackspaceClicked()
    {
        if (!isUnlocked && inputPassword.Length > 0)
        {
            inputPassword = inputPassword.Substring(0, inputPassword.Length - 1);
            UpdateDisplay();
            passwordMessage.text = "";
        }
    }

    public void OnConfirmClicked()
    {
        if (isUnlocked)
            return;

        if (inputPassword.Length == MAX_DIGITS)
        {
            if (inputPassword == correctPassword)
            {
                // 정답 - 메시지 패널 표시
                isUnlocked = true;
                ShowMessage();
                if (GameProgress.Instance != null)
                {
                    GameProgress.Instance.CompletePhonePuzzle();
                }

            }
            else
            {
                // 틀림
                passwordMessage.text = "틀렸습니다!";
                inputPassword = "";
                UpdateDisplay();
            }
        }
        else
        {
            // 4자리 아님
            passwordMessage.text = "4자리를 입력하세요!";
        }
    }

    void ShowMessage()
    {
        if (inputPanel != null)
        {
            inputPanel.gameObject.SetActive(false);
        }

        if (messagePanel != null)
        {
            messagePanel.gameObject.SetActive(true);
        }

        Debug.Log("메시지 표시!");
    }

    public void ClosePanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isUnlocked = false;
        inputPassword = "";
        passwordDisplay.text = "* * * *";
        passwordMessage.text = "";

        if (messagePanel != null)
        {
            messagePanel.gameObject.SetActive(false);
        }
        if (inputPanel != null)
        {
            inputPanel.gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateDisplay()
    {
        string display = "";
        for (int i = 0; i < MAX_DIGITS; i++)
        {
            if (i < inputPassword.Length)
            {
                display += "*";
            }
            else
            {
                display += "*";
            }

            if (i < MAX_DIGITS - 1)
                display += " ";
        }
        passwordDisplay.text = display;
    }
}