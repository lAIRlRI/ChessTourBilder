using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Data
{
    internal class ImageControl
    {
        //public static async Task<byte[]> GetImage()
        //{
        //    var result = await PickAndShow(ImageLoad());
        //    if (result is FileContentResult data)
        //    {
        //        var content = data.FileContent;
        //        return content;
        //    }
        //    return null;
        //}

        private static PickOptions ImageLoad() 
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>> { 
                    { DevicePlatform.iOS, new[] { "public.jpg" } },
                    { DevicePlatform.Android, new[] { "Image/jpg" } }, 
                    { DevicePlatform.WinUI, new[] { ".jpg"} }, 
                    { DevicePlatform.Tizen, new[] { "*/*" } }, 
                    { DevicePlatform.macOS, new[] { "jpg"} }, 
                }
            );
            
            PickOptions options = new() 
            { 
                PickerTitle = "Выберите картинку JPG", 
                FileTypes = customFileType, 
            };

            return options;
        }

        private static async Task<FileResult> PickAndShow(PickOptions options)
        {
            try { 
                var result = await FilePicker.Default.PickAsync(options); 
                if (result != null) 
                { 
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase)) 
                    { 
                        using var stream = await result.OpenReadAsync(); 
                        var image = ImageSource.FromStream(() => stream); 
                    } 
                } 
                return result; 
            }
            catch
            {
                return null;
            } 
        }
    }
}
