using HarmonyLib;
using System;
using TheForest.Items.Inventory;
using TheForest.Utils;

[HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.GetAvailableSpaceForItem))]
public static class InventoryAvailableSpacePatch
{

    static void Postfix(int itemId, ref int __result)
    {
        if (!Util.isRunnable())
        {
            return;
        }

        PlayerInventory inv = LocalPlayer.Inventory;

        int max = inv.GetMaxAmountOf(itemId);
        int current = inv.GetItemInstancesOfType(itemId).Count;

        __result = Math.Max(0, max - current);

    }

}
