using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected float interactDistance = 3f;
    [SerializeField] protected string interactText = "상호작용 (E)";

    public float InteractDistance => interactDistance;
    public string InteractText => interactText;

    public virtual void Interact()
    {
        Debug.Log(gameObject.name + "과 상호작용!");
    }
}