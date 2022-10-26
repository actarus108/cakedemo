using System.Diagnostics;

//#tool "nuget:?package="

var target 
    = Argument("target", "Default");
var build
    = Argument("build", "0");
var revision
    = Argument("revision", string.Empty);
var configuration 
    = Argument("configuration", "Release");

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

    List<string> loggers = new List<string>();
    loggers.Add("trx");

    List<string> collectors = new List<string>();
    collectors.Add("XPlat Code Coverage");

    GetFiles(rootAbsoluteDir + "./**/*[Tt]ests/*.csproj")
      .ToList()
        .ForEach(file => 
          DotNetTest(file.FullPath, new DotNetTestSettings
          {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true,
            ResultsDirectory = testResultsFolder,
            Loggers = loggers,
            Collectors = collectors
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
    var webAppFiles = GetFiles(webAppOutputFolder + "/*.*");
    Zip(webAppOutputFolder, artifactsFolder + "/" + webAppArtifactFileName, webAppFiles);
  });

Task("PublishLibrary")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(rootAbsoluteDir + "/MyLibrary", new DotNetPublishSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true,
      OutputDirectory = myLibraryOutputFolder,
      // EnvironmentVariables = 
      //   new Dictionary<string, string> {
      //       { "build", build },
      //       { "revision", revision }
    });
  });

Task("CreateLibraryNugetPackage")
  .IsDependentOn("PublishLibrary")
  .Does(() => {

    // var version = FileVersionInfo
    //         .GetVersionInfo(myLibraryOutputFolder + "/MyLibrary.dll")
    //         .FileVersion; 

    DotNetPack(rootAbsoluteDir + "/MyLibrary", new DotNetPackSettings
    {
      Configuration = configuration, 
      OutputDirectory = artifactsFolder,
     // Version = version
    });
  });

Task("Default")
  .IsDependentOn("ZipWebAppOutput")
  .IsDependentOn("CreateLibraryNugetPackage");

RunTarget(target);
