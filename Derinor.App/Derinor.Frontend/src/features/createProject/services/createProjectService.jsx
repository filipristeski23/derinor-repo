import axios from "axios";

const api = `https://localhost:7113/`;

export const createProjectService = {
  fetchRepositories: async () => {
    const response = await axios.get(`${api}projects/fetch-repositories`);
    return response.data;
  },

  fetchBranches: async (repositoryName) => {
    const response = await axios.get(`${api}projects/fetch-branches`, {
      params: { repositoryName },
    });

    return response.data;
  },

  postProjectDetails: async (projectData) => {
    await axios.post(`${api}projects/create-project`, projectData);
  },
};
