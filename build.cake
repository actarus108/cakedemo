var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var rootAbsoluteDir = "./";
var myWebAppFolder = "./MyWebApp";
var myLibraryFolder = "./MyLibrary";
var artifactsFolder = "./artifacts";
var webAppOutputFolder = "./artifacts/mywebapp";
var webAppZippedFileName = "website.zip";
var myLibraryOutputFolder = "./artifacts/mylibrary"; 

Task("Clean")
  .Does(() => {
    CleanDirectory(artifactsFolder);
    CleanDirectory(webAppOutputFolder);
    CleanDirectory(myLibraryOutputFolder);
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
    DotNetTest(rootAbsoluteDir, new DotNetTestSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true
    });
  });

Task("PublishWebApp")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(myWebAppFolder , new DotNetPublishSettings
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
    Zip(webAppOutputFolder, artifactsFolder + "/" + webAppZippedFileName, webAppFiles);
  });

Task("PublishLibrary")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(myLibraryFolder, new DotNetPublishSettings
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
    DotNetPack(myLibraryFolder + "/MyLibrary.csproj", new DotNetPackSettings
    {
      Configuration = configuration, 
      OutputDirectory = artifactsFolder
    });
  });

Task("Default")
  .IsDependentOn("ZipWebAppOutput")
  .IsDependentOn("CreateLibraryNugetPackage");

RunTarget(target);
