using HarmonyLib;
using Sons.Gameplay;
using Sons.Items.Core;
using TheForest.Items.Inventory;
using TheForest.Utils;

[HarmonyPatch(typeof(PickUp), nameof(PickUp.CanPlayerGather))]
public static class PickUpPatch
{

    static void Postfix(PickUp __instance, ref bool __result)
    {

        if (!Util.isRunnable())
        {
            return;
        }

        ItemData item = __instance.GetItemData();

        Plugin.Instance.Log.LogInfo($"CanPlayerGather: {__result} on Item: {item.Name}");
    }

}