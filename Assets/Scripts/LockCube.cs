using UnityEngine;

public class LockCube : Interactable
{
    [SerializeField] private CodeDrawer codeDrawer;

    void Start()
    {
        interactText = "자물쇠 풀기 (E)";
        Debug.Log("LockCube Start - CodeDrawer: " + (codeDrawer != null ? "있음" : "없음"));
    }

    void Update()
    {
        if (!GameProgress.Instance.CanProgressDesk())
        {
            interactText = "";
            return;
        }
        interactText = "자물쇠 풀기 (E)";
    }

    public override void Interact()
    {
        Debug.Log("===== LockCube.Interact() 호출됨! =====");
        Debug.Log("GameProgress Stage: " + GameProgress.Instance.CurrentStage);
        Debug.Log("CanProgressDesk: " + GameProgress.Instance.CanProgressDesk());

        if (!GameProgress.Instance.CanProgressDesk())
        {
            Debug.Log("아직 이 자물쇠를 풀 수 없습니다.");
            return;
        }

        Debug.Log("CodeDrawer: " + (codeDrawer != null ? "있음" : "없음"));
        if (codeDrawer != null)
        {
            Debug.Log("CodeDrawer.Interact() 호출!");
            codeDrawer.Interact();
        }
    }
}