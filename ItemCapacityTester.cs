using Sons.Items.Core;
using UnityEngine;
using TheForest.Utils;
using static Util;
using TheForest.Items.Inventory;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

public class ItemCapacityTester : MonoBehaviour
{

    private bool _ran;
    private bool _initialized = false;
    private float _lastStrengthProgress = 0.0f;
    private int _lastStrengthLevel = 1;
    private Dictionary<int, int> _originalItemCapacities = new Dictionary<int, int>();
    private int _frameCount = 0;

    private void Update()
    {
        _frameCount++;

        if (_frameCount < 8) {
            //Plugin.Instance.Log.LogInfo("Skipped check");
            return;
        }

        //Plugin.Instance.Log.LogInfo("Entered check");

        if (_frameCount >= 5) {
            _frameCount = 0;
        }

        if (!_ran)
        {
            if (ItemDatabaseManager._instance == null)
            {
                return;
            }

            if (!_initialized) {
                Plugin.Instance.Log.LogInfo("Item Database Manager found. Initalizing item capacities...");
                Init();
            }

            if (LocalPlayer.GameObject == null)
            {
                return;
            }

            Vitals vitals = LocalPlayer.Vitals;

            if (vitals == null)
            {
                return;
            }

            int strength = vitals.CurrentStrengthLevel;

            // Vitals not fully loaded
            if (strength <= 1)
            {
                return;
            }

            _ran = true;


            Plugin.Instance.Log.LogInfo("Vitals found. Logging strength and item values...");
            LogItems(vitals);
            _lastStrengthProgress = vitals.GetStrength();
            _lastStrengthLevel = vitals.CurrentStrengthLevel;
            LogStats(vitals);

            //this.enabled = false;
        }
        else if (_ran)
        {

            if (!isRunnable()) { return; }

            Vitals vitals = LocalPlayer.Vitals;
            float strengthProgress = vitals.GetStrength();
            int strength = vitals.CurrentStrengthLevel;

            if (strengthProgress != _lastStrengthProgress)
            {
                _lastStrengthProgress = vitals.GetStrength();
                LogStats(vitals);
            }

            if (strength != _lastStrengthLevel)
            {
                _lastStrengthLevel = strength;
                LogItems(vitals);
            }

            //Plugin.Instance.Log.LogInfo("Item Database Manager found. Logging item capacities...");
            //LogItems();

        }
    }

    private void Init() {
        Plugin.Instance.Log.LogInfo("Initializing item capacities");

        foreach (ItemData item in ItemDatabaseManager._instance._itemDataList)
        {
            int maxCapacity = 1000;

            _originalItemCapacities.Add(item.Id, item.MaxAmount);

            Plugin.Instance.Log.LogInfo($"Initializing {item.Name} with capacity of {maxCapacity}");

            item.MaxAmount = maxCapacity;
        }

        _initialized = true;
    }

    private void LogItems(Vitals vitals)
    {
        Plugin.Instance.Log.LogInfo("Entered LogValues method.");


        string[] allowed = {
            "AirCanister",
            "AloeVera",
            "Batteries",
            "Bone",
            "CreepyArmour",
            "Stick",
            "TurtleShell"
        };


        foreach (ItemData item in ItemDatabaseManager._instance._itemDataList)
        {

            //for (int i = 0; i < allowed.Length; i++)
            //{
            //    if (item._name == allowed[i])
            //    {
            //        int originalCapacity = _originalItemCapacities[item.Id];
            //        Plugin.Instance.Log.LogInfo($"Received original capacity for {item.Name}: {originalCapacity}");
            //        CapacityCalculation c = CalculateMaxCapacity(vitals.CurrentStrengthLevel, originalCapacity);

            //        Plugin.Instance.Log.LogInfo($"{item.Name}: {{previous: {item.MaxAmount}, now: {c.actualValue} ({c.preciseValue})}}");

            //        //PlayerInventory inv = LocalPlayer.Inventory;
            //        //int maxAmountFromInventory = inv.GetMaxAmountOf(item.Id);
            //        //bool hasRoomFor = inv.HasRoomFor(item.Id, 10);
            //        //int getAvailableSpace = inv.GetAvailableSpaceForItem(item.Id);
            //        //int amount = inv.GetItemInstancesOfType(item.Id).Count;

            //        //Plugin.Instance.Log.LogInfo($"{item.Name}: {{GetMaxAmountOf: {amount}/{maxAmountFromInventory}, HasRoomFor: {hasRoomFor}, GetAvailableSpaceForItem: {getAvailableSpace}}}");

            //        item.MaxAmount = c.actualValue;

            //        //Plugin.Instance.Log.LogInfo(item._name + ": " + item._maxAmount);
            //        break;
            //    }
            //}

            int originalCapacity = _originalItemCapacities[item.Id];
            Plugin.Instance.Log.LogInfo($"Received original capacity for {item.Name}: {originalCapacity}");
            CapacityCalculation c = CalculateMaxCapacity(vitals.CurrentStrengthLevel, originalCapacity);

            int capacityBeforeModification = item.MaxAmount;

            if (originalCapacity > 1) {
                item.MaxAmount = c.actualValue;
            }
            else if (originalCapacity <= 1) {
                item.MaxAmount = originalCapacity;
            }

            Plugin.Instance.Log.LogInfo($"{item.Name}: {{previous: {capacityBeforeModification}, now: {item.MaxAmount} ({c.preciseValue})}}");
        }

    }

    private void LogStats(Vitals vitals)
    {
        var strength = vitals.CurrentStrengthLevel;
        var strengthProgress = vitals.GetStrength();

        Plugin.Instance.Log.LogInfo($"Strength: {strength}, Strength Progress: {strengthProgress}");
    }
}
