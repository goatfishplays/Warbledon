using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUICountElement : MonoBehaviour
{
    [SerializeField] protected ItemSO _item;
    public ItemSO item => _item;
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI countText;

    public virtual void SetCount(int amt = 1)
    {
        if (item.consumeOnUse)
        {
            countText.text = amt.ToString();
        }
        else
        {
            countText.text = "";
        }
        icon.color = amt > 0 ? Color.white : Color.gray;
    }

    public virtual void SetItem(ItemSO item, int amt = 1)
    {
        this._item = item;
        icon.sprite = item.sprite;
        SetCount(amt);
    }
    public virtual void ClearItem()
    {
        this._item = null;
        icon.color = Color.clear;
        if (countText != null)
        {
            countText.text = "";
        }
    }
}
