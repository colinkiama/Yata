using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using YATA.Model;

namespace YATA.Services
{
    public class FileIOService
    {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;


        public async Task<bool> loadData()
        {
            bool dataLoaded = false;

            bool ToDoTasksFilePresent = await checkIfToDoTasksFileIsPresent();

            if (ToDoTasksFilePresent)
            {
                try
                {
                    await loadFileIntoClass();
                    dataLoaded = true;
                }
                catch (Exception ex)
                {


                }

            }

            else
            {
                Debug.WriteLine("Save File not found so saving data instead");
                await saveData();
            }

            TileService.UpdateLiveTile(ToDoTask.listOfTasks);
            return dataLoaded;
        }

        private async Task loadFileIntoClass()
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<ToDoTask>));
            var ToDoTasksFile = await getToDoTaskFile();

            try
            {
                using (Stream stream = await ToDoTasksFile.OpenStreamForReadAsync())
                {
                    ObservableCollection<ToDoTask> loadedToDoTasks = new ObservableCollection<ToDoTask>();
                    loadedToDoTasks = (ObservableCollection<ToDoTask>)serializer.Deserialize(stream);
                    ToDoTask.listOfTasks = loadedToDoTasks;
                    await stream.FlushAsync();
                }

            }

            catch (Exception x)
            {
                Debug.WriteLine("Failed to load save file into class");
                Debug.WriteLine(x.Message);


            }


        }

        private async Task<bool> checkIfToDoTasksFileIsPresent()
        {
            bool fileExists = false;

            var query = localFolder.CreateFileQuery();
            var listOfFiles = await query.GetFilesAsync();

            if (listOfFiles.Count > 0)
            {
                foreach (var file in listOfFiles)
                {
                    if (file.Name == "ToDoTasks.txt")
                    {
                        fileExists = true;
                        break;
                    }
                }
            }




            return fileExists;
        }

        public async Task<bool> saveData()
        {
            bool dataSaved = false;

            var serializer = new XmlSerializer(typeof(ObservableCollection<ToDoTask>));
            var ToDoTasksFile = await createToDoTaskFile();

            try
            {
                using (Stream stream = await ToDoTasksFile.OpenStreamForWriteAsync())
                {
                    serializer.Serialize(stream, ToDoTask.listOfTasks);
                    await stream.FlushAsync();
                }
            }

            catch (Exception x)
            {

                Debug.WriteLine("Something Was wrong with saving the file");
                Debug.WriteLine(x);
                Debug.Write(x.Message);

            }

            return dataSaved;
        }


        private async Task<StorageFile> getToDoTaskFile()
        {
            StorageFile ToDoTasksFile = await localFolder.GetFileAsync("ToDoTasks.txt");

            return ToDoTasksFile;
        }


        private async Task<StorageFile> createToDoTaskFile()
        {
            StorageFile ToDoTasksFile = await localFolder.CreateFileAsync("ToDoTasks.txt", CreationCollisionOption.ReplaceExisting);

            return ToDoTasksFile;
        }
    }
}
