using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace YATA.Core.Audio
{
    public class SoundFX
    {
        private static MediaPlayer SoundFXSystem = new MediaPlayer { AudioCategory = MediaPlayerAudioCategory.SoundEffects, Volume = 0.1, IsLoopingEnabled = false };
        private static MediaPlayer FinishedSoundFXSystem = new MediaPlayer { AudioCategory = MediaPlayerAudioCategory.SoundEffects, Volume = 0.1, IsLoopingEnabled = false };
        private static MediaSource CompletedStampSoundSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/RemoveCompletedStamp.mp3"));
        private static MediaSource FinishCreatingTaskSoundSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/FinshedCreatingTask.mp3"));


        public static void PlayCompletedSound()
        {
            SoundFXSystem.Source = CompletedStampSoundSource;
            SoundFXSystem.Play();
        }

       

        public static void PlayFinishCreatingTaskSound()
        {
            FinishedSoundFXSystem.Source = FinishCreatingTaskSoundSource;
            FinishedSoundFXSystem.Play();
        }

    }
}

