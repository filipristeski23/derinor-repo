import { useEffect, useState } from "react";
import { createProjectService } from "../services/createProjectService";

export const useRepositories = () => {
  const [repositories, setRepositories] = useState([]);

  useEffect(() => {
    const fetchRepositories = async () => {
      const data = await createProjectService.fetchRepositories();
      const formattedRepoData = data.map((repo) => ({
        repoID: repo.id,
        repoName: repo.name,
      }));
      setRepositories(formattedRepoData);
    };

    fetchRepositories();
  }, []);
  return { repositories };
};
