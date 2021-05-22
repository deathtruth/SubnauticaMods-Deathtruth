using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace AdjustableFireExtinguisher
{
    [QModCore]
    public class AdjustableFireExtinguisher
    {
        internal static MyConfig Config { get; set; }
        static readonly Assembly myAssembly = Assembly.GetExecutingAssembly();

        [QModPatch]
        public static void Load()
        {
            Logger.Log(Logger.Level.Debug, "Deathtruth Adjustable Fire Extinguisher start patching");

            Harmony.CreateAndPatchAll(myAssembly, $"Deathtruth_{myAssembly.GetName().Name}");

            Config = OptionsPanelHandler.Main.RegisterModOptions<MyConfig>();

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}