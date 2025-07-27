using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIVerboseElement : ItemUICountElement
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    public override void SetItem(ItemSO item, int amt = 1)
    {
        base.SetItem(item, amt);
        if (nameText != null)
        {
            nameText.text = item.name;
        }
        if (descText != null)
        {
            descText.text = item.description;
        }
    }

    public override void ClearItem()
    {
        base.ClearItem();
        if (nameText != null)
        {
            nameText.text = "";
        }
        if (descText != null)
        {
            descText.text = "";
        }
    }
}
