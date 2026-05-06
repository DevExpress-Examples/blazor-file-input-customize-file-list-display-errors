<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/XXXXXXXXX/25.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/TXXXXXXX)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Blazor File Input - Display Custom Error Messages

This example demonstrates how to use the [DxFileInput](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput) component to display custom error messages in a custom file list UI.

The key techniques shown in this example:
- Disable the built-in file list ([ShowFileList](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ShowFileList)) and replace it with a custom file list component placed inside the `DxFileInput` content area.
- Handle the [FilesUploading](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.FilesUploading) event to process files asynchronously and catch exceptions as custom error messages.
- Call [CancelFileUpload](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.CancelFileUpload(DevExpress.Blazor.UploadFileInfo)) when an error occurs to mark the upload as failed.
- Show state-specific UI for each file: a progress bar during upload, a success indicator when done, and a custom error message with a reload button on failure.
- Use [ReloadFile](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ReloadFile(DevExpress.Blazor.UploadFileInfo)) to retry a failed upload.

![Blazor DxFileInput - Custom Error Messages](images/blazor-dxfileinput-custom-error-message.png)

## Implementation Details

### Custom File List

Set [ShowFileList](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ShowFileList) to `false` to hide the built-in file list and place a custom `FileInputList` component as child content of `DxFileInput`:

```razor
<DxFileInput ShowFileList="false"
             SelectedFilesChanged="OnSelectedFilesChanged"
             FilesUploading="OnFilesUploading"
             ...>
    <CascadingValue Value="FileInput" Name="FileInput">
        <FileInputList Entries="entries" />
    </CascadingValue>
</DxFileInput>
```

The `FileInputList` component receives the file entries and renders state-specific UI for each file. It accesses the parent `DxFileInput` via a cascading parameter to call upload control methods (cancel, reload, remove).

### Custom Error Messages

The `FileInputListEntry` class tracks the state and error message for each file:

```csharp
public class FileInputListEntry {
    public required UploadFileInfo UploadInfo { get; init; }
    public IFileInputSelectedFile? SelectedFile { get; set; }
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
```

In the `FilesUploading` event handler, files are processed asynchronously. Exceptions (including cancellations) are caught and stored as a custom `ErrorMessage`. When a non-cancellation error is detected, `CancelFileUpload` is called to notify the component:

```csharp
async Task ProcessFile(FileInputListEntry entry) {
    try {
        // ... read file stream ...
    }
    catch (OperationCanceledException) {
        entry.ErrorMessage = "Canceled";
    }
    catch (Exception ex) {
        FileInput.CancelFileUpload(entry.UploadInfo);
        entry.ErrorMessage = ex.Message;
    }
}
```

### State-Specific UI

The `FileInputList` component renders different UI based on each file's upload state:

```razor
@switch (state) {
    case UploadState.NotStarted:
        <span>Ready to upload</span>
        break;
    case UploadState.InProgress:
        <DxProgressBar Value="entry.BytesRead" MaxValue="entry.Size" ... />
        break;
    case UploadState.Error:
        <span class="custom-upload-file-view-invalid-load-state">@(entry.ErrorMessage ?? "Error")</span>
        break;
    case UploadState.Success:
        <span class="custom-upload-file-view-success-load-state">Uploaded</span>
        break;
}
```

When a file is in the `Error` state, a reload button calls [ReloadFile](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ReloadFile(DevExpress.Blazor.UploadFileInfo)) to retry the upload.

## Files to Review

- [Index.razor](CS/DxFileInput.CustomErrorMessage/Components/Pages/Index.razor)
- [FileInputList.razor](CS/DxFileInput.CustomErrorMessage/Components/Shared/FileInputList.razor)
- [FileInputListEntry.cs](CS/DxFileInput.CustomErrorMessage/Components/Shared/FileInputListEntry.cs)

## Documentation

- [DxFileInput](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput)
- [ShowFileList](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ShowFileList)
- [FilesUploading](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.FilesUploading)
- [SelectedFilesChanged](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.SelectedFilesChanged)
- [CancelFileUpload](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.CancelFileUpload(DevExpress.Blazor.UploadFileInfo))
- [ReloadFile](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ReloadFile(DevExpress.Blazor.UploadFileInfo))

<!-- feedback -->
## Does This Example Address Your Development Requirements/Objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-fileinput-custom-error-message&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-fileinput-custom-error-message&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
