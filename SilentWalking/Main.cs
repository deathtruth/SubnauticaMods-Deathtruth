using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace SilentWalking
{
    [QModCore]
    public class Main
    {
        static readonly Assembly myAssembly = Assembly.GetExecutingAssembly();
        internal static MyConfig Config { get; set; }

        [QModPatch]
        public static void Load()
        {
            Logger.Log(Logger.Level.Debug, "Deathtruth Silent Walking patching");

            Harmony.CreateAndPatchAll(myAssembly, $"Deathtruth_{myAssembly.GetName().Name}");
            Config = OptionsPanelHandler.Main.RegisterModOptions<MyConfig>();

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}