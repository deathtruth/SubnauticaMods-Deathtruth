using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;

namespace NoRebreatherSounds
{
    [QModCore]
    public class Main
    {
        static readonly Assembly myAssembly = Assembly.GetExecutingAssembly();

        [QModPatch]
        public static void Load()
        {
            Logger.Log(Logger.Level.Debug, "Deathtruth No Rebreather Sounds patching");

            Harmony.CreateAndPatchAll(myAssembly, $"Deathtruth_{myAssembly.GetName().Name}");

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}