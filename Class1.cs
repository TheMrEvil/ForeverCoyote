using System;
using MelonLoader;
using HarmonyLib;
using System.Reflection;

namespace ForeverCoyote
{
    public class Class1 : MelonMod
    {
        public override void OnInitializeMelon()
        {
            var harmony = new HarmonyLib.Harmony("forevercoyote.airjumps.patch");
            harmony.Patch(
                AccessTools.Method("CMF.AdvancedWalkerController:OnGroundContactRegained", (Type[])null, (Type[])null),
                postfix: new HarmonyMethod(typeof(Class1), nameof(OnGroundContactRegained_Postfix))
            );
            harmony.Patch(
                AccessTools.Method("CMF.AdvancedWalkerController:OnJumpStart", new Type[] { typeof(bool) }),
                postfix: new HarmonyMethod(typeof(Class1), nameof(OnJumpStart_Postfix))
            );
        }

        // Postfix method to set airJumpsUsed to -1
        public static void OnGroundContactRegained_Postfix(object __instance)
        {
            var field = __instance.GetType().GetField("airJumpsUsed", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(__instance, -1);
            }
        }

        // Postfix for OnJumpStart: increment airJumpsUsed if not an air jump
        public static void OnJumpStart_Postfix(object __instance, bool isAirJump)
        {
            if (!isAirJump)
            {
                var field = __instance.GetType().GetField("airJumpsUsed", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    int current = (int)field.GetValue(__instance);
                    field.SetValue(__instance, current + 1);
                }
            }
        }
    }
}
