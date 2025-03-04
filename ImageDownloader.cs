using System;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

public class ImageDownloader
{
    public async Task DownloadImagesAsync(string inputFile, string outputFolder, Action<string, bool> logAction)
    {
        // Проверка и создание директории
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        // Проверка существования файла
        if (!File.Exists(inputFile))
        {
            logAction("Файл links.txt не найден.", false);
            return;
        }

        // Чтение файла
        string[] urls;
        try
        {
            urls = (await File.ReadAllLinesAsync(inputFile))
                   .Where(line => !string.IsNullOrWhiteSpace(line) && !line.Trim().StartsWith("#"))
                   .ToArray();
        }
        catch (Exception ex)
        {
            logAction($"Ошибка при чтении файла: {ex.Message}", false);
            return;
        }

        // Проверка на пустой файл
        if (urls.Length == 0)
        {
            logAction("Ошибка: Файл links.txt пуст или содержит только пустые строки/комментарии.", false);
            return;
        }

        // Загрузка изображений
        using (HttpClient client = new HttpClient())
        {
            foreach (string url in urls)
            {
                try
                {
                    logAction($"Скачивание: {url}", true);
                    byte[] imageData = await client.GetByteArrayAsync(url);

                    // Формируем имя файла на основе URL
                    string fileName = Path.GetFileNameWithoutExtension(url);
                    string fileExtension = ".jpg"; // Всегда используем .jpg
                    string fullFileName = Path.Combine(outputFolder, $"{fileName}{fileExtension}");

                    // Проверяем, существует ли файл с таким именем, и если да, добавляем суффикс
                    int counter = 1;
                    while (File.Exists(fullFileName))
                    {
                        fullFileName = Path.Combine(outputFolder, $"{fileName}_{counter}{fileExtension}");
                        counter++;
                    }

                    await File.WriteAllBytesAsync(fullFileName, imageData);
                    logAction($"Сохранено: {fullFileName}", true);
                }
                catch (Exception ex)
                {
                    logAction($"Ошибка при загрузке {url}: {ex.Message}", false);
                }
            }
        }
    }
}