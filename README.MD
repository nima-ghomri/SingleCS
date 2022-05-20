# SingleCS
This is a console application that combines .cs files into a single file.

Generally speaking, it's not a good idea to write all of your application in a single file. But in certain situations, you are required to store all of your source code in single file, e.g. online contests like Google's [CodeJam](https://codingcompetitions.withgoogle.com/codejam/) ask to submit your answer as a single file.

Instead of writing your application in one file you could develop it as usual, separating each class or interface in their own files. Using SingleCS you can combine all .cs files together in build time.

## Getting Started
It can be used as simple as running SingleCS.exe in CMD and specifying custom files paths as arguments:

```
SingleCS.exe "C:\MyProject\Program.cs" "D:\Helpers\Utils.cs"
```

The result is `Program - Merged.cs` file which will be created in the directory that the program is running.

## Example
Here is an example to show how SingleCS works. There is a project named `MyProject` in drive `C:` alongside the SingleCS.exe. And there are multiple classes which we want to include in our output file located in drive `D:`.
* C:
    * SingleCS.exe
    * MyProject
        * Models
            * User.cs
            * Product.cs
        * Program.cs
* D:
    * Helpers
        * Utils.cs
        * Math.cs

## Paths
Both relative and absolute paths (and a combination of both) are supported by SingleCS. You can use [wildcards](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-6.0) (*, ** and ..) to specify patterns.

```
C:\>SingleCS.exe "MyProject\Program.cs" "MyProject\Models\*.cs" "D:\Helpers\Utils.cs"
```

These files would be grabbed by SingleCS. It outputs the name of the output file and a list of the files that it grabs to create the output file:

```
SingleCS: Combining...
Program - Merged.cs
                 C:\MyProject\Program.cs
                 C:\MyProject\Models\Product.cs
                 C:\MyProject\Models\User.cs
                 D:\Helpers\Utils.cs
```
## Options
Here are the available flags:

| Short         | Long          | Description   |
| ------------- | ------------- | ------------- |
| -? -h         | --help        | Show help information|
| -e            | --exclude     | Exclude files |
| -m            | --main        | Add main files|
| -d            | --directory   | Specify working directory|
| -o            | --output      | Output files template|
| -r            | --refactor    | Refactor usings and empty lines|

### Exclude (-e|--exclude)
You may want to exclude some files from a pattern (e.g. some .cs files created by compiler in obj folder), in this case you could use exclude flag (-e or --exclude) to specify files that should be excluded.

```
C:\>SingleCS.exe "MyProject\**\*.cs" --exclude "MyProject\obj\*"
```
`\**\*.cs` grabs all .cs files in all subdirectories.

### Main (-m|--main)
By default, the first file that is grabbed by SingleCS would be assumed to be the "main" file, the file that will be stored on top of other files in the combined output. But you can change this behavior by specifying main flag.

```
C:\>SingleCS.exe "MyProject\**\*.cs" --main "MyProject\Models\User.cs"
```
The result is `User - Merged.cs` file and `User` class will be on top of the file.

**User - Merged.cs**
```
using System;using System;
using Helpers;
namespace MyProject.Models
{
    public class User
    {
        // User class...            
    }
}


namespace MyProject
{
...
```
If multiple arguments specified for main flag (or a pattern used), multiple output files will be created for each main file, each of which starts with on of the main files:

```
C:\>SingleCS.exe "MyProject\**\*.cs" --main "D:\Helpers\*.cs"
```
This will create 2 output files:

```
Math - Merged.cs
                 D:\Helpers\Math.cs
                 C:\MyProject\Program.cs
                 C:\MyProject\Models\Product.cs
                 C:\MyProject\Models\User.cs
Utils - Merged.cs
                 D:\Helpers\Utils.cs
                 C:\MyProject\Program.cs
                 C:\MyProject\Models\Product.cs
                 C:\MyProject\Models\User.cs
```

### Directory (-d|--directory)
This will change the working directory, which could simplify the relative paths specified in arguments.
```
SingleCS.exe "**\*.cs" --main "Models\User.cs" --directory "MyProject"
```

### Output(-o|--output)
Output files will be created for each main files (if no main files specified, the first file is the main one). By default, the name of the output file is the name of the main file appended with `"Merged"` work, like `"<MainFileName> - Merged.cs"`. This could be changed by output flag. A string containing the pattern of the output file is specifid, e.g. `"*Output.cs"`. The asterisk (*) is the main file name. So if the main file is `Program.cs`the output file name would be `ProgramOutput.cs`.

```
C:\>SingleCS.exe "MyProject\**\*.cs" --output "*Output.cs"
```

### Refactor (-r|--refactor)
Refactor removes duplicated using statements, empty lines and adds needed line breaks.

Without refactor for the current example if you run this:
```
C:\>SingleCS.exe "MyProject\**\*.cs"
```
The result is:
```
using System;using System;
using Helpers;

namespace MyProject
{
...
```
Running it with refactor flag removes the extra `using System;`.

```
C:\>SingleCS.exe "MyProject\**\*.cs" --refactor
```
Resulting:
```
using System;
using Helpers;

namespace MyProject
{
...
```

## Visual Studio
Visual Studio has some build-in [macros](https://docs.microsoft.com/en-us/visualstudio/ide/reference/pre-build-event-post-build-event-command-line-dialog-box?view=vs-2022#macros) for build events. To make SingleCS to run after compilation of your project, in Visual Studio go to:

Project > Properties > Build > Events > Post-build event

And add your command there, for example:
```
C:\SingleCS.exe "**\*.cs" "D:\Helpers\*.cs" -m "Program.cs" -e "obj\*" -o "D:\$(ProjectName).cs" -r
```
![Assign post-build event](PostBuildEvent.png "Post-Build Event")

This command does these things:
* Combines all .cs files inside project root and Helpers directory
* Makes sure that Program class is on top of the combined file
* Excludes files that are in obj directory
* User build-in vs macro `$(ProjectName)` as template for output file (the output name will be `MyProject.cs`)
* Refactors using statements

## Notes
1. SingleCS does not check for syntax errors, so make sure that your files don't have any compilation error. A good idea is to run application after building the project.
