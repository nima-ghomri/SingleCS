# SingleCS
This is a console application that combines .cs files into a single file.

Generally speaking it's not a good idea to write all of your application in a single file. But in certain situations you are required to store all of your source code in single file, e.g. online contests like Google's [CodeJam](https://codingcompetitions.withgoogle.com/codejam/) ask to submit your answer as a single file.

Instead of writing your application in one file you could develop it as usual, seperating each class or interface in their own files. Using SingleCS you can combine all .cs files togather in build time.

## Getting Started
It can be used as simple as running SingleCS.exe in CMD and specifiying custom files paths as arguments:

```
SingleCS.exe "C:\MyProject\Program.cs" "D:\Helpers\Utils.cs"
```

The result is `"Program - Merged.cs"` file which will be created in the directory that the program is running.

## Example
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

## Paths
Both relative and absolute paths (and a combination of both) are supported by SingleCS. You can use [wildcards](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-6.0) (*, ** and ..) to specify patterns.

```
C:\>SingleCS.exe "MyProject\Program.cs" "MyProject\Models\*.cs" "D:\Helpers\Utils.cs"
```

These files would be grabbed by SingleCS:.

```
C:\MyProject\Program.cs
C:\MyProject\Models\Product.cs
C:\MyProject\Models\User.cs
D:\Helpers\Utils.cs
```
## Options
Here are the available flags:

| Short         | Long          | Description   | Example       |
| ------------- | ------------- | ------------- | ------------- |
| -? -h         | --help        | Show help information|
| -e            | --exclude     | Exclude files |
| -m            | --main        | Add main files|
| -d            | --directory   | Specify working directory|
| -o            | --output      | Output files template|

### Exclude (-e|--exclude)
You may want to exclude some files from a pattern (e.g. some .cs files created by compiler in obj folder), in this case you could use exclude flag (-e or --exclude) to specify files that should be excluded.

```
C:\>SingleCS.exe "MyProject\**\*.cs" --exclude "MyProject\obj\*"
```
`\**\*.cs` would grab all .cs files in all subdirectories.

### Main (-m|--main)
By default the first file that is grabbed by SingleCS would be assumed to be the "main" file, the file that will be stored on top of other files in combined output. But you can change this behaviour by using main flag.

```
C:\>SingleCS.exe "MyProject\**\*.cs" --exclude "MyProject\obj\*"
```




## Visual Studio

## Notes