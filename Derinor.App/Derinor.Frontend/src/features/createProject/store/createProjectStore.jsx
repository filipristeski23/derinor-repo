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
    console.log("Final Project Data being sent:", projectData);
    try {
      await api.post("projects/create-project", projectData);
      return true;
    } catch (error) {
      console.error("Failed to create project:", error);
      return false;
    }
  },

  reset: () => set({ projectData: initialState }),
}));
