import { create } from "zustand";
import { geminiDataService } from "../services/geminiDataService";

export const useGenerateReportStore = create((set, get) => ({
  projectID: null,
  geminiData: null,

  setProjectID: (id) => set({ projectID: id, geminiData: null }),

  fetchGeminiData: async () => {
    const { projectID } = get();
    if (!projectID) return;
    const data = await geminiDataService.fetchGeminiData(projectID);
    set({ geminiData: data });
  },

  clearData: () => set({ projectID: null, geminiData: null }),
}));
