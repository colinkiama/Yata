using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATA.Model
{
    public class Note
    {
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }
        public bool isCompleted { get; set; }

        public static ObservableCollection<Note> listOfTasks = new ObservableCollection<Note>();

        public event EventHandler isCompletedChanged;

      
        public static void CreateNote(string content)
        {
            Note noteToCreate = new Note();
            noteToCreate.Content = content;
            noteToCreate.DateCreated = DateTime.Now;
            noteToCreate.isCompleted = false;
            listOfTasks.Add(noteToCreate);
        }

        public void changeIsCompletedState()
        {
            isCompleted = !isCompleted;
            isCompletedChanged?.Invoke(this, EventArgs.Empty);
            
            
        }
    }
}
