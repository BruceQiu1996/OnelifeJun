using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace LiJunSpace.ViewModels
{
    public class NewRecordComponentViewModel : ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }
        [StringLength(4000, ErrorMessage = "正文长度需小于等于4000")]
        public string Content { get; set; }
        [StringLength(16, ErrorMessage = "标题长度需小于等于16")]
        public string Title { get; set; }

        [Inject]
        public HttpRequest HttpRequest { get; set; }

        public List<UploadImageItem> ImageList = new();

        public async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files)
        {
            if (ImageList.Count >= 9)
            {
                Snackbar.Add("最多上传9张图片", Severity.Info);
                return;
            }
            const string format = "image/jpeg";
            foreach (var file in files)
            {
                if (file.Size > 1024 * 1024 * 10)
                {
                    Snackbar.Add($"图片{file.Name}大小超过10M", Severity.Info);
                    return;
                }

                var imageFile = await file.RequestImageFileAsync(format, 1200, 675);
                await using var fileStream = imageFile.OpenReadStream();
                await using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                var data = memoryStream.ToArray();
                var image = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                ImageList.Add(new UploadImageItem()
                {
                    File = image,
                    Name = file.Name,
                    Data = data
                });
            }
        }

        public bool Open { get; set; }
        public Anchor Anchor { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public UploadImageItem CurrentImage { get; set; }
        public void OpenImageDrawer(Anchor anchor, UploadImageItem item)
        {
            Anchor = anchor;
            CurrentImage = item;
            Open = true;
            Width = "100%";
            Height = "100px";
            StateHasChanged();
        }

        public void RemoveUploadImage()
        {
            ImageList.Remove(CurrentImage);
            Open = false;
            CurrentImage = null;
            StateHasChanged();
        }

        public async Task PublishNewRecordAsync()
        {
            if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Content) && ImageList.Count <= 0)
            {
                Snackbar.Add("不能发表空的记录", Severity.Info);
                return;
            }

            List<string> fileNames = new List<string>();
            foreach (var item in ImageList)
            {
                var resp = await HttpRequest.PostImageAsync(HttpRequestUrls.upload_record_image, item.Name, item.Data);
                if (resp == null)
                {
                    Snackbar.Add("上传错误", Severity.Error);
                    return;
                }

                var filename = await resp.Content.ReadAsStringAsync();
                fileNames.Add(filename);
            }

            RecordCreationDto recordCreationDto = new RecordCreationDto();
            recordCreationDto.Title = Title;
            recordCreationDto.Content = Content;
            recordCreationDto.Image_1 = fileNames.Count >= 1 ? fileNames[0] : null;
            recordCreationDto.Image_2 = fileNames.Count >= 2 ? fileNames[1] : null;
            recordCreationDto.Image_3 = fileNames.Count >= 3 ? fileNames[2] : null;
            recordCreationDto.Image_4 = fileNames.Count >= 4 ? fileNames[3] : null;
            recordCreationDto.Image_5 = fileNames.Count >= 5 ? fileNames[4] : null;
            recordCreationDto.Image_6 = fileNames.Count >= 6 ? fileNames[5] : null;
            recordCreationDto.Image_7 = fileNames.Count >= 7 ? fileNames[6] : null;
            recordCreationDto.Image_8 = fileNames.Count >= 8 ? fileNames[7] : null;
            recordCreationDto.Image_9 = fileNames.Count >= 9 ? fileNames[8] : null;

            var publishResult = await HttpRequest.PostAsync(HttpRequestUrls.record, recordCreationDto);
            if (publishResult != null) 
            {
                //发布成功
            }
            else 
            {
                Snackbar.Add("发布失败", Severity.Error);
                return;
            }
        }
    }

    public class UploadImageItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string File { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
