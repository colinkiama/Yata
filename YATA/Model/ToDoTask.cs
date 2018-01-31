using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using YATA.Services;

namespace YATA.Model
{
    
    public class ToDoTask
    {
        
        public DateTime DateCreated { get; set; }
        
        public string Content { get; set; }
        
        public bool isCompleted { get; set; }

        
        public static ObservableCollection<ToDoTask> listOfTasks = new ObservableCollection<ToDoTask>();

        public event EventHandler isCompletedChanged;

      
        public async static Task CreateNote(string content)
        {
            ToDoTask noteToCreate = new ToDoTask();
            noteToCreate.Content = content;
            noteToCreate.DateCreated = DateTime.Now;
            noteToCreate.isCompleted = false;
            listOfTasks.Add(noteToCreate);
            //await SaveData();
        }

        public async Task changeIsCompletedState()
        {
            isCompleted = !isCompleted;
            isCompletedChanged?.Invoke(this, EventArgs.Empty);
            //await SaveData();
            
        }

        private async static Task SaveData()
        {
            await new FileIOService().saveData();
        }
    }
}
