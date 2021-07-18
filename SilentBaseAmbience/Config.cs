using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using System.Linq;
using UnityEngine;

namespace SilentBaseAmbience
{
    [Menu("Silent Base Ambience")]
    public class MyConfig : ConfigFile
    {
        public MyConfig() : base("config") { }

        [Toggle("Mute Scanner Room", Tooltip = "Mutes the scanner room ambient sound e.g circus music."), OnChange(nameof(OnChangeScannerRoom))]
        public bool muteScannerRoom = true;


        [Toggle("Mute Inside Sounds", Tooltip = "Mutes generic base ambient sounds e.g sonar pinging sound. Requires reload of save game when unmuting"), OnChange(nameof(OnChangeInsideSounds))]
        public bool muteInsideSounds = true;

        [Toggle("Mute Background Ambience", Tooltip = "Mutes the biome background ambience when inside habitat"), OnChange(nameof(OnChangeBackgroundSounds))]
        public bool muteBackgroundSounds = true;

        [Toggle("Mute Water Filtration Sounds", Tooltip = "Mutes the filtration machine operating sound"), OnChange(nameof(OnChangeWaterFiltrationSounds))]
        public bool muteWaterFiltrationSounds = true;

        public void OnChangeScannerRoom()
        {
            if (MapRoomFunctionality_Start_Patch.MapRoomFunctionalityList.Any())
            {
                foreach (MapRoomFunctionality mrf in MapRoomFunctionality_Start_Patch.MapRoomFunctionalityList)
                {
                    if (muteScannerRoom)
                    {
                        mrf.ambientSound.Stop();
                    }
                    else
                    {
                        mrf.ambientSound.Play();
                    }
                }
            }
        }
        public void OnChangeInsideSounds()
        {
            if (Base_Start_Patch.InsideSoundsList.Any())
            {
                foreach (GameObject obj in Base_Start_Patch.InsideSoundsList)
                {
                    if (muteInsideSounds)
                    {
                        GameObject.Destroy(obj);
                    }
                }
            }
        }
        public void OnChangeWaterFiltrationSounds()
        {
            if (FiltrationMachine_Start_Patch.FiltrationMachineList.Any())
            {
                foreach (FiltrationMachine fm in FiltrationMachine_Start_Patch.FiltrationMachineList)
                {
                    if (fm)
                    {
                        if (muteWaterFiltrationSounds)
                        {
                            fm.workSound.Stop();
                        }
                        else
                        {
                            if (fm.working)
                            {
                                fm.workSound.Play();
                            }
                        }
                    }
                }
            }
        }
        public void OnChangeBackgroundSounds()
        {
            if (muteBackgroundSounds && Player_Start_Patch.playerObj.IsInBase())
            {
                Player_Start_Patch.backgroundObj.SetActive(false);
            }
            else
            {
                Player_Start_Patch.backgroundObj.SetActive(true);
            }
        }
    }
}