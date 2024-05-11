using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LiJunSpace.ViewModels
{
    public class NewRecordComponentViewModel : ComponentBase
    {
        public string Content { get; set; }
        public string Title { get; set; }


        public List<string> ImageList = new();

        public async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files) 
        {
            ImageList = new List<string>();
            const string format = "image/jpeg";
            foreach (var file in files)
            {
                var imageFile = await file.RequestImageFileAsync(format, 1200, 675);
                await using var fileStream = imageFile.OpenReadStream();
                await using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                var image = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                ImageList.Add(image);
            }
        }
    }
}
