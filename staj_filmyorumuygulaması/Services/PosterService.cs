using System;
using System.IO;
using System.Windows.Forms;
using MovieReviewApp.Services.Interfaces;

namespace MovieReviewApp.Services
{
    public class PosterService : IPosterService
    {
        private const string POSTERS_FOLDER = "Posters";

        public string SavePosterToAppDirectory(string sourceImagePath)
        {
            if (string.IsNullOrEmpty(sourceImagePath) || !File.Exists(sourceImagePath))
                return null;

            try
            {
                var appDir = Application.StartupPath;
                var postersDir = Path.Combine(appDir, POSTERS_FOLDER);

                if (!Directory.Exists(postersDir))
                {
                    Directory.CreateDirectory(postersDir);
                }

                var fileName = Path.GetFileName(sourceImagePath);
                var newPath = Path.Combine(postersDir, $"{Guid.NewGuid()}_{fileName}");

                File.Copy(sourceImagePath, newPath, true);
                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Poster kaydedilirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
