<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/XXXXXXXXX/25.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/TXXXXXXX)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Blazor File Input - Display Custom Error Messages

The [DxFileInput](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput) component includes a built-in file list, but you can replace it with a fully custom UI to display error messages that reflect your specific business logic. This example shows how to disable the built-in file list, render a custom one with per-file state indicators (ready, uploading, success, or error), and surface custom error messages when processing fails.

Click **Simulate Error** to trigger an error during file upload and see how the custom error message appears next to the affected file.

![Blazor DxFileInput - Custom Error Messages](images/blazor-dxfileinput-custom-error-message.png)

## Implementation Details

### 1. Replace the Built-In File List

Set [ShowFileList](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ShowFileList) to `false` to hide the default file list. Place a custom `FileInputList` component as the child content of `DxFileInput` and pass the parent component reference via a cascading parameter so the child can call upload control methods:

```razor
<DxFileInput @ref="FileInput"
             ShowFileList="false"
             UploadMode="UploadMode.OnButtonClick"
             SelectedFilesChanged="OnSelectedFilesChanged"
             FilesUploading="OnFilesUploading"
             ...>
    <CascadingValue Value="FileInput" Name="FileInput">
        <FileInputList Entries="entries" />
    </CascadingValue>
</DxFileInput>
```

### 2. Track File State

Create a `FileInputListEntry` class to track each file's upload state and error message. Derive the state from the available data — if `SelectedFile` is null the upload has not started; if `ErrorMessage` is set the upload failed; if `BytesRead` equals `Size` the upload succeeded:

```csharp
public class FileInputListEntry {
    public required UploadFileInfo UploadInfo { get; init; }
    public IFileInputSelectedFile? SelectedFile { get; set; }
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
```

Handle the [SelectedFilesChanged](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.SelectedFilesChanged) event to keep the entry list in sync as the user adds or removes files.

### 3. Catch Errors During Upload

Handle the [FilesUploading](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.FilesUploading) event to process each file asynchronously. Wrap file processing in a `try/catch` block. On failure, call [CancelFileUpload](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.CancelFileUpload(DevExpress.Blazor.UploadFileInfo)) to mark the upload as cancelled and store the exception message in `ErrorMessage`:

```csharp
async Task OnFilesUploading(FilesUploadingEventArgs args) {
    foreach (var file in args.Files) {
        var entry = entries.First(e => e.UploadInfo.Guid == file.Guid);
        entry.SelectedFile = file;
        entry.ErrorMessage = null;
        _ = ProcessFile(entry);
    }
    await InvokeAsync(StateHasChanged);
}

async Task ProcessFile(FileInputListEntry entry) {
    try {
        // read file stream and update entry.BytesRead incrementally
    }
    catch (OperationCanceledException) {
        entry.ErrorMessage = "Canceled";
    }
    catch (Exception ex) {
        FileInput.CancelFileUpload(entry.UploadInfo);
        entry.ErrorMessage = ex.Message;
    }
    await InvokeAsync(StateHasChanged);
}
```

### 4. Render State-Specific UI

In the `FileInputList` component, switch on `entry.State()` to display the correct UI for each file. Use a [DxProgressBar](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxProgressBar) while uploading and show the custom error message when the upload fails:

```razor
@switch (state) {
    case UploadState.NotStarted:
        <span>Ready to upload</span>
        break;
    case UploadState.InProgress:
        <DxProgressBar Value="entry.BytesRead" MaxValue="entry.Size" ShowLabel="false" />
        break;
    case UploadState.Error:
        <span class="custom-upload-file-view-invalid-load-state">@(entry.ErrorMessage ?? "Error")</span>
        break;
    case UploadState.Success:
        <span class="custom-upload-file-view-success-load-state">Uploaded</span>
        break;
}
```

When a file is in the `Error` state, show a reload button that calls [ReloadFile](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ReloadFile(DevExpress.Blazor.UploadFileInfo)) to clear the error and retry the upload:

```razor
case UploadState.Error:
    <DxButton Click="() => ReloadFile(entry)">
        <CustomIcon Type="uc-reload" />
    </DxButton>
    break;
```

```csharp
private void ReloadFile(FileInputListEntry entry) {
    entry.ErrorMessage = null;
    FileInput.ReloadFile(entry.UploadInfo);
}
```

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
