import { useState } from "react";
import { useEffect } from "react";
import { searchProjectService } from "../services/searchProjectService";

export const useSearchProjects = () => {
  const [searchProjectData, setSearchProjectData] = useState("");
  const [projectData, setProjectData] = useState([]);

  useEffect(() => {
    const fetchProjects = async () => {
      const fetchedProjectData = await searchProjectService.fetchProjects(
        searchProjectData
      );
      setProjectData(fetchedProjectData);
    };

    fetchProjects();
  }, [searchProjectData]);

  return { searchProjectData, setSearchProjectData, projectData };
};
