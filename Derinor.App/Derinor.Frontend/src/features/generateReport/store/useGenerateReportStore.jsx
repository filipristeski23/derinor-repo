import { create } from "zustand";
import api from "../../../app/axiosInstance";

export const useGenerateReportStore = create((set, get) => ({
  projectID: null,
  startDate: null,
  endDate: null,
  geminiData: null,
  isLoading: false,
  isPublishing: false,

  setProjectID: (id) =>
    set({ projectID: id, geminiData: null, startDate: null, endDate: null }),

  setDateRange: (dates) => {
    const [start, end] = dates;
    set({ startDate: start, endDate: end });
  },

  fetchGeminiData: async () => {
    const { projectID, startDate, endDate } = get();
    if (!projectID || !startDate || !endDate) return;

    set({ isLoading: true, geminiData: null });

    try {
      const requestBody = {
        projectID: parseInt(projectID, 10),
        startDate,
        endDate,
      };
      const response = await api.post("projects/get-gemini-data", requestBody);
      set({ geminiData: response.data, isLoading: false });
    } catch (error) {
      console.error("Failed to fetch Gemini data:", error);
      set({ isLoading: false });
    }
  },

  publishReport: async (reportContent) => {
    const { projectID } = get();
    if (!projectID || !reportContent) return;

    set({ isPublishing: true });

    try {
      const requestBody = {
        projectID: parseInt(projectID, 10),
        reportContent: reportContent,
      };
      await api.post("projects/publish-report", requestBody);
      set({ isPublishing: false });
      alert("Report published successfully!");
    } catch (error) {
      console.error("Failed to publish report:", error);
      set({ isPublishing: false });
      alert("Failed to publish report.");
    }
  },

  clearData: () =>
    set({
      projectID: null,
      geminiData: null,
      startDate: null,
      endDate: null,
    }),
}));
