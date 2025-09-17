using Derinor.Common.RequestDTOs;
using Derinor.Common.ResponseDTOs;
using Derinor.Domain.Models;
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

        Task<GeneratedReportDataRequestDTO> GenerateReport(int userID, GenerateReportRequestDTO request, Projects fetchDetails);
        Task<List<GithubCommitResponseDTO>> GetGithubCommits(int userID, int projectID, DateTime startDate, DateTime endDate, Projects details);

        Task<GeminiDataResponseDTO> GetGeminiData(int userID, GenerateReportRequestDTO request);

        Task<List<GithubRepositoryResponseDTO>> FetchRepositories(int userID);

        Task<List<GithubBranchesResponseDTO>> FetchBranches(int userID, string repositoryName);

        Task PublishProject(PublishProjectDTO publishProjectDTO);

        Task<List<ProjectReportDTO>> GetReportsByProject(int projectID);
    }
}
