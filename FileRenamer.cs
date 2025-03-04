using System;
using System.IO;

public class FileRenamer
{
    public void AddPrefixToFiles(string folderPath, string prefix = "/")
    {
        // Проверка существования папки
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Папка не найдена.");
            return;
        }

        // Получаем все файлы в папке
        string[] files = Directory.GetFiles(folderPath);

        // Проходим по каждому файлу
        foreach (string file in files)
        {
            try
            {
                // Получаем имя файла и путь к директории
                string directory = Path.GetDirectoryName(file);
                string fileName = Path.GetFileName(file);

                // Формируем новое имя файла с префиксом
                string newFileName = Path.Combine(directory, $"{prefix}{fileName}");

                // Переименовываем файл
                File.Move(file, newFileName);
                Console.WriteLine($"Файл переименован: {newFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при переименовании файла {file}: {ex.Message}");
            }
        }

        Console.WriteLine("Переименование завершено.");
    }
}