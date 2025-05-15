import { useState } from "react";
import { createProjectService } from "../services/createProjectService";
import { useEffect } from "react";

export const useSendProjectDetails = () => {
  const [projectData, setProjectData] = useState({
    projectOwner: "",
    projectDescription: "",
    projectName: "",
    startingDate: new Date().toISOString(),
    projectBranches: {
      projectRepository: "",
      projectProductionBranch: "",
    },
  });

  const [selectedRepository, setSelectedRepository] = useState(null);

  const selectRepository = (repo) => {
    setSelectedRepository(repo);
    setProjectData((prev) => ({
      ...prev,
      projectBranches: {
        ...prev.projectBranches,
        projectRepository: repo.repoName,
      },
    }));
  };

  const updateProjectDetails = (details) => {
    setProjectData((prev) => ({
      ...prev,
      ...details,
    }));
  };

  const updateBranchStatuses = (branch) => {
    setProjectData((prev) => ({
      ...prev,
      projectBranches: {
        ...prev.projectBranches,
        projectProductionBranch: branch.branchName,
      },
    }));
  };

  useEffect(() => {
    console.log("projectData updated:", projectData);
  }, [projectData]);

  const sendProject = async () => {
    await createProjectService.postProjectDetails(projectData);
  };

  return {
    projectData,
    selectedRepository,
    setSelectedRepository,
    updateProjectDetails,
    selectRepository,
    updateBranchStatuses,
    setProjectData,
    sendProject,
  };
};
