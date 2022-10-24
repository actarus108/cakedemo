var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solutionFolder = "./";
var myLibraryFolder = "./MyLibrary";
var artifactsFolder = "./artifacts";
var webAppOutputFolder = "./artifacts/mywebapp";
var webAppZippedFileName = "website.zip";
var myLibraryOutputFolder = "./artifacts/mylibrary";
var myLibraryZippedFileName = "mylibrary.zip";

Task("Clean")
  .Does(() => {
    CleanDirectory(webAppOutputFolder);
    CleanDirectory(myLibraryOutputFolder);
  });

Task("Restore")
  .Does(() => {
    DotNetRestore(solutionFolder);
  });

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .Does(() => {
    DotNetBuild(solutionFolder, new DotNetBuildSettings
    {
      NoRestore = true,
      Configuration = configuration
    });
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    DotNetTest(solutionFolder, new DotNetTestSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true
    });
  });

Task("PublishWebApp")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(solutionFolder, new DotNetPublishSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true,
      OutputDirectory = webAppOutputFolder
    });
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

Task("ZipArtifacts")
  .IsDependentOn("PublishWebApp")
  .IsDependentOn("PublishLibrary")
  .Does(() => {
    var webAppFiles = GetFiles(webAppOutputFolder + "/*.*");
    Zip(webAppOutputFolder, artifactsFolder + "/" + webAppZippedFileName, webAppFiles);

    var libraryFiles = GetFiles(myLibraryOutputFolder + "/*.*");
    Zip(myLibraryOutputFolder, artifactsFolder + "/" + myLibraryZippedFileName, libraryFiles);

  });

Task("Default")
  .IsDependentOn("ZipArtifacts");

RunTarget(target);
