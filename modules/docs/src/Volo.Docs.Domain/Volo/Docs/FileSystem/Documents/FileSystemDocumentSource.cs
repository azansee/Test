using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Volo.Abp.Domain.Services;
using Volo.Abp.IO;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.FileSystem.Documents
{
    public class FileSystemDocumentSource : DomainService, IDocumentSource
    {
        public const string Type = "FileSystem";

        public async Task<Document> GetDocumentAsync(Project project, string documentName, string languageCode, string version)
        {
            var projectFolder = project.GetFileSystemPath();
            var path = Path.Combine(projectFolder, languageCode, documentName);

            CheckDirectorySecurity(projectFolder, path);
            
            var content = await FileHelper.ReadAllTextAsync(path);
            var localDirectory = "";

            if (documentName.Contains("/"))
            {
                localDirectory = documentName.Substring(0, documentName.LastIndexOf('/'));
            }

            return new Document(GuidGenerator.Create(),
                project.Id,
                documentName,
                version,
                languageCode,
                Path.GetFileName(path),
                content,
                project.Format,
                path,
                "/",
                $"/document-resources?projectId={project.Id.ToString()}&version={version}&languageCode={languageCode}&name=",
                localDirectory,
                File.GetCreationTime(path),
                File.GetLastWriteTime(path),
                DateTime.Now);
        }

        public Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            return Task.FromResult(new List<VersionInfo>());
        }

        public async Task<LanguageConfig> GetLanguageListAsync(Project project, string version)
        {
            var path = Path.Combine(project.GetFileSystemPath(), "docs-langs.json");
            var configAsJson = await FileHelper.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject<LanguageConfig>(configAsJson);
        }

        public async Task<DocumentResource> GetResource(Project project, string resourceName, string languageCode, string version)
        {
            var projectFolder = project.GetFileSystemPath();
            var path = Path.Combine(projectFolder, languageCode, resourceName);

            if (!DirectoryHelper.IsSubDirectoryOf(projectFolder, path))
            {
                throw new SecurityException("Can not get a resource file out of the project folder!");
            }

            return new DocumentResource(await FileHelper.ReadAllBytesAsync(path));
        }

        private static void CheckDirectorySecurity(string projectFolder, string path)
        {
            if (!DirectoryHelper.IsSubDirectoryOf(projectFolder, path))
            {
                throw new SecurityException("Can not get a resource file out of the project folder!");
            }
        }
    }
}
