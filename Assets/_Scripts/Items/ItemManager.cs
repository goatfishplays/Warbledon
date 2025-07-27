using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public const string ITEMS_TAB = "Items";
    public const string KEY_ITEMS_TAB = "Key Items";
    public const string BEINGS_TAB = "Beings";
    public ItemSO NONE_ITEM;
    public ItemSO[] itemInitializer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple ItemManagers detected, deleting extra");
            Destroy(this);
        }
        InitializeItems();
    }

    private void InitializeItems()
    {
        foreach (ItemSO itemSO in itemInitializer)
        {
            AddItem(itemSO);
        }

    }


    private Dictionary<string, int> nameToInd = new Dictionary<string, int>();
    private List<ItemSO> itemCatalogue = new List<ItemSO>();

    public void AddItem(ItemSO item)
    {
        if (nameToInd.ContainsKey(item.name))
        {
            Debug.LogError($"Attempted to add '{item.name}' to Item Manager where '{item.name}' already exists");
            return;
        }
        int newInd = nameToInd.Count;
        nameToInd.Add(item.name, newInd);
        itemCatalogue.Add(item);
        Debug.Log($"Initial Task: Added item: '{item.name}' with item index: {newInd}");
    }

    public ItemSO GetItem(string name)
    {
        int id = -1;
        if (nameToInd.TryGetValue(name, out id))
        {
            return GetItem(id);
        }
        Debug.LogError($"Attempted to get item with name, '{name}', where it does not exist");
        return null;
    }

    public ItemSO GetItem(int id)
    {
        if (id >= 0 && id < itemCatalogue.Count)
        {
            return itemCatalogue[id];
        }
        Debug.LogError($"Attempted to get item with id, '{id}', where it does not exist");
        return null;
    }


    public int GetId(ItemSO item)
    {
        if (nameToInd.TryGetValue(item.name, out int val))
        {
            return val;
        }
        Debug.LogError($"Attemped to find id of '{item.name}' where it does not exist in the catalogue");
        return -1;
    }
    public int GetId(string name)
    {
        if (nameToInd.TryGetValue(name, out int val))
        {
            return val;
        }
        Debug.LogError($"Attemped to find id of '{name}' where it does not exist in the catalogue");
        return -1;
    }

}
