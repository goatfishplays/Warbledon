using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    // TODO replace SortedList with custom version of sorted set that allows forwards and backwards getting, screw whoever wrote IEnumerator with MoveNext() but no MovePrev()
    protected Dictionary<string, SortedList<int, ItemSO>> itemTabs = new Dictionary<string, SortedList<int, ItemSO>>();
    protected static readonly string[] visibleTabs = new string[] { ItemManager.ITEMS_TAB, ItemManager.KEY_ITEMS_TAB, ItemManager.BEINGS_TAB };
    public Dictionary<ItemSO, int> itemCounts = new Dictionary<ItemSO, int>();
    public ItemSO[] initialization;
    // public ItemSO primaryItem;
    // [SerializeField] protected ItemUICountElement primaryItemUI; 
    public Entity entity;
    // [SerializeField] float dropOffset = 1f;

    protected ItemManager itemManager => ItemManager.instance;

    private void Start()
    {
        foreach (string tab in visibleTabs)
        {
            itemTabs.Add(tab, new SortedList<int, ItemSO>());
        }

        StartCoroutine(Initialize());
    }

    protected IEnumerator Initialize()
    {
        yield return null;
        foreach (ItemSO item in initialization)
        {
            // Debug.Log("WHY");
            AddItem(item);
        }

    }
    // public void Equip(ItemSO item)
    // {
    //     primaryItem = item;
    //     primaryItemUI.SetItem(item, GetItemCount(item));
    // }

    public void Drop(ItemSO item) // * not sure if this method needs to exist in games wehre you don't have physical drops
    {
        if (GetItemCount(item) <= 0)
        {
            Debug.LogWarning($"Attempted to drop item: '{item.name}' with 0 count");
            return;
        }
        RemoveItem(item);
        // DropManager.instance.CreateDrop(entity.transform.position + dropOffset * entity.transform.forward, Quaternion.identity, item, 1, 1f);

        // if (primaryItem == item) 
        // {
        //     primaryItemUI.SetCount(GetItemCount(item));
        // }
    }

    // public bool CanUsePrimary()
    // {
    //     return primaryItem != null && GetItemCount(primaryItem) > 0;
    // }

    // public bool AttemptUsePrimary()
    // {
    //     if (!AttemptUseItem(primaryItem))
    //     {
    //         return false;
    //     }
    //     int amtLeft = GetItemCount(primaryItem);
    //     // if (amtLeft > 0)
    //     // {
    //     primaryItemUI.SetCount(amtLeft);
    //     // } // TODO idrk if it should auto clear or how to manage that, someone remind me to make it so that when you have no item and pick up item it auto selects that one 
    //     // else 
    //     // {
    //     //     primaryItemUI.ClearItem(); 
    //     // }
    //     return true;
    // }

    public SortedList<int, ItemSO> GetTabItems(string tabName)
    {
        if (!itemTabs.ContainsKey(tabName))
        {
            Debug.Log($"Tried to get tab '{tabName}' where it does not exist");
            return null;
        }
        return itemTabs[tabName];
    }

    /// <summary>
    /// Gets itemTab from among the VISIBLE tabs
    /// </summary>
    /// <param name="tabIndex"></param>
    /// <returns></returns>
    public SortedList<int, ItemSO> GetTabItems(int tabIndex)
    {
        // Debug.Log(tabIndex % visibleTabs.Count); // screw you C#
        int div = visibleTabs.Length;
        int ind = tabIndex % div;
        if (ind < 0)
        {
            ind += div;
        }
        return itemTabs[visibleTabs[ind]];
    }

    /// <summary>
    /// Gets tabName from among the VISIBLE tabs
    /// </summary>
    /// <param name="tabIndex"></param>
    /// <returns></returns> 
    public string GetTabName(int tabIndex)
    {
        int div = visibleTabs.Length;
        int ind = tabIndex % div;
        if (ind < 0)
        {
            ind += div;
        }
        return visibleTabs[ind];
    }

    public bool HasEnough(ItemSO item, int count)
    {
        return itemCounts.TryGetValue(item, out int val) && val >= count;
        // return itemTabs.TryGetValue(item.tab, out InventoryTab tab) && tab.HasEnough(item, count);
    }

    public int GetItemCount(ItemSO item)
    {
        if (item == null)
        {
            return 0;
        }
        return itemCounts.GetValueOrDefault(item, 0);
        // return itemTabs.TryGetValue(item.tab, out InventoryTab tab) ? tab.GetItemCount(item) : 0;
    }

    public virtual void AddItem(ItemSO item, int count = 1)
    {
        // if (!itemTabs.TryGetValue(item.tab, out InventoryTab tab))
        // {
        //     itemTabs.Add(item.tab, new InventoryTab(item.tab));
        //     tab = itemTabs[item.tab];
        //     // return;
        // }
        // tab.AddItem(item, count);

        if (!itemCounts.TryAdd(item, count)) // if already in just add on if not in add and add to tabs
        {
            itemCounts[item] += count;
        }
        else
        {
            itemTabs[item.tab].Add(itemManager.GetId(item), item);
        }


        // if (item == primaryItem)
        // { 
        //     Equip(primaryItem);
        // }
    }

    public virtual void RemoveItem(ItemSO item, int count = 1)
    {
        if (GetItemCount(item) <= 0)
        {
            Debug.LogWarning($"Attempted to remove item: '{item.name}' with 0 count");
            return;
        }
        itemCounts[item] -= count;
        if (itemCounts[item] <= 0)
        {
            itemCounts.Remove(item);
            itemTabs[item.tab].Remove(itemManager.GetId(item));
        }
        // if (!itemTabs.TryGetValue(item.tab, out InventoryTab tab))
        // {
        //     return;
        // }
        // tab.RemoveItem(item, count);
    }


    public bool AttemptUseItem(ItemSO item) // TODO make this server side only
    {
        if (GetItemCount(item) > 0 && item.AttemptUse(entity))
        {
            // Debug.Log($"{gameObject.name} used Item: '{item.itemName}'");  
            if (item.consumeOnUse)
            {
                RemoveItem(item);
            }
            return true;
        }
        return false;
        // return itemTabs.TryGetValue(item.tab, out InventoryTab tab) && tab.AttemptUseItem(item, entity);
    }



}
