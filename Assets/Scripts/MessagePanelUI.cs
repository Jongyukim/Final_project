using UnityEditor.Hardware;
using UnityEngine;

public class MessagePanelUI : MonoBehaviour
{
    [SerializeField] private Transform messagePanel;

    void Update()
    {
        // QÅ°·Î ´Ý±â
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CloseMessage();
        }
    }

    public void CloseMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.gameObject.SetActive(false);
        }

        // ÇÚµåÆùµµ ´Ý±â
        transform.parent.gameObject.SetActive(false);
    }
}