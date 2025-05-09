import axios from "axios";

const api = `https://e3a13a61-b8f7-421a-9035-4cd12976dc4e.mock.pstmn.io/api`;

export const createProjectService = {
  fetchRepositories: async () => {
    const response = await axios.get(`${api}/repositories`);
    return response.data;
  },

  fetchBranches: async (repositoryName) => {
    const response = await axios.get(
      `${api}/repositories/${repositoryName}/branches`
    );
    return response.data;
  },

  postProjectDetails: async (projectData) => {
    await axios.post(`${api}/create-project`, projectData);
  },
};
