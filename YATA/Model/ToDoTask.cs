using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using YATA.Services;

namespace YATA.Model
{

    public class ToDoTask: INotifyPropertyChanged
    {

        public DateTime DateCreated { get; set; }

        public string Content { get; set; }


        private bool _isCompleted;

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            { 
                _isCompleted = value;
                NotifyPropertyChanged();
                ListOfTasksChanged?.Invoke(listOfTasks, EventArgs.Empty);
            }
        }

        public static int CompletedTasks { get; set; } = 0;

        public static ObservableCollection<ToDoTask> listOfTasks = new ObservableCollection<ToDoTask>();


        public static event EventHandler CompletedTasksCountChanged;

        public event EventHandler isCompletedChanged;

        public static event EventHandler ListOfTasksChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public static void UpdateCompletedTasks(int score)
        {
            CompletedTasks = score;
            CompletedTasksCountChanged?.Invoke(true, EventArgs.Empty);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static void CreateNote(string content)
        {
            ToDoTask noteToCreate = new ToDoTask();
            noteToCreate.Content = content;
            noteToCreate.DateCreated = DateTime.Now;
            noteToCreate.IsCompleted = false;
            listOfTasks.Add(noteToCreate);
            ListOfTasksChanged?.Invoke(listOfTasks, EventArgs.Empty);
        }

        public void changeIsCompletedState()
        {
            IsCompleted = !IsCompleted;
            isCompletedChanged?.Invoke(this, EventArgs.Empty);
            CompletedTasks = IsCompleted ? CompletedTasks + 1 : CompletedTasks - 1;
            CompletedTasksCountChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteNote()
        {
            listOfTasks.Remove(this);
            ListOfTasksChanged?.Invoke(listOfTasks, EventArgs.Empty);
        }

        public static void ReplaceOldTasksWithNewTasks(ObservableCollection<ToDoTask> listOfNewTasks)
        {
            Debug.WriteLine(listOfNewTasks.Count);

            
            
            if (listOfNewTasks.Count > 0)
            {
                for (int i = 0; i < listOfNewTasks.Count; i++)
                {
                    listOfTasks[i] = listOfNewTasks[i];
                }

                for (int i = 0; i < listOfTasks.Count; i++)
                {
                    if (i > listOfNewTasks.Count - 1)
                    {
                        listOfTasks.RemoveAt(i);
                    }
                }
            }
            else
            {
                listOfTasks.Clear();
            }
            
        }
    }
}
