# Install
```shell
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
DefaultProvider [cdnjs]: [Enter]
```
# Config in `libman.json`
```json
{
  "version": "1.0",
  "defaultProvider": "cdnjs",
  "libraries": [
    {
      "library": "multiple-select@1.2.3",
      "destination": "wwwroot/lib/multiple-select"
    }
  ]
}
```
|    Name of lib    |  Version |              Path             |
| :---------------- | :------- | :---------------------------- |
| `multiple-select` | `1.2.3`  | `wwwroot/lib/multiple-select` |
# Restore lbs
```shell
libman restore
```
# Update libs
```shell
libman update NameOfLib
```
# Usage
```csharp
@section Scripts {
    <script src="~/lib/multiple-select/multiple-select.min.js"></script>
    <link rel="stylesheet" href="~/lib/multiple-select/multiple-select.min.css"/>
    <script>
        $("#id-select-roles").multipleSelect({
            selectAll: false,
            keepOpen: false,
            isOpen: false,
        })
    </script>
}
```