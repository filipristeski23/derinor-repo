import { useState } from "react";
import { useEffect } from "react";

export const useSendProjectDetails = () => {
  const [projectData, setProjectData] = useState({
    projectOwner: "",
    projectDescription: "",
    projectName: "",
    repositoryID: "",
    repositoryName: "",
    branchID: "",
    branchName: "",
  });

  const [selectedRepository, setSelectedRepository] = useState(null);

  const selectRepository = (repo) => {
    setSelectedRepository(repo);
    setProjectData((prev) => ({
      ...prev,
      repositoryID: repo.repoID,
      repositoryName: repo.repoName,
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
      branchID: branch.branchID,
      branchName: branch.branchName,
    }));
  };

  useEffect(() => {
    console.log("projectData in browser console:", projectData);
  }, [projectData]);

  return {
    projectData,
    selectedRepository,
    setSelectedRepository,
    updateProjectDetails,
    selectRepository,
    updateBranchStatuses,
    setProjectData,
  };
};
