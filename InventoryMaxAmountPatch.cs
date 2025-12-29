using HarmonyLib;
using TheForest.Items.Inventory;
using TheForest.Utils;

[HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.GetMaxAmountOf))]
public static class InventoryMaxAmountPatch {

    static void Postfix(int itemId, ref int __result) {
        if (!Util.isRunnable()) {
            return;
        }

        Vitals vitals = LocalPlayer.Vitals;
        int strength = vitals.CurrentStrengthLevel;

        float multiplier = Util.CalculateMultiplier(strength);
        int scaled = (int) (__result * multiplier);

        __result = scaled;
    }

}
