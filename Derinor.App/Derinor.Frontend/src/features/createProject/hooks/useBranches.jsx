import { useState, useEffect } from "react";
import { createProjectService } from "../services/createProjectService";

export const useBranches = (repositoryName) => {
  const [branches, setBranches] = useState([]);

  useEffect(() => {
    const fetchBranches = async () => {
      const data = await createProjectService.fetchBranches(repositoryName);
      const formattedBranchData = data.map((branch) => ({
        branchID: branch.name,
        branchName: branch.name,
      }));

      setBranches(formattedBranchData);
    };
    fetchBranches();
  }, [repositoryName]);
  return branches;
};
