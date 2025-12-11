import { create } from "zustand";
import api from "../../../app/axiosInstance";

const initialState = {
  projectOwner: "",
  projectName: "",
  projectDescription: "",
  startingDate: new Date().toISOString(),
  projectBranches: {
    projectRepository: "",
    projectProductionBranch: "",
  },
};

export const useCreateProjectStore = create((set, get) => ({
  projectData: initialState,
  isCreating: false,
  updateProjectDetails: (details) =>
    set((state) => ({
      projectData: { ...state.projectData, ...details },
    })),
  selectRepository: (repo) =>
    set((state) => ({
      projectData: {
        ...state.projectData,
        projectBranches: {
          ...state.projectData.projectBranches,
          projectRepository: repo.repoName,
        },
      },
    })),
  selectBranch: (branch) =>
    set((state) => ({
      projectData: {
        ...state.projectData,
        projectBranches: {
          ...state.projectData.projectBranches,
          projectProductionBranch: branch.name,
        },
      },
    })),
  createProject: async () => {
    const { projectData } = get();
    set({ isCreating: true });
    try {
      await api.post("projects/create-project", projectData);
      set({ isCreating: false });
      return true;
    } catch (error) {
      console.error("Failed to create project:", error);

      const msg =
  typeof error.response?.data === "string"
    ? error.response.data
    : error.response?.data?.message ||
      error.response?.data?.error ||
      "An error occurred";

alert(msg);

      set({ isCreating: false });
      return { success: false, error: true };
    }
  },
  reset: () => set({ projectData: initialState }),
}));
