﻿namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        bool IsFileValid(IFormFile photoFile);

        Task<string> UploudAsync(IFormFile file);

        Task<string> UploudAsync(byte[] file);
    }
}