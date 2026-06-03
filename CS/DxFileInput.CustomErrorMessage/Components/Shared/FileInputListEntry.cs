using DevExpress.Blazor;

namespace DxFileInput.CustomErrorMessage.Components.Shared;

public class FileInputListEntry {
    public required UploadFileInfo UploadInfo { get; init; }
    public IFileInputSelectedFile? SelectedFile { get; set; }

    public string Name => UploadInfo.Name;
    public string Guid => UploadInfo.Guid;
    public int Size => (int)UploadInfo.Size;

    public int BytesRead { get; set; }
    public string? ErrorMessage { get; set; }
    public byte[]? Data { get; set; }

    public UploadState State() => (
        SelectedFile is null ? UploadState.NotStarted :
        !String.IsNullOrEmpty(ErrorMessage) ? UploadState.Error :
        BytesRead < Size ? UploadState.InProgress :
        UploadState.Success
    );
}

public enum UploadState {
    NotStarted,
    InProgress,
    Success,
    Error
}
