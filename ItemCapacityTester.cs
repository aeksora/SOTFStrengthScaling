using Sons.Items.Core;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
using static Util;

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

        if (_frameCount < 80) {
            //Plugin.Instance.Log.LogInfo("Skipped check");
            return;
        }

        Plugin.Instance.Log.LogInfo("Entered check");

        if (_frameCount >= 80) {
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

            if (!LocalPlayer.IsInWorld) {
                Plugin.Instance.Log.LogInfo("Player is not yet in world");
                return;
            }

            Vitals vitals = LocalPlayer.Vitals;

            if (vitals == null)
            {
                Plugin.Instance.Log.LogInfo("LocalPlayer.Vitals was null");
                return;
            }

            int strength = vitals.CurrentStrengthLevel;

            // Vitals not fully loaded
            if (strength <= 1)
            {
                Plugin.Instance.Log.LogInfo("Strength was not fully loaded");
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
            int maxCapacity = 100;

            _originalItemCapacities.Add(item.Id, item._maxAmount);

            Plugin.Instance.Log.LogInfo($"Initializing {item.Name} with capacity of {maxCapacity}");

            item._maxAmount = maxCapacity;
        }

        _initialized = true;

        Plugin.Instance.Log.LogInfo($"Done initializing");
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
            int originalCapacity = _originalItemCapacities[item.Id];
            Plugin.Instance.Log.LogInfo($"Received original capacity for {item.Name}: {originalCapacity}");
            CapacityCalculation c = CalculateMaxCapacity(vitals.CurrentStrengthLevel, originalCapacity);

            int capacityBeforeModification = item._maxAmount;

            if (originalCapacity > 1) {
                item._maxAmount = c.actualValue;
            }
            else if (originalCapacity <= 1) {
                item._maxAmount = originalCapacity;
            }

            Plugin.Instance.Log.LogInfo($"{item.Name}: {{previous: {capacityBeforeModification}, now: {item._maxAmount} ({c.preciseValue})}}");
        }

    }

    private void LogStats(Vitals vitals)
    {
        var strength = vitals.CurrentStrengthLevel;
        var strengthProgress = vitals.GetStrength();

        Plugin.Instance.Log.LogInfo($"Strength: {strength}, Strength Progress: {strengthProgress}");
    }
}
