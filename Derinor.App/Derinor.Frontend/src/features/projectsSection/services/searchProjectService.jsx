import axios from "axios";

const api = `https://localhost:7113/`;

export const searchProjectService = {
  fetchProjects: async (searchProjectData) => {
    const projectsData = await axios.get(`${api}projects/all-projects`, {
      params: searchProjectData ? { search: searchProjectData } : {},
    });
    return projectsData.data;
  },
};
