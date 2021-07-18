using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace SilentEating
{
    [Menu("Silent Eating")]
    public class MyConfig : ConfigFile
    {
        public MyConfig() : base("config") { }

        [Toggle("Enable")]
        public bool silentEating = true;

        [Choice("Eating sound:", new string[] { "No Sound", "Pickup Egg", "Pickup Fish", "Pickup Organic", "Pickup Fins" })]
        public string eatingSoundChoice = "No Sound";

        public Dictionary<string, string> eatingSound = new Dictionary<string, string>()
        {
            ["No Sound"] = "",
            ["Pickup Egg"] = "event:/loot/pickup_egg",
            ["Pickup Fish"] = "event:/loot/pickup_fish",
            ["Pickup Organic"] = "event:/loot/pickup_organic",
            ["Pickup Fins"] = "event:/loot/pickup_fins"
        };
    }
}
