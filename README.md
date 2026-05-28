<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/1161595219/25.2.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1329493)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Blazor File Input - Customize File List and Display Custom Error Messages

This example customizes the [DevExpress Blazor File Input](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput) component as follows:

* Implements a custom file list UI to replace the built-in file list.
* Processes files in a custom way: displays upload state, buttons depending on state, and upload errors with custom messages.

![Custom file list](custom-file-list.gif)

## Main Page Structure

See [Index.razor](./CS/DxFileInput.CustomErrorMessage/Components/Pages/Index.razor).

* Adds a **Simulate Error** button to toggle simulating a custom upload error.
* Adds the [DxFileInput](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput) component:
   * Sets the [ShowFileList](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.ShowFileList) property to `false` to hide the built-in file list.
   * Handles the [SelectedFileChanged](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.SelectedFileChanged) event to display the custom file list.
   * Handles the [FilesUploading](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput.FilesUploading) event. The handler calls the **ProcessFile** method to simulate file upload, catch custom exceptions, and display exception messages.

## Custom File List Component

See [FileInputList.razor](./CS/DxFileInput.CustomErrorMessage/Components/Shared/FileInputList.razor).

The **FileInputList** component replaces the built-in file list. It displays file names, upload states, state-specific buttons (Upload, Reload, Cancel, Remove all), and custom exception messages.

The component uses [FileInputListEntry.cs](./CS/DxFileInput.CustomErrorMessage/Components/Shared/FileInputListEntry.cs) to store information about each file, including its upload state and error message.

## Documentation

- [Blazor File Input](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxFileInput)

<!-- feedback -->
## Does This Example Address Your Development Requirements/Objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=draft-blazor-fileinput-custom-error-message&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=draft-blazor-fileinput-custom-error-message&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
