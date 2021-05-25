using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace SilentWalking
{
    [Menu("Silent Walking")]
    public class MyConfig : ConfigFile
    {
        public MyConfig() : base("config") { }

        [Toggle("Silent Walking on Metal"), OnChange(nameof(OnChangeMute))]
        public bool silentWalkingOnMetal;

        [Toggle("Silent Walking on Land"), OnChange(nameof(OnChangeMute))]
        public bool silentWalkingOnLand;

        [Toggle("Silent Walking on Precursor"), OnChange(nameof(OnChangeMute))]
        public bool silentWalkingOnPrecursor;

        public void OnChangeMute()
        {
            if (FootstepSounds_Start_Patch.thisFootstepSounds != null)
            {
                if (silentWalkingOnMetal)
                {
                    FootstepSounds_Start_Patch.thisFootstepSounds.metalSound = null;
                }
                else
                {
                    FootstepSounds_Start_Patch.thisFootstepSounds.metalSound = FootstepSounds_Start_Patch.metalSound;
                }

                if (silentWalkingOnLand)
                {
                    FootstepSounds_Start_Patch.thisFootstepSounds.landSound = null;
                }
                else
                {
                    FootstepSounds_Start_Patch.thisFootstepSounds.landSound = FootstepSounds_Start_Patch.landSound;
                }

                if (silentWalkingOnPrecursor)
                {
                    FootstepSounds_Start_Patch.thisFootstepSounds.precursorInteriorSound = null;
                }
                else
                {
                    FootstepSounds_Start_Patch.thisFootstepSounds.precursorInteriorSound = FootstepSounds_Start_Patch.precursorInteriorSound;
                }
            }
        }
    }
}