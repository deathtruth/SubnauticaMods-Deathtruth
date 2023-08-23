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

        [Toggle("Mute Scanner Room", Tooltip = "Mutes the scanner room ambient sound e.g circus music.")]
        public bool muteScannerRoom = true;

        [Toggle("Mute Inside Sounds", Tooltip = "Mutes generic base ambient sounds e.g sonar pinging sound. (Unmuting requires exit and re-entry of habitat"), OnChange(nameof(OnChangeInsideSounds))]
        public bool muteInsideSounds = true;

        [Toggle("Mute Background Ambience", Tooltip = "Mutes the biome background ambience when inside habitat")]
        public bool muteBackgroundSounds = true;

        [Toggle("Mute Water Filtration Sounds", Tooltip = "Mutes the filtration machine operating sound"), OnChange(nameof(OnChangeWaterFiltrationSounds))]
        public bool muteWaterFiltrationSounds = true;

        [Toggle("Mute GasoPod Sounds Inside", Tooltip = "Mutes gasopod sounds while inside."), OnChange(nameof(OnchangeGasoPodSounds))]
        public bool muteGasoPodSoundsInside = true;

        [Toggle("Mute GasoPod Sounds Outside", Tooltip = "Mutes gasopod sounds while outside."), OnChange(nameof(OnchangeGasoPodSounds))]
        public bool muteGasoPodSoundsOutside = true;

        [Slider("Reefback Sounds Volume Inside", 0f, 1f, DefaultValue = 1f, Id = "reefbackVolumeInside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeReefbackVolumeInside))]
        public float reefbackVolumeInside = 1f;

        [Slider("Reefback Sounds Volume Outside", 0f, 1f, DefaultValue = 1f, Id = "reefbackVolumeOutside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeReefbackVolumeOutside))]
        public float reefbackVolumeOutside = 1f;

       /* [Slider("SandShark Sounds Volume Inside", 0f, 1f, DefaultValue = 1f, Id = "sandSharkVolumeInside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeSandSharkVolumeInside))]
        public float sandSharkVolumeInside = 1f;

        [Slider("SandShark Sounds Volume Outside", 0f, 1f, DefaultValue = 1f, Id = "sandSharkVolumeOutside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeSandSharkVolumeOutside))]
        public float sandSharkVolumeOutside = 1f;*/

        [Slider("Stalker Sounds Volume Inside", 0f, 1f, DefaultValue = 1f, Id = "stalkerVolumeInside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeStalkerVolumeInside))]
        public float stalkerVolumeInside = 1f;

        [Slider("Stalker Sounds Volume Outside", 0f, 1f, DefaultValue = 1f, Id = "stalkerVolumeOutside", Step = 0.01f, Format = "{0:F2}"), OnChange(nameof(OnChangeStalkerVolumeOutside))]
        public float stalkerVolumeOutside = 1f;

        public void OnChangeInsideSounds()
        {
            if (Base_Start_Patch.insideSoundsList.Any())
            {
                foreach (GameObject obj in Base_Start_Patch.insideSoundsList)
                {
                    if (obj)
                    {
                        foreach (FMOD_StudioEventEmitter sound in obj.GetAllComponentsInChildren<FMOD_StudioEventEmitter>())
                        {
                            if (muteInsideSounds)
                            {
                                sound.Stop(true);
                            }
                        }
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
            if (Player_Start_Patch.playerObj != null && Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeReefbackVolume(e.Value);
            }
        }

        public void OnChangeReefbackVolumeOutside(SliderChangedEventArgs e)
        {
            if (Player_Start_Patch.playerObj != null && !Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeReefbackVolume(e.Value);
            }
        }
/*
        public void OnChangeSandSharkVolumeInside(SliderChangedEventArgs e)
        {
            if (Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeSandSharkVolume(e.Value);
            }
        }

        public void OnChangeSandSharkVolumeOutside(SliderChangedEventArgs e)
        {
            if (!Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeSandSharkVolume(e.Value);
            }
        }*/

        public void OnChangeStalkerVolumeInside(SliderChangedEventArgs e)
        {
            if (Player_Start_Patch.playerObj != null && Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeStalkerVolume(e.Value);
            }
        }

        public void OnChangeStalkerVolumeOutside(SliderChangedEventArgs e)
        {
            if (Player_Start_Patch.playerObj != null && !Player_Start_Patch.playerObj.IsInBase())
            {
                ChangeStalkerVolume(e.Value);
            }
        }

        public void UpdateIsUnderwater()
        {
            if (Player_Start_Patch.playerObj != null)
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
                if (Player_Start_Patch.playerObj.IsInBase() && !Player_UpdateIsUnderwater_Patch.playerInside) //entering
                {
                    //On inital load the creatures load later than the player so we need to keep trying
                    if (ChangeReefbackVolume(reefbackVolumeInside) /*&& ChangeSandSharkVolume(sandSharkVolumeInside)*/ && ChangeStalkerVolume(stalkerVolumeInside) && OnchangeGasoPodSounds())
                    {
                        Player_UpdateIsUnderwater_Patch.playerInside = true;
                    }
                    OnChangeInsideSounds();
                }
                else if (!Player_Start_Patch.playerObj.IsInBase() && Player_UpdateIsUnderwater_Patch.playerInside) //exiting
                {
                    if (ChangeReefbackVolume(reefbackVolumeOutside) /*&& ChangeSandSharkVolume(sandSharkVolumeOutside)*/ && ChangeStalkerVolume(stalkerVolumeOutside) && OnchangeGasoPodSounds())
                    {
                        Player_UpdateIsUnderwater_Patch.playerInside = false;
                    }
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

        /*public bool ChangeSandSharkVolume(float sandSharkVolume)
        {
            if (Creature_Start_Patch.sandSharkList.Any())
            {
                foreach (SandShark ss in Creature_Start_Patch.sandSharkList)
                {
                    if (ss)
                    {
                        if (ss.liveMixin.IsAlive())
                        {
                            foreach (FMOD_StudioEventEmitter sound in ss.GetComponents<FMOD_StudioEventEmitter>())
                            {
                                sound.evt.setVolume(sandSharkVolume);
                                if (sandSharkVolume <= 0f)
                                {
                                    sound.Stop();
                                }
                                else if (!sound.GetIsPlaying())
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
        }*/

        public bool ChangeStalkerVolume(float stalkerVolume)
        {

            if (Creature_Start_Patch.stalkerList.Any())
            {
                foreach (Stalker stalker in Creature_Start_Patch.stalkerList)
                {
                    if (stalker)
                    {
                        if (stalker.liveMixin.IsAlive())
                        {
                            foreach (FMOD_CustomEmitter sound in stalker.GetComponents<FMOD_CustomEmitter>())
                            {
                                sound.evt.setVolume(stalkerVolume);
                                if (stalkerVolume <= 0f)
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

        public bool OnchangeGasoPodSounds()
        {
            if (Creature_Start_Patch.gasoPodList.Any())
            {
                foreach (GasoPod gp in Creature_Start_Patch.gasoPodList)
                {
                    if (gp)
                    {
                        if (gp.liveMixin.IsAlive())
                        {
                            foreach (FMOD_CustomLoopingEmitter sound in gp.GetAllComponentsInChildren<FMOD_CustomLoopingEmitter>())
                            {
                                if ((Main.Config.muteGasoPodSoundsOutside && !Player_Start_Patch.playerObj.IsInBase()) || (Main.Config.muteGasoPodSoundsInside && Player_Start_Patch.playerObj.IsInBase()))
                                {
                                    sound.Stop();
                                }
                                else
                                {
                                    sound.Play();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}