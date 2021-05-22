using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace AdjustableFireExtinguisher
{
    [Menu("Adjustable Fire Extinguisher")]
    public class MyConfig: ConfigFile
    {
        public MyConfig() : base("config") { }

       /* [Toggle("Unlimited Fuel", Tooltip = "Enable/Disable Unlimited Fuel")]
        public bool unlimitedFuel = true;*/

        [Keybind("Keybind To Adjust Regulator", Tooltip = "When pressed you can adjust the fire extinguisher pressure regulator using mouse wheel"), /*OnChange(nameof(KeyBindChanged))*/]
        public KeyCode adjustKeyCode = KeyCode.F;

        [Toggle("Mute Regulator Adjustment Sound", Tooltip = "Mute the sound which is played when adjusting the regulator"), /*OnChange(nameof(MuteChanged))*/]
        public bool muteSound = false;

        public int feForce = 5;

    }
}