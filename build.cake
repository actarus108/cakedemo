var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var solutionFolder = "./";
var outputFolder = "./artifacts";

Task("Clean")
  .Does(() => {
    CleanDirectory(outputFolder);
  });

Task("Restore")
  .Does(() => {
    DotNetRestore(solutionFolder);
  });

Task("Build")
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

Task("Publish")
  .IsDependentOn("Clean")
  .IsDependentOn("Test")
  .Does(() => {
    DotNetPublish(solutionFolder, new DotNetPublishSettings
    {
      NoRestore = true,
      Configuration = configuration,
      NoBuild = true,
      OutputDirectory = outputFolder
    });
  });

RunTarget(target);
