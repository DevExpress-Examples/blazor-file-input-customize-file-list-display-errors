<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/1161595219/25.2.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1329493)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Blazor File Input — Customize File List and Display Custom Error Messages

This example customizes the [DevExpress Blazor File Input](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput) component as follows:

* Implements a custom file list UI to replace the built-in file list.
* Adds custom file processing: displays upload state and corresponding buttons, and upload errors with custom messages.

![Custom file list](file-input-list.gif)

## Configure a File Input Component

See [Index.razor](./CS/DxFileInput.CustomErrorMessage/Components/Pages/Index.razor).

The main application page displays two components:

* A **Simulate Error** button used to toggle simulation of a custom upload error.
* A [DxFileInput](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput) component:
   * The [ShowFileList](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ShowFileList) property is set to `false` to hide the built-in file list.
   * The [SelectedFileChanged](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.SelectedFileChanged) event is handled to display the custom file list.
   * The [FilesUploading](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.FilesUploading) event is handled. The handler calls the **ProcessFile** method to simulate file upload, catch custom exceptions, and display exception messages.

```Razor
<DxButton Click="() => SimulateError = !SimulateError">
    Simulate Error: @SimulateError
</DxButton>

<DxFileInput @ref="FileInput"
             AllowMultiFileUpload="true"
             UploadMode="UploadMode.OnButtonClick"
             ShowFileList="false"
             SelectedFilesChanged="OnSelectedFilesChanged"
             FilesUploading="OnFilesUploading"
             SelectButtonText="Choose files"
             CssClass="flex-column"
             MaxFileSize="1_000_000_000">
    <CascadingValue Value="FileInput" Name="FileInput">
        <FileInputList Entries="entries" />
    </CascadingValue>
</DxFileInput>
```

## Create a Custom File List Component

See [FileInputList.razor](./CS/DxFileInput.CustomErrorMessage/Components/Shared/FileInputList.razor).

The **FileInputList** component replaces the built-in file list. It displays file names, upload states, state-specific buttons (Upload, Reload, Cancel, Remove all), and custom exception messages.

```cs
﻿<div class="custom-upload-container">
    <div class="custom-upload-file-list-view" aria-live="polite">
        @foreach (var entry in Entries) {
            // Your markup to display files in a file list
        }
    ﻿</div>
﻿</div>

@code {
    [Parameter]
    public IEnumerable<FileInputListEntry> Entries { get; set; } = Enumerable.Empty<FileInputListEntry>();
    // ...
}
```

The component uses [FileInputListEntry.cs](./CS/DxFileInput.CustomErrorMessage/Components/Shared/FileInputListEntry.cs) to store information about each file, including its upload state and error message.

```cs
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
```

## Documentation

- [Blazor File Input](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput)

<!-- feedback -->
## Does This Example Address Your Development Requirements/Objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=draft-blazor-fileinput-custom-error-message&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=draft-blazor-fileinput-custom-error-message&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
