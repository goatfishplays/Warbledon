using UnityEngine;

public class ItemIconSelectionBehavior : MonoBehaviour
{
    public ItemUIInventoryController controller;
    // public ItemUICountElement itemHolder;

    public void SendItem()
    {
        controller.SetSelected(GetComponent<ItemUICountElement>());
    }

}
