using UnityEngine;

public class MenuState : MonoBehaviour
{
    public GameObject MenuObject;
    public bool isActive { get; protected set; }
    public bool locksInputs;
    public bool locksCursor;
    public bool locksMenuButtons;

    public virtual void SetActive(bool state)
    {
        if (state)
        {
            PlayerManager.instance.LockInputs(locksInputs, locksCursor, locksMenuButtons);
        }
        MenuObject.SetActive(state);
        isActive = state;
    }
}
