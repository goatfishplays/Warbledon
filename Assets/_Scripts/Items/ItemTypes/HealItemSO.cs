using UnityEngine;


[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/Heal")]
public class HealItemType : ItemSO
{
    public int healAmount;

    // public TestItemType(string itemName, string description = "no desc", bool consumeOnUse = true, Sprite sprite = null, int healAmount = 5) : base(itemName, description, consumeOnUse, sprite)
    // {
    //     this.healAmount = healAmount;
    // }

    public override bool AttemptUse(Entity user)
    {
        Debug.Log($"{user.gameObject.name} used {name} to change health by {healAmount}");
        user.entityHealth.ChangeHealth(false, healAmount, 0f, true);
        return true;
    }
}
