using UnityEngine;

public class InvnetoryMenuState : MenuState
{
    public ItemUIInventoryController inventoryUI;

    public override void SetActive(bool state)
    {
        MenuObject.SetActive(state);
        isActive = state;
        if (state)
        {
            inventoryUI.OpenInventory();
        }
    }
}