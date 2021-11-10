using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace GreenPoint.Dotnet.WebAdmin.Services
{
    public class AwsS3FileUploadService
    {
        private readonly AwsS3Options _awsOptions;

        public AwsS3FileUploadService(IOptions<AwsS3Options> awsOptions)
        {
            this._awsOptions = awsOptions.Value;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("eu-central-1")
            };

            var s3 = new AmazonS3Client(
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                config
            );

            var guid = Guid.NewGuid().ToString();

            var request = new PutObjectRequest
            {
                BucketName = _awsOptions.BucketName,
                Key = guid + fileName,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead,

            };

            await s3.PutObjectAsync(request);
            return $"https://{_awsOptions.BucketName}.s3.eu-central-1.amazonaws.com/{guid + fileName}";
        }

    }
}