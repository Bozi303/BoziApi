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
                using (var content = new MultipartFormDataContent())
                {
                    content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=\"file\"; filename=\"{file.FileName}\"");

                    using (var fileStream = file.OpenReadStream())
                    {
                        content.Add(new StreamContent(fileStream), "file", file.FileName);
                    } 

                    var response = await _httpClient.PostAsync<MultipartFormDataContent, FileManagerUploadResponse>(_uploadUrl, content);

                    return response.FileId ?? throw new Exception("خطا در ذخیره فایل");
                }
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
                var response = await _httpClient.GetAsync<FileManagerDownloadResponse>(title);
                return response;
            } catch (Exception ex)
            {
                throw new BoziException(400, ex.Message);
            }
        }
    }
}
