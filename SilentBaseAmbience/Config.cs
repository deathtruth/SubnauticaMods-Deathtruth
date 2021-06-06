using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace SilentBaseAmbience
{
    [Menu("Silent Base Ambience")]
    public class MyConfig : ConfigFile
    {
        public MyConfig() : base("config") { }

        [Toggle("Mute Scanner Room", Tooltip = "Mutes the scanner room ambient sound e.g circus music. Requires reload of save game when unmuting"), OnChange(nameof(OnChangeSound))]
        public bool muteScannerRoom;


        [Toggle("Mute Inside Sounds", Tooltip = "Mutes generic base ambient sounds e.g sonar pinging sound. Requires reload of save game when unmuting"), OnChange(nameof(OnChangeSound))]
        public bool muteInsideSounds;

        public void OnChangeSound()
        {
            if (muteScannerRoom && MapRoomFunctionality_Start_Patch.MapRoomFunctionalityList != null)
            {
                foreach (MapRoomFunctionality mrf in MapRoomFunctionality_Start_Patch.MapRoomFunctionalityList)
                {
                    if (mrf != null)
                    {
                        mrf.ambientSound.Stop();
                        mrf.ambientSound = new FMOD_CustomLoopingEmitter();
                    }
                }
            }
            if (muteInsideSounds && Base_Start_Patch.insideSoundsList != null)
            {
                foreach (GameObject insideSounds in Base_Start_Patch.insideSoundsList)
                {
                    if (insideSounds != null)
                    {
                        GameObject.Destroy(insideSounds);
                    }
                }
            }
        }
    }
}