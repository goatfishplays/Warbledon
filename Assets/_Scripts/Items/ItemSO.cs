using UnityEngine;

// [System.Serializable]
[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{

    /// <summary>
    /// Will control where it gets sorted
    /// </summary>
    public string tab = ItemManager.ITEMS_TAB;
    public string displayName = "No Name";
    public string description = "No description";
    public string useText = "No use text"; // TODO Add something that interprets this with codes like {User}, also might be better to have an alternate version for player using
    public bool canUseInBattle = true;
    public bool canUseInOverworld = true;
    public bool consumeOnUse = true;
    public bool dropable = true;
    public bool equipable = false;
    public int sellValue = 0;
    // public float useTime = 0f;
    public int maxCount = 27;
    // public GameObject dropPrefab;
    public Sprite sprite;

    // // Only need to use this if we decide we want to load resources at runtime rather than at editortime(is it called editor time? idrk, maybe it is compile time for this too), would allow for resource packs lmaoooo we so don't need it do we...
    // public ItemSO(string itemName, string description, bool consumeOnUse, Sprite sprite)
    // {
    //     this.name = itemName;
    //     this.description = description;
    //     this.consumeOnUse = consumeOnUse;
    //     this.sprite = sprite;
    // }

    public virtual bool AttemptUse(Entity user)
    {
        Debug.LogWarning($"{user.gameObject.name} attempted to use '{name}' without item implementation");
        return false;
    }
}
