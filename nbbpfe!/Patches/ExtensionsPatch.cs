﻿using HarmonyLib;
using MTM101BaldAPI;
using nbppfe.BasicClasses.Extensions;

namespace nbppfe.Patches
{
    [HarmonyPatch(typeof(Window), "Start")]
    internal static class WindowPatch
    {
        [HarmonyPostfix]
        internal static void AddExtension(Window __instance) =>
            __instance.gameObject.GetOrAddComponent<WindowExtension>();

    }

    [HarmonyPatch(typeof(StandardDoor), "Start")]
    internal static class DoorPatch
    {
        [HarmonyPostfix]
        internal static void AddExtension(StandardDoor __instance) =>
            __instance.gameObject.AddComponent<DoorExtension>();

    }

    [HarmonyPatch(typeof(NPC), nameof(NPC.Initialize))]
    internal static class NPCPatch
    {
        [HarmonyPostfix]
        public  static void AddExtension(NPC __instance)
        {
            __instance.Navigator.passableObstacles.Remove(PassableObstacle.LockedDoor);
            __instance.gameObject.GetOrAddComponent<NpcExtension>();
        }

    }
}
