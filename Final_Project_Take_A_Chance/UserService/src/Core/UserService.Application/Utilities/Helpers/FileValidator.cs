using Microsoft.AspNetCore.Http;
using UserService.Application.Utilities.Enums;

namespace UserService.Application.Utilities.Helpers
{
    public static class FileValidator
    {


        public static bool ValidateType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateSize(this IFormFile file, FileSize fileSize, int size)
        {
            return file.Length < (int)fileSize * size;
        }

        public static async Task<string> CreateFileAsync(this IFormFile file, params string[] roots)
        {
            string fileName = file.FileName;

            fileName = fileName.Length > 64
                ? fileName.Substring(fileName.Length - 64, 64)
                : fileName;


            fileName = String.Concat(Guid.NewGuid().ToString(), file.FileName);
            string path = string.Empty;
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }

            path = Path.Combine(path, fileName);

            using (FileStream fileStream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public static void DeleteFile(this string fileName, params string[] roots)
        {
            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }

            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
