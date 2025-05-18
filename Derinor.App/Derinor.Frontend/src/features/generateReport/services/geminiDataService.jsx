import axios from "axios";

const api = `https://localhost:7113/`;

export const geminiDataService = {
  fetchGeminiData: async (projectID) => {
    if (!projectID) throw new Error("ProjectID is required");
    const response = await axios.get(`${api}projects/get-gemini-data`, {
      params: { projectID: projectID },
    });
    return response.data;
  },
};
