using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
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

        [Toggle("Mute Background Ambience", Tooltip = "Mutes the biome background ambience when inside habitat")]
        public bool muteBackgroundSounds = true;

        [Toggle("Mute Water Filtration Sounds", Tooltip = "Mutes the filtration machine operating sound"), OnChange(nameof(OnChangeWaterFiltrationSounds))]
        public bool muteWaterFiltrationSounds = true;

        [Slider("Reefback Sounds Volume Inside", 0f, 1f, DefaultValue = 1f, Id = "reefbackVolumeInside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeReefbackVolumeInside))]
        public float reefbackVolumeInside = 1f;

        [Slider("Reefback Sounds Volume Outside", 0f, 1f, DefaultValue = 1f, Id = "reefbackVolumeOutside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeReefbackVolumeOutside))]
        public float reefbackVolumeOutside = 1f;

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

        public void OnChangeReefbackVolumeInside(SliderChangedEventArgs e)
        {
            if (Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeReefbackVolume(e.Value);
            }
        }

        public void OnChangeReefbackVolumeOutside(SliderChangedEventArgs e)
        {
            if (!Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeReefbackVolume(e.Value);
            }
        }

        public void UpdateIsUnderwater()
        {
            if (muteBackgroundSounds && Player_Start_Patch.playerObj.IsInBase())
            {
                Player_Start_Patch.backgroundObj.SetActive(false);
            }
            else
            {
                Player_Start_Patch.backgroundObj.SetActive(true);
            }

            //Store where the player is to only process on changes, this avoids unnecessary processing
            if (Player_Start_Patch.playerObj.IsInBase() && !Player_UpdateIsUnderwater_Patch.playerInside)
            {
                //On inital load the reefbacks load much later than the player so we need to keep trying
                if (ChangeReefbackVolume(reefbackVolumeInside))
                {
                    Player_UpdateIsUnderwater_Patch.playerInside = true;
                }
            }
            else if(!Player_Start_Patch.playerObj.IsInBase() && Player_UpdateIsUnderwater_Patch.playerInside)
            {
                
                if (ChangeReefbackVolume(reefbackVolumeOutside))
                {
                    Player_UpdateIsUnderwater_Patch.playerInside = false;
                }
            }
        }

        public bool ChangeReefbackVolume(float reefbackVolume)
        {
            
            if (Creature_Start_Patch.reefbackList.Any())
            {
                foreach (Reefback rb in Creature_Start_Patch.reefbackList)
                {
                    if (rb)
                    {
                        if (rb.liveMixin.IsAlive())
                        {
                            foreach (FMOD_CustomLoopingEmitter sound in rb.GetComponents<FMOD_CustomLoopingEmitter>())
                            {
                                sound.GetEventInstance().setVolume(reefbackVolume);
                                if (reefbackVolume <= 0f)
                                {
                                    sound.Stop();
                                }
                                else if (!sound.playing)
                                {
                                    sound.Start();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}