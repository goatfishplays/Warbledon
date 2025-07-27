using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIInventoryController : MonoBehaviour
{
    public Inventory inventory;
    [Header("Top Bar")]
    [SerializeField] public int curTab = 0;
    [SerializeField] private TextMeshProUGUI tabText;

    [Header("Item Icons")]
    [SerializeField] private RectTransform itemIconsHolder;
    [SerializeField] private GridLayoutGroup itemIconsHolderGLG;
    [SerializeField] private GameObject itemIconPrefab;
    private List<ItemUICountElement> itemIcons = new List<ItemUICountElement>();

    [Header("Selected Item")]
    [SerializeField] private ItemSO selectedItem;
    [SerializeField] private ItemUICountElement selectedItemIcon;
    [SerializeField] private ItemUIVerboseElement selectedDisplay;
    [SerializeField] private GameObject dropButton;
    [SerializeField] private GameObject equipButton;


    // public void Equip()
    // {
    //     inventory.Equip(selectedItem);
    // }
    public void Drop()
    {
        inventory.Drop(selectedItem);
        int amtLeft = inventory.GetItemCount(selectedItem);
        // Debug.Log(amtLeft);
        if (amtLeft > 0)
        {
            selectedDisplay.SetCount(amtLeft);
            selectedItemIcon.SetCount(amtLeft);
        }
        else
        {
            UpdateScreen(inventory.GetTabItems(curTab), inventory.GetTabName(curTab));
        }
    }

    void Start()
    {
        foreach (ItemUICountElement ice in itemIconsHolder.GetComponentsInChildren<ItemUICountElement>())
        {
            itemIcons.Add(ice);
            ice.GetComponent<ItemIconSelectionBehavior>().controller = this;
        }
        gameObject.SetActive(false);
    }

    public void SetSelected(ItemUICountElement itemIcon)
    {
        selectedItem = itemIcon.item;
        selectedItemIcon = itemIcon;
        selectedDisplay.SetItem(selectedItem, inventory.GetItemCount(selectedItem));
        dropButton.SetActive(selectedItem.dropable);
        equipButton.SetActive(selectedItem.equipable);
    }

    public void ResetSelected()
    {
        selectedItem = null;
        selectedItemIcon = null;
        selectedDisplay.ClearItem();
        dropButton.SetActive(false);
        equipButton.SetActive(false);
    }

    public void FetchNextScreen()
    {
        curTab++;

        UpdateScreen(inventory.GetTabItems(curTab), inventory.GetTabName(curTab));
    }

    public void FetchPrevScreen()
    {
        curTab--;
        UpdateScreen(inventory.GetTabItems(curTab), inventory.GetTabName(curTab));
    }

    public void OpenInventory(int tab = 0)
    {
        curTab = tab;
        UpdateScreen(inventory.GetTabItems(tab), inventory.GetTabName(tab));
    }

    public void UpdateScreen(SortedList<int, ItemSO> tab, string tabName, bool wipeSelected = true)
    {
        // reset screen
        foreach (ItemUICountElement item in itemIcons)
        {
            item.gameObject.SetActive(false);
        }
        if (wipeSelected)
        {
            ResetSelected();
        }
        else
        {
            SetSelected(selectedItemIcon);
        }

        // add extra icons if needed
        while (tab.Count > itemIcons.Count)
        {
            Debug.Log("New Item Icon Slot Added");
            GameObject newIcon = Instantiate(itemIconPrefab, itemIconsHolder);
            itemIcons.Add(newIcon.GetComponent<ItemUICountElement>());
            newIcon.GetComponent<ItemIconSelectionBehavior>().controller = this;
        }

        // set new icons 
        int i = 0;
        foreach (ItemSO item in tab.Values)
        {
            itemIcons[i].SetItem(item, inventory.GetItemCount(item));
            itemIcons[i].gameObject.SetActive(true);
            i++;
        }

        // set tab name
        tabText.text = tabName;

        // resize holder
        // itemIconsHolderGLG.SetLayoutVertical();
        // itemIconsHolderGLG.CalculateLayoutInputVertical(); 
        StartCoroutine(ResizeHolder());
    }

    private IEnumerator ResizeHolder()
    {
        yield return new WaitForEndOfFrame();
        // Debug.Log(itemIconsHolderGLG.flexibleHeight); 
        // Debug.Log(itemIconsHolderGLG.preferredHeight);
        // Debug.Log(itemIconsHolderGLG.minHeight);
        itemIconsHolder.sizeDelta = new Vector2(itemIconsHolder.sizeDelta.x, itemIconsHolderGLG.preferredHeight);


    }





}
