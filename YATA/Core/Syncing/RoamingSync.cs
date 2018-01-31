using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using YATA.Model;

namespace YATA.Core.Syncing
{
    public class RoamingSync
    {
        private static ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

        public static bool RestoreScore()
        {
            bool scoreRestored = false;
            if (roamingSettings.Values["CompletedTasks"] != null)
            {
                int score = (int)roamingSettings.Values["CompletedTasks"];
                ToDoTask.UpdateCompletedTasks(score); 
                scoreRestored = true;
            }

            return scoreRestored;
        }

        public static void UpdateScore(int updatedScore)
        {
            roamingSettings.Values["CompletedTasks"] = updatedScore;
            
        }

        public static void UpdateDataChange()
        {
            roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values["CompletedTasks"] != null)
            {
                int retrievedScore = (int)roamingSettings.Values["CompletedTasks"];

                if (ToDoTask.CompletedTasks < retrievedScore)
                {
                    ToDoTask.UpdateCompletedTasks(retrievedScore);

                }

                else
                {
                    roamingSettings.Values["CompletedTasks"] = ToDoTask.CompletedTasks;
                }
            }
        }
    }
}
