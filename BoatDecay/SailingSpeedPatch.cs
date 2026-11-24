using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace BoatDecay;

internal static class SailingSpeedPatch
{
    private const string StatCode = "sailingSpeed";

    internal static void Apply(Harmony harmony, ILogger logger)
    {
        MethodInfo methodInfo = AccessTools.DeclaredMethod(typeof (EntityBoat), "SeatsToMotion");
        if (methodInfo.Equals(null))
            logger.Error("Could not find EntityBoat.SeatsToMotion to patch; the sailing speed stat will not work.");
        else
            harmony.Patch(methodInfo, null, new HarmonyMethod(typeof (SailingSpeedPatch), "SeatsToMotionPostfix"));
    }

    private static void SeatsToMotionPostfix(EntityBoat __instance, Vec2d __result)
    {
        if (__result.Equals(null))
            return;
        EntityStats stats = __instance.GetBehavior<EntityBehaviorSeatable>()?.Controller?.Stats;
        float val1 = (stats != null ? stats.GetBlended("sailingSpeed") : 1f) * SpeedMultiplier(__instance);
        if (val1 == 1.0)
            return;
        __result.X *= Math.Max(val1, 0.0f);
    }
    
    internal static float SpeedMultiplier(Entity boat)
    {
        float durability = boat.WatchedAttributes.GetFloat("boatdecay:durability", 1.0f);
        return (float) (1.0 - 0.8 * (1 - durability));
    }
}