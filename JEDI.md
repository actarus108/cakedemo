# JEDI EXPACEO 03/11/2022

**Cake - Vous reprendrez bien une part de gâteau ?**

Comment optimiser son processus de CI/CD avec un outil d'automatisation de build multiplateforme gratuit et open source avec un DSL C# pour des tâches telles que la compilation de code, la copie de fichiers et de dossiers, l'exécution de tests unitaires, la compression de fichiers et la création de packages NuGet. Vous pouvez ensuite utiliser ce système pour construire votre projet de la même façon sur Azure DevOps, Jenkins, TeamCity, GitHub Actions ou sur le système de build de votre choix.

## Présentation

### Cake c'est quoi ?

Cake est un outil d'automatisation de build multiplateforme gratuit et open source avec un DSL C# pour des tâches telles que la compilation de code, la copie de fichiers et de dossiers, l'exécution de tests unitaires, la compression de fichiers et la création de packages NuGet. 
Vous pouvez ensuite utiliser ce système pour construire votre projet de la même façon sur Azure DevOps, Jenkins, TeamCity, GitHub Actions ou sur le système de build de votre choix.

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

Cake .NET Tool is a runner which allows to run Cake scripts.

##### Requirements

The Cake.Tool NuGet package, is a .NET Core tool compiled for .NET Core 3.1 or newer.

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

Cake Frosting is a .NET host which allows you to write your build scripts as a console application.

##### Requirements

Cake.Frosting can be used to write console applications targeting netcoreapp3.1 or newer.

##### Usage

```shell
dotnet Cake.Frosting.dll [switches]
```

##### Setup

To create a new Cake Frosting project you need to install the Frosting template:

```shell
dotnet new --install Cake.Frosting.Template
```

Create a new Frosting project:

```shell
dotnet new cakefrosting
```

This will create the Cake Frosting project and bootstrapping scripts.

### Tools

During a build tasks like compiling, linting, testing, etc need to be execute. Cake itself is only a build orchestrator. For achieving the previously mentioned task Cake calls different tools (like MsBuid, NUnit, etc).

Cake supports installing tools which are distributed as NuGet packages and provides logic to find tool locations during runtime.

#### Installing tools via pre-processor directive

The #tool pre-processor directive for Cake .NET Tool can be used to automatically download a tool and install it in the `tools` folder.

Out of the box NuGet and .NET Tools (since Cake 1.1) are supported as provider. More providers are available through [Modules](https://cakebuild.net/extensions/).

The following example downloads the [xunit.runner.console package](https://www.nuget.org/packages/xunit.runner.console) as part of executing your build script:

```csharp
#tool "nuget:?package=xunit.runner.console&version=2.4.1"
```

#### Installing tools with InstallTool

Cake Frosting provides a `InstallTool` method to download a tool and install it:

Out of the box NuGet and .NET Tools (since Cake 1.1) are supported as provider. More providers are available through [Modules](https://cakebuild.net/extensions/).

The following example downloads the [xunit.runner.console package](https://www.nuget.org/packages/xunit.runner.console) as part of executing your build script:

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

Addins can provide additional aliases to a Cake build. They are .NET assemblies shipped as NuGet packages.

##### Write custom addins

TODO: To Complete

#### Modules

Modules are a special Cake component designed to augment, change or replace the internal logic of Cake itself. Modules can be used, for example, to replace the built-in Cake build log, process runner or tool locator, just to name a few. Internally, this is how Cake manages its "moving parts", but you can also load modules as part of running your build script, which will allow you to replace/change how Cake works as part of your build code.

##### Write custom modules

TODO: To Complete

#### Recipes

Cake build scripts can be published as NuGet packages, so called Recipes. These packages can contain shared tasks and can be consumed by other build scripts.

When using Cake .NET Tool, the load directive can be used with the nuget scheme to download the Recipe NuGet packages and load all .cake files in the content folder. The following example loads version 1.0.0 of the MyRecipePackage NuGet package:

```csharp
#load nuget:?package=MyRecipePackage&version=1.0.0
```

When using Cake Frosting, Recipe NuGet package can be referenced like any other NuGet package:

```csharp
<PackageReference Include="MyRecipePackage" Version="1.0.0" />
```

## Démo

TODO: Création projet en mode démarrage avec repo dédié sur AzureDevops et repo dédié sur Github.

1. Présentation de la solution de démarrage
2. Création Manifest Dotnet Tools
3. Installation Cake
4. Création Fichier build.cake
5. Ajout Tasks pour :
   1. Dotnet Restore
   2. Dotnet Build
   3. Dotnet Tests
   4. Dotnet Publish
   5. Cleanup
   6. Zip Artifacts
   7. Dotnet Pack
6. Fonctionnement en local de l'intégralité du script
7. Azure DevOps
   1. Création d'un pipeline pour la partie CI
      1. Dotnet Tool Restore
      2. Cake
      3. Publish Tests Results
      4. Publish Code Coverage Results
      5. Publish Artifacts
      6. Bonus : Déploiement Package Nuget
      7. Bonus : Déploiement WebApp
8. GitHub
   1. Création d'un pipeline pour la partie CI
      1. Dotnet Tool Restore
      2. Cake
      3. Publish Tests Results
      4. Publish Code Coverage Results
      5. Publish Artifacts
      6. Bonus : Déploiement Package Nuget vers GitHub Packages
      7. Bonus : Déploiement WebApp
