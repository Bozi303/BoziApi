﻿using FileManager.Models;

namespace FileManager.Services
{
    public class FileManagerService : IFileManager
    {
        private readonly string _storageDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BoziApp", "Files");

        public FileManagerService()
        {
            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory);
            }
        }

        public void DeleteFile(string fileId)
        {
            try
            {
                string filePath = GetFilePath(fileId);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            } catch (Exception ex)
            {
                throw new FileManagerException(500, ex.Message);
            }
        }

        public FileModel GetFile(string fileId)
        {
            string filePath = GetFilePath(fileId);
            if (File.Exists(filePath))
            {
                FileModel fileModel = ReadFileModel(filePath);
                return fileModel;
            }
            else
            {
                throw new FileManagerException(404, "فایل مورد نظر یافت نشد");
            }
        }

        public string SaveFile(byte[] data, string name, string dataType)
        {
            try
            {
                var currentTime = DateTime.Now;
                string fileId = GenerateFileId(name, dataType, currentTime);

                FileModel fileModel = new()
                {
                    Name = name,
                    FileExtension = dataType,
                    CreationDate = currentTime,
                    Data = data
                };

                WriteFileModel(fileModel);

                return fileId;
            } catch (Exception ex)
            {
                throw new FileManagerException(500, ex.Message);
            }
        }

        private string GenerateFileId(string name, string dataType, DateTime currentTime)
        {
            string timestamp = currentTime.ToString("yyyyMMddHHmmssffff");
            string fileId = $"{name}_{timestamp}_{dataType}";
            return fileId;
        }

        private string GetFilePath(string fileId)
        {
            return Path.Combine(_storageDirectory, fileId);
        }

        private void WriteFileModel(FileModel fileModel)
        {
            string fileName = $"{fileModel.Name}_{fileModel.CreationDate:yyyyMMddHHmmssffff}_{fileModel.FileExtension}";
            string fullFilePath = GetFilePath(fileName);
            File.WriteAllBytes(fullFilePath, fileModel.Data);
        }

        private FileModel ReadFileModel(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string[] parts = fileName.Split('_');

            return new FileModel
            {
                Name = parts[0],
                CreationDate = DateTime.ParseExact(parts[1], "yyyyMMddHHmmssffff", null),
                FileExtension = parts[2],
                Data = fileData
            };
        }
    }
}
