using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SuperShop.Common;
using SuperShop.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using SuperShop.Model.ViewModel;
using SuperShop.Model.CommonModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Formats.Webp;
using System.Drawing;
using System.Drawing.Imaging;
using Libwebp.Standard;
using Microsoft.AspNetCore.WebUtilities;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Diagnostics.Eventing.Reader;

namespace SuperShop.Services.Common
{
    public class CommonServices : ICommonServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConnectionStringConfig _connectionStringConfig;

        #region Private Service
        //private System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        //{
        //    var destRect = new System.Drawing.Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }

        //    return destImage;
        //}
        //private byte[] ConvertToWebP(System.Drawing.Image image)
        //{
        //    using (var outputStream = new MemoryStream())
        //    {
        //        // Create an ImageCodecInfo object for the WebP format
        //        ImageCodecInfo webpCodec = GetWebPCodecInfo();

        //        // Create an EncoderParameters object to set WebP quality
        //        EncoderParameters encoderParams = new EncoderParameters(1);
        //        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L); // Set the quality level here

        //        // Save the image to the output stream using the WebP codec and quality settings
        //        image.Save(outputStream, webpCodec, encoderParams);

        //        return outputStream.ToArray();
        //    }
        //}
        //private ImageCodecInfo GetWebPCodecInfo()
        //{
        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        //    foreach (ImageCodecInfo codec in codecs)
        //    {
        //        if (codec.FormatID == ImageFormat.Webp.Guid)
        //        {
        //            return codec;
        //        }
        //    }
        //    throw new Exception("WebP codec not found.");
        //}
        #endregion
        public CommonServices(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ConnectionStringConfig connectionStringConfig)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _connectionStringConfig = connectionStringConfig;
        }
        public string GetBaseURL()
        {
            return _httpContextAccessor.HttpContext != null ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}" : "";
        }
        public async Task<ServiceResponse<PublishFileDetail>> UploadFile_Supershop(string fileFolder, string baseUrl = "", IFormFile formFile = null, byte[] fileByte = null)
        {
            try
            {
                var webRootPath = _webHostEnvironment.WebRootPath;

                var filePath = Path.GetFullPath((baseUrl.IsNullOrEmpty() ? webRootPath : baseUrl) + fileFolder);
                //var filePath = Path.GetFullPath(_webHostEnvironment.WebRootPath + fileFolder);
                var uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + Utilities.GenerateRandomCodeStringByByteSize(10);
                var extension = "";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (formFile != null) // If Uploaded Using Multi-Part Form-Data
                {
                    extension = Path.GetExtension(formFile.FileName);
                    filePath = Path.Combine(filePath, uniqueFileName + extension);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
                else if (fileByte != null) // If Uploaded Using Raw Byte Array
                {
                    extension = ".png";
                    using (FileStream stream = new FileStream(Path.Combine(filePath + uniqueFileName + extension), FileMode.CreateNew))
                    {
                        await stream.WriteAsync(fileByte, 0, fileByte.Length);
                    }
                }

                var finalFolderURL = (baseUrl.IsNullOrEmpty() ? webRootPath : baseUrl) + fileFolder + uniqueFileName + extension;
                return ServiceResponse<PublishFileDetail>.Success("", new PublishFileDetail()
                {
                    BaseURL = (baseUrl.IsNullOrEmpty() ? webRootPath : baseUrl),
                    SubFolderLocation = fileFolder,
                    FileName = uniqueFileName + extension,
                    Extension = extension,
                    FinalURL = finalFolderURL
                });
            }
            catch (Exception ex)
            {
                return ServiceResponse<PublishFileDetail>.Error(ex.Message);
            }
        }
        public async Task<ServiceResponse<PublishFileDetail>> UploadImageWebPFormat_Supershop(string fileFolder, string baseUrl = "", IFormFile formFile = null, int width = 0, int height = 0)
        {
            try
            {
                if(formFile == null)
                {
					return ServiceResponse<PublishFileDetail>.Error("No data found.");
				}

                if (formFile.ContentType == "image/jpeg" || formFile.ContentType == "image/jpg" || formFile.ContentType == "image/png")
                {
					var webRootPath = _webHostEnvironment.WebRootPath;

					var filePath = Path.GetFullPath((baseUrl.IsNullOrEmpty() ? webRootPath : baseUrl) + fileFolder);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + Utilities.GenerateRandomCodeStringByByteSize(10);
					var extension = ".webp";

					filePath = Path.Combine(filePath, uniqueFileName + extension);

					using (var image = SixLabors.ImageSharp.Image.Load(formFile.OpenReadStream()))
					{
						// Resize the image to 200x200 pixels
						// Resize the image to 200x200 pixels
						if(width > 0 || height > 0)
                        {
							image.Mutate(x => x.Resize(new ResizeOptions
							{
								Size = new SixLabors.ImageSharp.Size(width, height),
								Mode = ResizeMode.Max
							}));
						}

						// Save the image in WebP format
						using (var outputStream = new FileStream(filePath, FileMode.Create))
						{
							image.Save(outputStream, new JpegEncoder()); // Use JpegEncoder for WebP
						}

						var finalFolderURL = (baseUrl.IsNullOrEmpty() ? GetBaseURL() : webRootPath) + fileFolder + uniqueFileName + extension;

						return ServiceResponse<PublishFileDetail>.AddedSuccessfully(new PublishFileDetail
						{
							BaseURL = (baseUrl.IsNullOrEmpty() ? webRootPath : baseUrl),
							SubFolderLocation = fileFolder,
							FileName = $"{uniqueFileName}{extension}",
							Extension = extension,
							FinalURL = finalFolderURL
						});
					}
				}
                else
                {
                    return ServiceResponse<PublishFileDetail>.Error("Incorrect File Format. Only Accept image/jpeg, image/jpg, image/png");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return ServiceResponse<PublishFileDetail>.Error(ex.Message);
            }
        }
             
    }
     
    public interface ICommonServices
    {
        string GetBaseURL();
        Task<ServiceResponse<PublishFileDetail>> UploadFile_Supershop(string fileFolder, string baseUrl, IFormFile formFile = null, byte[] fileByte = null);
        Task<ServiceResponse<PublishFileDetail>> UploadImageWebPFormat_Supershop(string fileFolder, string baseUrl = "", IFormFile formFile = null, int width = 200, int height = 200);
    }
}
