using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string inputFile = "C:\\Programming\\Нова папка\\ConsoleApp2\\links.txt"; // Файл со ссылками
        string outputFolder = "C:\\Users\\Gladi\\OneDrive\\Desktop\\SavedImages"; // Папка для сохранения

        while (true)
        {
            Console.WriteLine("=== Программа для загрузки изображений ===");
            Console.WriteLine($"Текущий файл с ссылками: {inputFile}");
            Console.WriteLine($"Текущая папка для сохранения: {outputFolder}");
            Console.WriteLine();
            Console.WriteLine("=== Меню ===");
            Console.WriteLine("1. Изменить путь к файлу с ссылками");
            Console.WriteLine("2. Изменить папку для сохранения изображений");
            Console.WriteLine("3. Загрузить изображения");
            Console.WriteLine("4. Переименовать файлы с префиксом");
            Console.WriteLine("5. Открыть файл с ссылками для редактирования");
            Console.WriteLine("6. Выйти");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    inputFile = ChangeFilePath();
                    break;
                case "2":
                    outputFolder = ChangeOutputFolder();
                    break;
                case "3":
                    await DownloadImagesAsync(inputFile, outputFolder);
                    break;
                case "4":
                    RenameFiles(outputFolder);
                    break;
                case "5":
                    OpenFileForEditing(inputFile);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    // Метод для изменения пути к файлу с ссылками
    static string ChangeFilePath()
    {
        Console.Write("Введите новый путь к файлу с ссылками: ");
        string newFilePath = Console.ReadLine();

        if (File.Exists(newFilePath))
        {
            Console.WriteLine("Файл успешно изменён.");
            return newFilePath;
        }
        else
        {
            Console.WriteLine("Файл не найден. Используется текущий файл.");
            return "C:\\Programming\\Нова папка\\ConsoleApp2\\links.txt"; // Возвращаем старый путь
        }
    }

    // Метод для изменения папки для сохранения изображений
    static string ChangeOutputFolder()
    {
        Console.Write("Введите новый путь к папке для сохранения: ");
        string newOutputFolder = Console.ReadLine();

        if (Directory.Exists(newOutputFolder))
        {
            Console.WriteLine("Папка успешно изменена.");
            return newOutputFolder;
        }
        else
        {
            Console.WriteLine("Папка не найдена. Используется текущая папка.");
            return "C:\\Users\\Gladi\\OneDrive\\Desktop\\SavedImages"; // Возвращаем старый путь
        }
    }

    // Метод для загрузки изображений
    static async Task DownloadImagesAsync(string inputFile, string outputFolder)
    {
        ImageDownloader downloader = new ImageDownloader();

        await downloader.DownloadImagesAsync(inputFile, outputFolder, (message, isSuccess) =>
        {
            Console.ForegroundColor = isSuccess ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        });
    }

    // Метод для переименования файлов с префиксом
    static void RenameFiles(string outputFolder)
    {
        try
        {
            FileRenamer renamer = new FileRenamer();
            renamer.AddPrefixToFiles(outputFolder, "https---");
            Console.WriteLine("Файлы переименованы.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при переименовании файлов: {ex.Message}");
        }
    }


    // Метод для открытия файла с ссылками в текстовом редакторе
    static void OpenFileForEditing(string inputFile)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Файл links.txt не найден.");
            return;
        }

        try
        {
            // Открываем файл в ассоциированной программе (например, Блокнот)
            Process.Start(new ProcessStartInfo(inputFile) { UseShellExecute = true });
            Console.WriteLine("Файл открыт для редактирования.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при открытии файла: {ex.Message}");
        }
    }
}