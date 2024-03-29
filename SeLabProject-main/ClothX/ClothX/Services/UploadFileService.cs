using ClothX.Constants;
using ClothX.Utility;

namespace ClothX.Services
{
	// Service for uploading files
	public class UploadFileService
	{
		private static UploadFileService _instance;

		public static UploadFileService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UploadFileService();
				return _instance;
			}
		}

		private UploadFileService() { }

		// Upload a file and return the file path
		public async Task<string> UploadFile(IFormFile image, List<FilePathEnum> folderPath, IWebHostEnvironment webHost)
		{
			var webRootPath = webHost.WebRootPath;
			string path = "";
			string path2 = "";
			foreach (var fPath in folderPath)
			{
				path = Path.Combine(path, fPath.ToString());
			}
			path2 = path;
			var WebFolder = Path.Combine(webRootPath, path);

			if (!Directory.Exists(WebFolder))
			{
				Directory.CreateDirectory(WebFolder);
			}

			var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
			string uniquefileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
			var fileName = Path.Combine(imagesFolder, uniquefileName);

			string uploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), WebFolder, fileName);
			using (FileStream file = new FileStream(uploadFilePath, FileMode.Create))
			{
				await image.CopyToAsync(file);
			}

			string filePath = "/" + Path.Combine(path2, uniquefileName);
			return filePath;
		}
	}
}
