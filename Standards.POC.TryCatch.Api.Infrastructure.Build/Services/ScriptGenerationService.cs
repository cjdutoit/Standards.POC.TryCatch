using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace Standards.POC.TryCatch.Api.Services
{
    public class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            this.adotNetClient = new ADotNetClient();

        public void GenerateBuildScript()
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "Build",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.Windows2019,

                        Steps = new List<GithubTask>
                    {
                        new CheckoutTaskV2
                        {
                            Name = "Check Out"
                        },

                        new SetupDotNetTaskV1
                        {
                            Name = "Setup Dot Net Version",

                            TargetDotNetVersion = new TargetDotNetVersion
                            {
                                DotNetVersion = "7.0.100-preview.4.22252.9",
                                IncludePrerelease = true
                            }
                        },

                        new RestoreTask
                        {
                            Name = "Restore"
                        },

                        new DotNetBuildTask
                        {
                            Name = "Build"
                        },

                        new TestTask
                        {
                            Name = "Test"
                        }
                    }
                    }
                }
            };

            string yamlRelativeFilePath = "../../../../.github/workflows/build.yml";
            string yamlFullPath = System.IO.Path.GetFullPath(yamlRelativeFilePath);
            FileInfo yamlDefinition = new FileInfo(yamlFullPath);

            if (!yamlDefinition.Directory.Exists)
            {
                yamlDefinition.Directory.Create();
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: yamlRelativeFilePath);
        }

        public void GenerateProvisionScript()
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "Provision",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.WindowsLatest,

                        EnvironmentVariables = new Dictionary<string, string>
                        {
                            { "AzureClientId", "${{ secrets.AZURECLIENTID }}" },
                            { "AzureTenantId", "${{ secrets.AZURETENANTID }}" },
                            { "AzureClientSecret", "${{ secrets.AZURECLIENTSECRET }}" },
                            { "AzureAdminName", "${{ secrets.AZUREADMINNAME }}" },
                            { "AzureAdminAccess", "${{ secrets.AZUREADMINACCESS }}" }
                        },

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Check Out"
                            },

                            new SetupDotNetTaskV1
                            {
                                Name = "Setup Dot Net Version",

                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "7.0.100-preview.1.22110.4",
                                    IncludePrerelease = true
                                }
                            },

                            new RestoreTask
                            {
                                Name = "Restore"
                            },

                            new DotNetBuildTask
                            {
                                Name = "Build"
                            },

                            new RunTask
                            {
                                Name = "Provision",
                                Run = "dotnet run --project .\\Standards.POC.TryCatch.Api.Infrastructure.Provision\\Standards.POC.TryCatch.Api.Infrastructure.Provision.csproj"
                            }
                        }
                    }
                }
            };

            string yamlRelativeFilePath = "../../../../.github/workflows/provision.yml";
            string yamlFullPath = System.IO.Path.GetFullPath(yamlRelativeFilePath);
            FileInfo yamlDefinition = new FileInfo(yamlFullPath);

            if (!yamlDefinition.Directory.Exists)
            {
                yamlDefinition.Directory.Create();
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: yamlRelativeFilePath);
        }
    }
}