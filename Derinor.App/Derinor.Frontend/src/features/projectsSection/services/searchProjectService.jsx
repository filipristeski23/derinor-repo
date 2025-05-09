import axios from "axios";

const api = `https://e3a13a61-b8f7-421a-9035-4cd12976dc4e.mock.pstmn.io/api`;

export const searchProjectService = {
  fetchProjects: async (searchProjectData) => {
    const projectsData = await axios.get(`${api}/projects`, {
      params: searchProjectData ? { search: searchProjectData } : {},
    });
    return projectsData.data;
  },
};
