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
        public static readonly string saveFileName = "ToDoTasks.txt";
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public static StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;


        public async Task<bool> loadData()
        {
            bool dataLoaded = false;

            bool ToDoTasksFilePresent = await this.checkIfToDoTasksFileIsPresent();

            if (ToDoTasksFilePresent)
            {
                try
                {
                    await loadFileIntoClass();
                    dataLoaded = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

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
            var ToDoTasksFile = await this.getToDoTaskFile();

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
            var ToDoTasksFile = await this.createToDoTaskFile();

            try
            {
                using (Stream stream = await ToDoTasksFile.OpenStreamForWriteAsync())
                {
                    serializer.Serialize(stream, ToDoTask.listOfTasks);
                    await stream.FlushAsync();
                    dataSaved = true;
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
            StorageFile ToDoTasksFile = await localFolder.GetFileAsync(saveFileName);

            return ToDoTasksFile;
        }


        private async Task<StorageFile> createToDoTaskFile()
        {
            StorageFile ToDoTasksFile = await localFolder.CreateFileAsync(saveFileName, CreationCollisionOption.ReplaceExisting);

            return ToDoTasksFile;
        }

        public async Task<StorageFile> GiveBackToDoTaskFile()
        {
            return await localFolder.GetFileAsync("ToDoTasks.txt");

        }

        internal static async Task<bool> replaceOldTasksWithNewTasks(StorageFile tempLoadedTask)
        {
            bool finishedSyncing = false;
            ObservableCollection<ToDoTask> listOfNewTasks = await getListOfTasksFromFile(tempLoadedTask);
            if (listOfNewTasks != null)
            {
                ToDoTask.ReplaceOldTasksWithNewTasks(listOfNewTasks);
                finishedSyncing = await new FileIOService().saveData();
            }

            return finishedSyncing;
        }

        private static async Task<ObservableCollection<ToDoTask>> getListOfTasksFromFile(StorageFile tempLoadedTask)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<ToDoTask>));
            var ToDoTasksFile = tempLoadedTask;
            ObservableCollection<ToDoTask> loadedToDoTasks = new ObservableCollection<ToDoTask>();
            try
            {
                using (Stream stream = await ToDoTasksFile.OpenStreamForReadAsync())
                {
                   
                    loadedToDoTasks = (ObservableCollection<ToDoTask>)serializer.Deserialize(stream);
                    ToDoTask.listOfTasks = loadedToDoTasks;
                    await stream.FlushAsync();
                    return loadedToDoTasks;
                }

            }
            catch
            {
                Debug.WriteLine("Error with saving cloud file during syncing");
                return loadedToDoTasks;
            }
        }
    } 
}
