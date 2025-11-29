using UnityEngine;

public class Note : Interactable
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Sprite noteIcon;
    [SerializeField] private Sprite noteDetailImage;
    [SerializeField] private string noteTitle = "포스트잇";

    private bool hasBeenActivated = false;

    void Start()
    {
        interactText = "포스트잇을 줍는다 (E)";
    }

    void Update()
    {
        bool canProgress = GameProgress.Instance.CanProgressPostits();
        Debug.Log(gameObject.name + " - CanProgressPostits: " + canProgress + " - isActive: " + gameObject.activeSelf);

        if (!canProgress && !hasBeenActivated)
        {
            interactText = "";
            gameObject.SetActive(false);
            return;
        }
        else if (gameObject.activeSelf)
        {
            interactText = "포스트잇을 줍는다 (E)";
        }
    }

    public override void Interact()
    {
        Debug.Log("===== Note.Interact() 호출 =====");
        Debug.Log("CanProgressPostits: " + GameProgress.Instance.CanProgressPostits());

        if (!GameProgress.Instance.CanProgressPostits())
        {
            Debug.Log("아직 이 노트를 줍을 수 없습니다.");
            return;
        }

        hasBeenActivated = true;
        Item note = new Item(noteTitle, "뭔가 적혀있다", noteIcon, noteDetailImage);
        inventoryManager.AddItem(note);

        Debug.Log(noteTitle + "을 획득했다!");
        Debug.Log("총 포스트잇: " + GetPostitsCount());

        // ← 추가: 3개 모두 모으면 완료
        if (GetPostitsCount() == 3)
        {
            Debug.Log("포스트잇 3개 모두 모음! Stage 업데이트!");
            GameProgress.Instance.CompletePostitPuzzle();
        }

        Destroy(gameObject);
    }

    int GetPostitsCount()
    {
        int count = 0;
        foreach (Item item in inventoryManager.GetInventory())
        {
            if (item != null && item.itemName.Contains("포스트잇"))
                count++;
        }
        return count;
    }
}