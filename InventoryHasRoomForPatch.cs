using HarmonyLib;
using TheForest.Items.Inventory;
using TheForest.Utils;

[HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.HasRoomFor))]
public static class InventoryHasRoomForPatch
{

    static void Postfix(int itemId, ref bool __result, int amount = 1)
    {
        if (__result) {
            return;
        }
        
        if (!Util.isRunnable())
        {
            return;
        }

        PlayerInventory inv = LocalPlayer.Inventory;

        int max = inv.GetMaxAmountOf(itemId);
        int current = inv.GetItemInstancesOfType(itemId).Count;
        int after = current + amount;

        if (after <= max) {
            __result = true;
        } else {
            __result = false;
        }

    }

}
