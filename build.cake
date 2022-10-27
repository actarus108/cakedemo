using System.Diagnostics;

var target            = Argument("target", "Default");
var configuration     = Argument("configuration", "Release");
var build             = Argument("build", "0");
var revision          = Argument("revision", "local");

var rootAbsoluteDir = "./";
var artifactsFolder = "./artifacts";
var webAppOutputFolder = "./artifacts/mywebapp";
var webAppArtifactFileName = "website.zip";
var myLibraryOutputFolder = "./artifacts/mylibrary"; 
var testResultsFolder = "./testresults";

Task("Clean")
  .Does(() => {
    CleanDirectory(artifactsFolder);
    CleanDirectory(webAppOutputFolder);
    CleanDirectory(myLibraryOutputFolder);
    CleanDirectory(testResultsFolder);

    var dirsToClean = GetDirectories("./**/bin");
    dirsToClean.Add(GetDirectories("./**/obj"));
    CleanDirectories(dirsToClean);
  });

Task("Restore")
  .Does(() => {
    DotNetRestore(rootAbsoluteDir );
  });

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .Does(() => {
    DotNetBuild(rootAbsoluteDir, new DotNetBuildSettings
    {
      NoRestore = true,
      Configuration = configuration
    });
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    GetFiles(rootAbsoluteDir + "./**/*[Tt]ests/*.csproj")
      .ToList()
        .ForEach(file => 
          DotNetTest(file.FullPath, new DotNetTestSettings
          {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true,
            ResultsDirectory = testResultsFolder,
            Loggers = new List<string> { "trx" },
            Collectors = new List<string> { "XPlat Code Coverage" }
          })
        );
  });

Task("PublishWebApp")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(rootAbsoluteDir + "/MyWebApp" , new DotNetPublishSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true,
      OutputDirectory = webAppOutputFolder
    });
  });

Task("ZipWebAppOutput")
  .IsDependentOn("PublishWebApp")
  .Does(() => {    
    Zip(webAppOutputFolder, artifactsFolder + "/" + webAppArtifactFileName);
  });

Task("PublishLibrary")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(rootAbsoluteDir + "MyLibrary", new DotNetPublishSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true,
      OutputDirectory = myLibraryOutputFolder
    });
  });

Task("CreateLibraryNugetPackage")
  .IsDependentOn("PublishLibrary")
  .Does(() => {

    var projectName = "MyLibrary";
    var basePath = $"./{projectName}/bin/{configuration}/net6.0";
    var fileversion = FileVersionInfo.GetVersionInfo($"{basePath}/{projectName}.dll").FileVersion;
    var packageVersion = $"{fileversion.Split('.')[0]}.{fileversion.Split('.')[1]}.{fileversion.Split('.')[2]}.{build}";

    DotNetPack(rootAbsoluteDir + "MyLibrary", new DotNetPackSettings
    {
      NoBuild = true,
      Configuration = configuration, 
      NoRestore = true,          
      OutputDirectory = artifactsFolder,
      VersionSuffix = revision,
      MSBuildSettings = new DotNetMSBuildSettings {
        FileVersion = fileversion,
        PackageVersion = packageVersion,
        VersionPrefix = packageVersion,
        VersionSuffix = revision
      }
    });
  });

Task("Default")
  .IsDependentOn("ZipWebAppOutput")
  .IsDependentOn("CreateLibraryNugetPackage");

RunTarget(target);
