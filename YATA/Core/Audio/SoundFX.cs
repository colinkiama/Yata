using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;

namespace YATA.Core.Audio
{
    public class SoundFX
    {
        private static MediaElement SoundFXSystem = new MediaElement();
        private static Uri CompletedStampSoundSource = new Uri("InsertSourceHere");
        private static Uri RemoveCompletedStampSoundSource = new Uri("InsertSourceHere");
        private static Uri CreateTaskSoundSource = new Uri("InsertSourceHere");
        private static Uri FinishCreatingTaskSoundSource = new Uri("InsertSourceHere");


        public static void PlayCompletedSound()
        {
            SoundFXSystem.Source = CompletedStampSoundSource;
            SoundFXSystem.Play();
        }

        public static void PlayRemoveCompletedStampSound()
        {
            SoundFXSystem.Source = RemoveCompletedStampSoundSource;
            SoundFXSystem.Play();
        }

        public static void PlayCreateTaskSound()
        {
            SoundFXSystem.Source = CreateTaskSoundSource;
            SoundFXSystem.Play();
        }

        public static void PlayFinishCreatingTaskSound()
        {
            SoundFXSystem.Source = FinishCreatingTaskSoundSource;
            SoundFXSystem.Play();
        }

    }
}

