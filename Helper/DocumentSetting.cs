namespace CompanyMangementSystem_PL.Helper
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            //Get Located FolderPath
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            //get FileName And Make it Unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //Get FilePath
            string filePath = Path.Combine(folderPath, fileName);

            //Save file As Stream
            using var filestream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(filestream);
            return fileName;
        }
        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\" + FolderName + FileName;
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
        

    }
}
