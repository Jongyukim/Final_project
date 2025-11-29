using UnityEngine;
using TMPro;

public class CodeInputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeDisplay;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Transform codeInputPanel;
    [SerializeField] private CodeDrawer drawerScript;

    private string inputCode = "";
    private const int MAX_DIGITS = 3;

    void Start()
    {
        codeDisplay.text = "* * *";
        messageText.text = "";
    }

    void Update()
    {
        // 숫자 키 입력
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                OnNumberButtonClicked(i);
            }
        }

        // 백스페이스
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnBackspaceClicked();
        }

        // 엔터
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnConfirmClicked();
        }

        // Q키로 패널 닫기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClosePanel();
        }
    }

    public void OnNumberButtonClicked(int number)
    {
        if (inputCode.Length < MAX_DIGITS)
        {
            inputCode += number.ToString();
            UpdateDisplay();
            messageText.text = "";
        }
    }

    public void OnBackspaceClicked()
    {
        if (inputCode.Length > 0)
        {
            inputCode = inputCode.Substring(0, inputCode.Length - 1);
            UpdateDisplay();
            messageText.text = "";
        }
    }

    public void OnConfirmClicked()
    {
        if (inputCode.Length == MAX_DIGITS)
        {
            drawerScript.TryOpenWithCode(inputCode);
            inputCode = "";
            UpdateDisplay();
            messageText.text = "";
            ClosePanel();
        }
        else
        {
            messageText.text = "3자리를 입력하세요!";
        }
    }

    public void ClosePanel()
    {
        if (codeInputPanel != null)
        {
            codeInputPanel.gameObject.SetActive(false);
        }
        inputCode = "";
        UpdateDisplay();
        messageText.text = "";
    }

    void UpdateDisplay()
    {
        string display = "";
        for (int i = 0; i < MAX_DIGITS; i++)
        {
            if (i < inputCode.Length)
            {
                display += inputCode[i];
            }
            else
            {
                display += "*";
            }

            if (i < MAX_DIGITS - 1)
                display += " ";
        }
        codeDisplay.text = display;
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}