using Derinor.Common.RequestDTOs;
using Derinor.Common.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.Application.ServiceInterfaces
{
    public interface IProjectsService
    {
        Task<List<ProjectHomeResponseDTO>> AllProjects(string? search);

        Task CreateProject(CreateProjectDetailsRequestDTO projectDetails, int userID);

        Task<GeneratedReportDataRequestDTO> GenerateReport(int userID, int projectID);

        Task<List<GithubCommitResponseDTO>> GetGithubCommits(int userID, int projectID);

        Task<GeminiDataResponseDTO> GetGeminiData(int userID, int projectID);

        Task<List<GithubRepositoryResponseDTO>> FetchRepositories(int userID);

        Task<List<GithubBranchesResponseDTO>> FetchBranches(int userID, string repositoryName);
    }
}
