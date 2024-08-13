using Microsoft.AspNetCore.Http;

namespace MinitestTN.Common
{
    public class FileHelper
    {
        private readonly IWebHostEnvironment env;

        public FileHelper(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public async Task<string> Upload(IFormFile file, string folder)
        {
            string OutFileName = "";

            string targetPath = Path.Combine(env.WebRootPath, folder);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath); // สร้าง Folder
            }

            string nameFime = Path.GetFileNameWithoutExtension(file.FileName);
            string key = DateTime.Now.ToString("yyMMddHHmmssff");
            string ext = Path.GetExtension(file.FileName);


            //OutFileName = nameFime + "_" + DateTime.Now.ToString("yyMMddHHmmssff")+ ext;

            OutFileName = $"{nameFime}_{key}{ext}";

            string savePath = Path.Combine(targetPath, OutFileName);

            using (var stream = File.Create(savePath))
            {
                await file.CopyToAsync(stream);
            }

            return OutFileName;
        }

        public void Delete(string folder, string fileName)
        {
            if (fileName != null)
            {
                string daletePath = Path.Combine(env.WebRootPath, folder, fileName);

                if (File.Exists(daletePath))
                {
                    File.Delete(daletePath);
                }
            }
        }
    }
}
