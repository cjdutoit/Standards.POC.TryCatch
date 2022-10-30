using Standards.POC.TryCatch.Api.Services;

var scriptGenerationService = new ScriptGenerationService();
scriptGenerationService.GenerateBuildScript();
scriptGenerationService.GenerateProvisionScript();