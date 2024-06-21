using ClietnApi.Services.PresentationModel;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using SharedModel.System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace ClietnApi.Services
{
    public class FileManagerClient
    {
        private string _uploadUrl;
        private string _downloadUrl;
        private HttpServiceFactory _httpClient;

        public FileManagerClient(string baseUrl, string uploadUrl, string download)
        {
            _httpClient = new HttpServiceFactory(baseUrl);
            _uploadUrl = uploadUrl;
            _downloadUrl = download;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                byte[] data;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    data = ms.ToArray();
                }

                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName).TrimStart('.');

                var req = new FileManagerUploadRequest
                {
                    Data = data,
                    Name = fileName,
                    Extension = fileExtension
                };
                
                var response = await _httpClient.PostAsync<FileManagerUploadRequest, FileManagerUploadResponse>(_uploadUrl, req);

                return response.FileId ?? throw new Exception("خطا در ذخیره فایل");
                
            }
            catch (Exception ex)
            {
                throw new BoziException(500, ex.Message);
            }
        }

        public async Task<FileManagerDownloadResponse> DownloadFile(string title)
        {
            try
            {
                var response = await _httpClient.GetAsync<FileManagerDownloadResponse>($"{_downloadUrl}?fileId={title}");
                return response;
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
