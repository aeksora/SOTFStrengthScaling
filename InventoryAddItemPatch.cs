using HarmonyLib;
using Sons.Inventory;
using TheForest.Items.Inventory;
using TheForest.Utils;

//[HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.AddItem))]
[HarmonyPatch(typeof(PlayerInventory))]
public static class InventoryAddItemPatch
{
    //[HarmonyPatch(nameof(PlayerInventory.AddItem))]
    //[HarmonyPrefix]
    //static void Prefix_All(object[] __args)
    //{
    //    Plugin.Instance.Log.LogInfo(
    //        "AddItem called with args: " + string.Join(", ", __args)
    //    );
    //}

    [HarmonyPatch(
        nameof(PlayerInventory.AddItem),
        new[] {
            typeof(int),
            typeof(int),
            typeof(bool),
            typeof(bool),
            typeof(ItemInstance)
        } 
    )]
    [HarmonyPrefix]
    static bool Prefix(
        int itemId,
        ref int amount,
        bool preventAutoEquip,
        bool wasCrafted,
        ItemInstance itemInstance,
        ref bool __result
    )
    {

        if (!Util.isRunnable())
        {
            return true;
        }


        PlayerInventory inv = LocalPlayer.Inventory;

        int max = inv.GetMaxAmountOf(itemId);
        int current = inv.GetItemInstancesOfType(itemId).Count;

        int spaceLeft = max - current;
        Plugin.Instance.Log.LogInfo($"AddItem intercepted: item {itemId}, amount {amount}, spaceLeft={max}-{current}={spaceLeft}");
        if (spaceLeft <= 0)
        {
            __result = false;
            return false;
        }

        if (amount > spaceLeft)
        {
            amount = spaceLeft;
        }

        return true;

    }

}

