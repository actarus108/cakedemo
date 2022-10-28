# JEDI EXPACEO 03/11/2022

**Cake - Vous reprendrez bien une part de gâteau ?**

Comment optimiser son processus de CI/CD avec un outil d'automatisation de build multiplateforme gratuit et open source avec un DSL C# pour des tâches telles que la compilation de code, la copie de fichiers et de dossiers, l'exécution de tests unitaires, la compression de fichiers et la création de packages NuGet. Vous pouvez ensuite utiliser ce système pour construire votre projet de la même façon sur Azure DevOps, Jenkins, TeamCity, GitHub Actions ou sur le système de build de votre choix.

## Présentation

### Cake c'est quoi ?

Cake est un outil d'automatisation de build multiplateforme gratuit et open source avec un DSL C# pour des tâches telles que la compilation de code, la copie de fichiers et de dossiers, l'exécution de tests unitaires, la compression de fichiers et la création de packages NuGet.
Vous pouvez ensuite utiliser ce système pour construire votre projet de la même façon sur Azure DevOps, Jenkins, TeamCity, GitHub Actions ou sur le système de build de votre choix.
Le projet a été présenté pour la première à la conférence NDC d'Oslo en 2016, ce n'est donc pas super récent.

### Avantages

#### Familier

Cake est construit sur le compilateur Roslyn qui vous permet d'écrire vos scripts de construction en pur C # dans un projet de console standard, en utilisant Cake Frosting, ou en tant que script Cake en utilisant Cake .NET Tool.

#### Cross platform & cross runtime

Cake fonctionne sur la plate-forme .NET récente (.NET Core 3.1 ou .NET 6 et plus récent) et est disponible sur Windows, Linux et macOS.

#### Intégration avec les IDE

Cake peut fonctionner comme de simples applications console avec une intégration IDE complète, y compris IntelliSense ou la refactorisation. Il existe des extensions disponibles pour fournir des fonctionnalités avancées pour les IDE et les éditeurs les plus populaires.

#### Fiable

Que vous construisiez sur votre propre machine ou sur un système CI tel qu'Azure Pipelines, GitHub Actions, TeamCity ou Jenkins, Cake est conçu pour se comporter de la même manière.

#### Prise en charge d'un grand nombre d'outils

Cake prend en charge les outils les plus courants utilisés lors des builds tels que MSBuild, .NET Core CLI, MSTest, xUnit, NUnit, NuGet, ILMerge, WiX et SignTool prêts à l'emploi et bien d'autres grâce à une liste sans cesse croissante d'addins et de modules.

#### Open source et soutenu par la communauté

Cake sera toujours gratuit et open source, même pour un usage commercial. Si vous souhaitez les soutenir, nous acceptons les dons. Il y a une communauté incroyable autour de Cake avec plusieurs centaines de contributeurs qui sont impliqués dans le projet et vous pouvez également contribuer à ce beau projet ➡️ [https://github.com/cake-build/cake]

### Cas d'usages
Plusieurs cas d'usages peuvent être intéressants avec Cake :

- Avoir un processus de build identique entre le poste de développement et l'environnement de CI utilisé
- Non-Adhérence du processus de build avec l'environnement de CI et donc portabilité du processus de CI entre les différents environnements.
  Ex : je dois migrer de Jenkins vers Azure DevOps ou de Azure DevOps vers GitHub (spoiler : cas client qui devrait beaucoup plus se produire en 2023


### Runners

#### Cake .NET Tool

Cake .NET Tool est un runner qui permet d'exécuter des scripts Cake.

##### Requirements

Le package Cake.Tool NuGet qui est un outil .NET Core compilé pour .NET Core 3.1 ou plus récent.

##### Usage

```shell
dotnet cake [script] [switches]
```

##### Setup

- Assurez-vous d'avoir un manifeste des outils dotnet disponible dans votre référentiel ou créez-en un à l'aide de la commande suivante :

  ```shell
  dotnet new tool-manifest
  ```

- Installez Cake en tant qu'outil local à l'aide de la commande dotnet tool (vous pouvez remplacer 2.3.0 par une version différente de Cake que vous souhaitez utiliser) :

  ```shell
  dotnet tool install Cake.Tool --version 2.3.0
  ```

#### Cake Frosting

Cake Frosting est un hôte .NET qui vous permet d'écrire vos scripts de construction en tant qu'application console.

##### Requirements

Cake.Frosting peut être utilisé pour écrire des applications console ciblant netcoreapp3.1 ou plus récent.

##### Usage

```shell
dotnet Cake.Frosting.dll [switches]
```

##### Setup

Pour créer un nouveau projet Cake Frosting, vous devez installer le modèle Frosting :

```shell
dotnet new --install Cake.Frosting.Template
```

Créez un nouveau projet Frosting :

```shell
dotnet new cakefrosting
```

Cela créera le projet Cake Frosting et les scripts d'amorçage.

### Tools

Au cours d'une construction, des tâches telles que la compilation, le filtrage, les tests, etc. doivent être exécutées. Cake lui-même n'est qu'un orchestrateur de build. Pour réaliser la tâche mentionnée précédemment, Cake appelle différents outils (comme MsBuid, NUnit, etc.).

Cake prend en charge l'installation d'outils qui sont distribués sous forme de packages NuGet et fournit une logique pour rechercher des emplacements d'outils pendant l'exécution.

#### Installing tools via pre-processor directive

La directive de pré-processeur #tool pour Cake .NET Tool peut être utilisée pour télécharger automatiquement un outil et l'installer dans le dossier `tools` .

Les outils NuGet et .NET prêts à l'emploi (depuis Cake 1.1) sont pris en charge en tant que fournisseur. D'autres fournisseurs sont disponibles via [Modules](https://cakebuild.net/extensions/).

L'exemple suivant télécharge le [package xunit.runner.console](https://www.nuget.org/packages/xunit.runner.console) dans le cadre de l'exécution de votre script de génération :

```csharp
#tool "nuget:?package=xunit.runner.console&version=2.4.1"
```

#### Installing tools with InstallTool

Cake Frosting fournit une méthode `InstallTool` pour télécharger un outil et l'installer :

Les outils NuGet et .NET prêts à l'emploi (depuis Cake 1.1) sont pris en charge en tant que fournisseur. D'autres fournisseurs sont disponibles via [Modules](https://cakebuild.net/extensions/).

L'exemple suivant télécharge le [package xunit.runner.console](https://www.nuget.org/packages/xunit.runner.console) dans le cadre de l'exécution de votre script de génération :

```csharp
public class Program : IFrostingStartup
{
    public static int Main(string[] args)
    {
        // Create and run the host.
        return
            new CakeHost()
                .InstallTool(new Uri("nuget:?package=xunit.runner.console&version=2.4.1"))
                .Run(args);
    }
}
```

### Extensions

#### Addins

Les compléments peuvent fournir des alias supplémentaires à une build Cake. Ce sont des assemblys .NET livrés sous forme de packages NuGet.

##### Write custom addins

Commencez par créer un nouveau projet de bibliothèque de classes et ajoutez une référence au package Cake.Core NuGet via le gestionnaire de packages.

```shell
PM> Install-Package Cake.Core
```

Ajoutez la méthode d'alias que vous souhaitez exposer à votre script Cake. Une méthode d'alias de script est simplement une méthode d'extension pour ICakeContext qui a été marquée avec l'attribut CakeMethodAliasAttribute. La méthode peut utiliser toutes les fonctionnalités CLR standard, y compris les paramètres facultatifs.

Vous pouvez également ajouter une propriété d'alias de script, qui fonctionne de la même manière qu'une méthode d'alias de script, sauf qu'elle n'accepte aucun argument et est marquée avec l'attribut CakePropertyAliasAttribute.

```csharp
using Cake.Core;
using Cake.Core.Annotations;

public static class MyCakeExtension
{
    [CakeMethodAlias]
    public static int GetMagicNumber(this ICakeContext context, bool value)
    {
        return value ? int.MinValue : int.MaxValue;
    }

    [CakeMethodAlias]
    public static int GetMagicNumberOrDefault(this ICakeContext context, bool value, Func<int> defaultValueProvider = null)
    {
        if (value)
        {
            return int.MinValue;
        }

        return defaultValueProvider == null ? int.MaxValue : defaultValueProvider();
    }

    [CakePropertyAlias]
    public static int TheAnswerToLife(this ICakeContext context)
    {
        return 42;
    }
}
```

###### Importing Namespaces

Votre alias de script peut avoir besoin d'importer un ou plusieurs espaces de noms dans votre script Cake. Cake prend en charge l'importation automatique d'espaces de noms avec des attributs.

Le CakeNamespaceImportAttribute peut être appliqué au niveau de la méthode, de la classe ou de l'assembly, ou toute combinaison de ceux-ci.

```csharp
// Imports the Cake.Common.IO.Paths namespace into the Cake script for this method only
[CakeNamespaceImport("Cake.Common.IO.Paths")]
public static ConvertableDirectoryPath Directory(this ICakeContext context, string path)
{...}
```

```csharp
// Imports the Cake.Common.IO.Paths namespace into the Cake script for any alias method used in the class.
[CakeNamespaceImport("Cake.Common.IO.Paths")]
public static class DirectoryAliases
{...}
```

```csharp
// Imports the Cake.Common.IO.Paths namespace into the Cake script for any alias method used in the assembly.
[assembly: CakeNamespaceImport("Cake.Common.IO.Paths")]
```

###### Using the addin

Compilez l'assembly et ajoutez-y une référence dans le script de construction via la directive #r.

```csharp
#r "tools/MyCakeExtension.dll"
```

Vous devriez maintenant pouvoir appeler la méthode à partir du script.

```csharp
Task("GetSomeAnswers")
    .Does(() =>
{
    // Write the values to the console.
    Information("Magic number: {0}", GetMagicNumber(false));
    Information("The answer to life: {0}", TheAnswerToLife);
});
```

#### Modules

Les modules sont un composant spécial de Cake conçu pour augmenter, modifier ou remplacer la logique interne de Cake elle-même. Les modules peuvent être utilisés, par exemple, pour remplacer le journal de construction intégré de Cake, le coureur de processus ou le localisateur d'outils, pour n'en nommer que quelques-uns. En interne, c'est ainsi que Cake gère ses "parties mobiles", mais vous pouvez également charger des modules dans le cadre de l'exécution de votre script de construction, ce qui vous permettra de remplacer/modifier le fonctionnement de Cake dans le cadre de votre code de construction.

#### Recipes

Les scripts de génération de Cake peuvent être publiés sous forme de packages NuGet, appelés recettes. Ces packages peuvent contenir des tâches partagées et peuvent être consommés par d'autres scripts de génération.

Lors de l'utilisation de Cake .NET Tool, la directive load peut être utilisée avec le schéma nuget pour télécharger les packages Recipe NuGet et charger tous les fichiers .cake dans le dossier de contenu. L'exemple suivant charge la version 1.0.0 du package NuGet MyRecipePackage :

```csharp
#load nuget:?package=MyRecipePackage&version=1.0.0
```

Lors de l'utilisation de Cake Frosting, le package Recipe NuGet peut être référencé comme n'importe quel autre package NuGet :

```csharp
<PackageReference Include="MyRecipePackage" Version="1.0.0" />
```

## Démo

1. Présentation de la solution de démarrage

     - un projet ASP.NET MVC .NET 6 standard
     - un projet Class Library .NET 6 avec une classe contenant deux méthodes Add et Substract
     - un projet de test pour la Class Library .NET 6 avec une classe de tests contenant des tests pour les méthodes Add et Substract

2. Création Manifest Dotnet Tools

    ```shell
    dotnet new tool-manifest
    ```

3. Installation Cake

    ```shell
    dotnet tool install Cake.Tool
    ```

4. Installation Extension Cake
   1. Pour VS Code
   2. Pour Visual Studio
5. Création Fichier build.cake

    ```csharp
    var target = Argument("target", "Build");

    RunTarget(target);
    ```

6. Ajout Tasks pour :
   1. Dotnet Restore

      ```csharp
      var rootAbsoluteDir = "./";

      Task("Restore")
        .Does(() => {
          DotNetRestore(rootAbsoluteDir );
        });
      ```

   2. Dotnet Build

      ```csharp
      var configuration     = Argument("configuration", "Release");

      Task("Build")
      .IsDependentOn("Restore")
      .Does(() => {
        DotNetBuild(rootAbsoluteDir, new DotNetBuildSettings
        {
          NoRestore = true,
          Configuration = configuration
        });
      });

      ```

   3. Dotnet Tests

      ```csharp
      var testResultsFolder = "./testresults";

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
      ```

   4. Dotnet Publish

      ```csharp
      var webAppOutputFolder = "./artifacts/mywebapp";

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

      ```

   5. Cleanup

      ```csharp
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
      ```

   6. Zip Artifacts

      ```csharp
      var webAppArtifactFileName = "website.zip";
      
      Task("ZipWebAppOutput")
        .IsDependentOn("PublishWebApp")
        .Does(() => {
          var webAppFiles = GetFiles(webAppOutputFolder + "/*.*");
          Zip(webAppOutputFolder, artifactsFolder + "/" + webAppArtifactFileName, webAppFiles);
        });
      ```

   7. Dotnet Pack

      ```csharp
      var build             = Argument("build", "0");
      var revision          = Argument("revision", string.Empty);

      var myLibraryOutputFolder = "./artifacts/mylibrary"; 

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

      ```

7. Fonctionnement en local de l'intégralité du script

    ```shell
      dotnet cake
    ```

8. Azure DevOps
   1. Création d'un pipeline pour la partie CI

      ```yaml
      trigger:
      - main

      variables:
        buildConfiguration: Release
        revision: ci

      pool:
        vmImage: ubuntu-latest

      stages:
        - stage: Build
          jobs:
          - job: BuildJob
            steps:   
      ```

      1. Dotnet Tool Restore

          ```yaml
          - task: DotNetCoreCLI@2
            displayName: Dotnet Tool Restore
            inputs:
              command: custom
              custom: tool
              arguments: restore
          ```

      2. Cake

          ```yaml
          - task: Cake@2
            displayName: Cake Build Process
            inputs:
              script: build.cake          
              target: Default
              verbosity: Verbose
              arguments: |
                --configuration $(buildConfiguration) 
                --build $(Build.BuildId) 
                --revision $(revision)
          ```

      3. Publish Tests Results

          ```yaml
          - task: PublishTestResults@2
            displayName: Publish Tests Results
            inputs:
              testResultsFormat: VSTest
              testResultsFiles: '**/*.trx'
              searchFolder: $(Build.SourcesDirectory)/testresults
              testRunTitle: $(Build.BuildNumber)
              publishRunAttachments: true
            condition: always()
          ```

      4. Publish Code Coverage Results

          ```yaml
          - task: PublishCodeCoverageResults@1
            displayName: Publish Code Coverage Results
            inputs:
              codeCoverageTool: 'Cobertura'
              summaryFileLocation: $(Build.SourcesDirectory)/testresults/**/coverage.cobertura.xml
              reportDirectory: $(Build.ArtifactStagingDirectory)
            condition: always()
          ```

      5. Publish Artifacts

          ```yaml
          - task: CopyFiles@2
            displayName: Copy Files to ArtifactStagingDirectory
            inputs:
              SourceFolder: $(Build.SourcesDirectory)/artifacts
              Contents: |
                *.zip
                *.nupkg
              TargetFolder: $(Build.ArtifactStagingDirectory)
              CleanTargetFolder: true

          - task: PublishBuildArtifacts@1
            displayName: Publish Artifacts
            inputs:
              PathtoPublish: $(Build.ArtifactStagingDirectory)
              ArtifactName: drop
              publishLocation: Container
          ```

   2. Bonus : Déploiement Package Nuget

        ```yaml
        - stage: DeployPackage
          displayName: Deployment of the nuget package
          condition: |
            and
            (
              succeeded('Build'),
              eq(variables['Build.SourceBranch'], 'refs/heads/main')
            )
          jobs:
          - job: DeployPackage
            steps:
              - download: current
                displayName: 'Download the artifacts'
                artifact: 'drop'

              - task: DotNetCoreCLI@2
                displayName: Publishing the nuget Package to the Azure DevOps feed
                inputs:
                  command: 'push'
                  packagesToPush: '$(Pipeline.Workspace)/drop/**/*.nupkg'
                  nuGetFeedType: 'internal'
                  publishVstsFeed: 'afa87726-6977-4997-b7df-99c5d3cf444e/fb789773-b199-461c-a975-0f88275a8f1f'

        ```

   3. Bonus : Déploiement WebApp

        ```yaml
        - stage: DeployWebApp
          displayName: Deployment of the web application
          condition: |
            and
            (
              succeeded('Build'),
              eq(variables['Build.SourceBranch'], 'refs/heads/main')
            )
          jobs:
          - job: DeployWebApp
            steps:
              - download: current
                displayName: 'Download the artifacts'
                artifact: 'drop'

              - task: AzureRmWebAppDeployment@4
                inputs:
                  ConnectionType: 'AzureRM'
                  azureSubscription: 'Yannick WILLI – MPN(0a5e66fb-a6b6-4a7e-b419-3f7e47ae620c)'
                  appType: 'webAppLinux'
                  WebAppName: 'democake-ywi-devops'
                  packageForLinux: '$(Pipeline.Workspace)/drop/**website.zip'
        ```

9. GitHub
   1. Création d'un pipeline pour la partie CI

      ```yaml
      name: Cake CI

      on:
        push:
          branches: [ main ]
        pull_request:
          branches: [ main ]

      env:
        AZURE_WEBAPP_NAME: democake-ywi-github
        buildConfiguration: Release
        revision: ci

      jobs:
        build:
          runs-on: ubuntu-latest
          steps:
            - uses: actions/checkout@v2

            
      ```

      1. Dotnet Tool Restore

          ```yaml
          - name: Dotnet Tool Restore
            run: dotnet tool restore
          ```

      2. Cake

          ```yaml
          - name: Set the Build Id
            id: get-id
            run: |
              buildid="${{github.run_id}}"
              echo "BUILD_ID=${buildid: -5}" >> $GITHUB_OUTPUT

          - name: Cake Build Process
            uses: cake-build/cake-action@v1.4.1
            with:
              target: Default
              verbosity: Diagnostic
              arguments: |
                configuration: ${{ env.buildConfiguration }}
                build: ${{ steps.get-id.outputs.BUILD_ID }}
                revision: ${{ env.revision }}
          ```

      3. Publish Tests Results

          ```yaml
          - name: Publish Tests Results
            uses: dorny/test-reporter@v1.6.0
            if: always()
            with:
              name: Tests Results
              path: testresults/*.trx
              reporter: dotnet-trx
          ```

      4. Publish Code Coverage Results

          ```yaml
          - name: Publish Code Coverage Results
            uses: irongut/CodeCoverageSummary@v1.3.0
            if: always()
            with: 
              filename: testresults/**/coverage.cobertura.xml
              badge: true
              output: both
              format: markdown
              indicators: true

          - name: Add Coverage PR Comment
            uses: marocchino/sticky-pull-request-comment@v2
            if: github.event_name == 'pull_request'
            with:
              recreate: true
              path: code-coverage-results.md
          ```

      5. Publish Artifacts

          ```yaml
          - name: Publish Website Artifacts
            uses: actions/upload-artifact@main
            with:
              name: website
              path: |
                artifacts/website.zip
                artifacts/*.nupkg

          - name: Publish MyLibrary Artifacts
            uses: actions/upload-artifact@main
            with:
              name: package
              path: |
                artifacts/*.nupkg
          ```

   2. Bonus : Déploiement Package Nuget vers GitHub Packages

        ```yaml
        deployPackage:
          needs: build
          if: github.ref_name == 'main'
          runs-on: ubuntu-latest
          steps:
          - uses: actions/checkout@v2
          - name: Download MyLibrary Artifacts
            uses: actions/download-artifact@main
            with: 
              name: package
          - run: echo Deploying the Nuget Package
          - name: Adding GitHub as a NuGet Package Feed
            run: dotnet nuget add source --username actarus108 --password ${{ secrets.GITHUB_TOKEN }} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/actarus108/index.json"
          - name: Publishing the nuget Package to GitHub Packages
            run: dotnet nuget push "MyLibrary.*.nupkg"  --api-key ${{ secrets.GITHUB_TOKEN }} --source "github"

        ```

   3. Bonus : Déploiement WebApp

        ```yaml
        deployWebApp:
          needs: build
          if: github.ref_name == 'main'
          runs-on: ubuntu-latest
          steps:
          - uses: actions/checkout@v2
          - name: Download Website Artifacts
            uses: actions/download-artifact@main
            with: 
              name: website    
          - name: 'Deploying the Web Application'
            uses: azure/webapps-deploy@v2
            with: 
              app-name: ${{ env.AZURE_WEBAPP_NAME }}
              publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
              package: 'website.zip'
        ```
