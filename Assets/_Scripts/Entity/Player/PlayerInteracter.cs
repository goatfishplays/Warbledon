using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField] private Entity entity;
    public Transform castPoint;
    public float maxCastDistance = 1f;
    private int layerMask;
    private int interactableMask;
    // private Interactable target;

    // public bool interactionHeld = false;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Entities", "Interactable", "World");
        interactableMask = LayerMask.GetMask("Interactable");
    }


    public bool AttemptInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(castPoint.position, castPoint.up, maxCastDistance, interactableMask);
        if (!hit)
        {
            return false;
        }
        hit.collider.GetComponent<Interactable>().Interact(entity);
        return true;
    }
}
