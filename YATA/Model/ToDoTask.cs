using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using YATA.Services;

namespace YATA.Model
{
    
    public class ToDoTask
    {
        
        public DateTime DateCreated { get; set; }
        
        public string Content { get; set; }
        
        public bool isCompleted { get; set; }

        public static int CompletedTasks { get; set; } = 0;

        public static ObservableCollection<ToDoTask> listOfTasks = new ObservableCollection<ToDoTask>();


        public static event EventHandler CompletedTasksCountChanged;

        public event EventHandler isCompletedChanged;

        public static event EventHandler ListOfTasksChanged;

        public static void UpdateCompletedTasks(int score)
        {
            CompletedTasks = score;
            CompletedTasksCountChanged?.Invoke(true, EventArgs.Empty);
        }


        public static void CreateNote(string content)
        {
            ToDoTask noteToCreate = new ToDoTask();
            noteToCreate.Content = content;
            noteToCreate.DateCreated = DateTime.Now;
            noteToCreate.isCompleted = false;
            listOfTasks.Add(noteToCreate);
            ListOfTasksChanged?.Invoke(listOfTasks, EventArgs.Empty);
        }

        public void changeIsCompletedState()
        {
            isCompleted = !isCompleted;
            isCompletedChanged?.Invoke(this, EventArgs.Empty);
            CompletedTasks = isCompleted ? CompletedTasks + 1 : CompletedTasks - 1;
            CompletedTasksCountChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteNote()
        {
            var itemToDelete = listOfTasks.Where(p => p.Content == this.Content && p.DateCreated == this.DateCreated).First();
            listOfTasks.Remove(itemToDelete);
            ListOfTasksChanged?.Invoke(listOfTasks, EventArgs.Empty);
        }

    }
}
