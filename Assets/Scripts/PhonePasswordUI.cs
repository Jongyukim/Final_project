using UnityEngine;
using TMPro;

public class PhonePasswordUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passwordDisplay;
    [SerializeField] private TextMeshProUGUI passwordMessage;
    [SerializeField] private Transform inputPanel;
    [SerializeField] private Transform messagePanel;

    private string inputPassword = "";
    private const int MAX_DIGITS = 4;
    private string correctPassword = "0513";
    private bool isUnlocked = false;
    private MouseLook mouseLook; 

    void Start()
    {
        passwordDisplay.text = "* * * *";
        passwordMessage.text = "";

        if (messagePanel != null)
        {
            messagePanel.gameObject.SetActive(false);
        }

        mouseLook = FindObjectOfType<MouseLook>();
    }

    void Update()
    {
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
                isUnlocked = true;
                ShowMessage();
                if (GameProgress.Instance != null)
                {
                    GameProgress.Instance.CompletePhonePuzzle();
                }
            }
            else
            {
                passwordMessage.text = "Wrong password!";
                inputPassword = "";
                UpdateDisplay();
            }
        }
        else
        {
            passwordMessage.text = "Enter 4 digits!";
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

        Debug.Log("Message displayed!");
    }

    public void ClosePanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mouseLook != null)
        {
            mouseLook.enabled = true;
        }
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