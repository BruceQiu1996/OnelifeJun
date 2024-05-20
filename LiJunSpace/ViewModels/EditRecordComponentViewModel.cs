using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.Helpers;
using LiJunSpace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class EditRecordComponentViewModel : ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }
        [StringLength(4000, ErrorMessage = "正文长度需小于等于4000")]
        public string Content { get; set; }
        [StringLength(16, ErrorMessage = "标题长度需小于等于16")]
        public string Title { get; set; }
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public List<EditImageItem> ImageList = new();
        [Parameter]
        public string RecordId { get; set; }
        public bool IsDialogVisible { get; set; }
        public EditImageItem CurrentImage { get; set; }
        public EditImageItem? DraggingImage { get; set; }//the model that is being dragged
        protected async override Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(RecordId))
                return;

            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.record_detail, RecordId));
            if (resp != null)
            {
                var record = JsonSerializer.Deserialize<RecordDto>(await resp.Content.ReadAsStringAsync(), HttpRequest._jsonSerializerOptions);
                if (record != null)
                {
                    Title = record.Title;
                    Content = record.Content;

                    ImageList = record.Images.Select(x => new EditImageItem()
                    {
                        File = x,
                    }).ToList();
                }
                StateHasChanged();
            }
        }

        public async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files)
        {
            if (ImageList.Count + files.Count >= 9)
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
                ImageList.Add(new EditImageItem()
                {
                    File = image,
                    Name = file.Name,
                    Data = data,
                    New = true
                });
            }
        }

        public bool Open { get; set; }
        public void OpenImageDrawer(string id)
        {
            CurrentImage = ImageList.FirstOrDefault(x => x.Id == id);
            IsDialogVisible = false;
            Open = true;
            StateHasChanged();
        }

        public void ViewImageAction()
        {
            Open = false;
            if (CurrentImage == null)
                return;

            IsDialogVisible = true;
            StateHasChanged();
        }

        public void DeleteImageAction()
        {
            Open = false;
            if (CurrentImage == null)
                return;

            ImageList.Remove(CurrentImage);
        }

        public async void CancelEdit()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var result = DialogService.Show<CancelDialog>("取消编辑", options);

            var data = await result.Result;
            if (!data.Cancelled)
            {
                JSRuntime.InvokeVoidAsync("window.history.back");
            }
        }

        public async Task SaveAsync()
        {
            if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Content) && ImageList.Count <= 0)
            {
                Snackbar.Add("不能发表空的记录", Severity.Info);
                return;
            }

            foreach (var item in ImageList.Where(x => x.New))
            {
                var resp = await HttpRequest.PostImageAsync(HttpRequestUrls.upload_record_image, item.Name, item.Data);
                if (resp == null)
                {
                    Snackbar.Add("上传错误", Severity.Error);
                    return;
                }

                var filename = await resp.Content.ReadAsStringAsync();
                item.File = filename;
            }

            RecordUpdateDto recordCreationDto = new RecordUpdateDto();
            recordCreationDto.Id = RecordId;
            recordCreationDto.Title = Title;
            recordCreationDto.Content = Content;
            if (ImageList.Count > 0)
            {
                recordCreationDto.Images = JsonSerializer.Serialize(ImageList.Select(x=>x.File));
            }

            var editResult = await HttpRequest.PostAsync(HttpRequestUrls.record_edit, recordCreationDto);
            if (editResult != null)
            {
                //更新成功
                JSRuntime.InvokeVoidAsync("window.history.back");
            }
            else
            {
                Snackbar.Add("发布失败", Severity.Error);
                return;
            }
        }

        public void HandleDrop(EditImageItem editImageItem)
        {
            Open = false;
            IsDialogVisible = false;
            if (editImageItem is null)
            {
                return;
            }

            var index1 = ImageList.IndexOf(editImageItem);
            var index2 = ImageList.IndexOf(DraggingImage);

            ImageList[index2] = editImageItem;
            ImageList[index1] = DraggingImage;
        }
    }

    public class EditImageItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string File { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public bool New { get; set; }
        public bool IsDragOver { get; set; }
    }
}
