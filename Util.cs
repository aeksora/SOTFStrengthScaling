using Sons.Items.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;

public static class Util {
    public static bool isRunnable()
    {
        if (ItemDatabaseManager._instance == null)
        {
            return false;
        }

        if (LocalPlayer.GameObject == null)
        {
            return false;
        }

        Vitals vitals = LocalPlayer.Vitals;

        if (vitals == null)
        {
            return false;
        }

        int strength = vitals.CurrentStrengthLevel;

        // Vitals not fully loaded
        if (strength <= 1)
        {
            return false;
        }

        return true;
    }

    public static CapacityCalculation CalculateMaxCapacity(int strength, int maxAmount) {
        float multiplier = CalculateMultiplier(strength);

        //Plugin.Instance.Log.LogInfo($"Calculated multiplier {multiplier} at strength level {strength}");

        CapacityCalculation c = new();
        c.multiplier = multiplier;
        c.preciseValue = maxAmount * multiplier;
        c.actualValue = (int)c.preciseValue;

        return c;
    }

    public static float CalculateMultiplier(int strength) {
        return 1.0f + strength * (Plugin.percentageIncreasePerStrengthLevel / 100.0f);
    }

    public struct CapacityCalculation
    {
        public int actualValue;
        public float preciseValue;
        public float multiplier;
    }
}
