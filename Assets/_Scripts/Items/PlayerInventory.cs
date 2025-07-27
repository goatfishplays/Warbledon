using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] protected ItemUIInventoryController controllerUI;

    // public void Start()
    // {
    //     // initailize visible tabs 
    //     // itemTabs.Add("Items", new InventoryTab());
    //     // itemTabs.Add("Key Items", new InventoryTab());
    //     foreach (string tabName in visibleTabs)
    //     {
    //         // itemTabs.Add(tabName, new InventoryTab(tabName)); 
    //         itemTabs.Add(tabName, new SortedList<int, ItemSO>());
    //     }
    // }

    public override void AddItem(ItemSO item, int count = 1)
    {
        base.AddItem(item, count);
        if (controllerUI && controllerUI.gameObject.activeInHierarchy)
        {
            controllerUI.UpdateScreen(GetTabItems(controllerUI.curTab), GetTabName(controllerUI.curTab), false);
        }
    }

    public override void RemoveItem(ItemSO item, int count = 1)
    {
        base.RemoveItem(item, count);
        if (controllerUI && controllerUI.gameObject.activeInHierarchy)
        {

            controllerUI.UpdateScreen(GetTabItems(controllerUI.curTab), GetTabName(controllerUI.curTab), !itemCounts.ContainsKey(item));
        }
    }
}