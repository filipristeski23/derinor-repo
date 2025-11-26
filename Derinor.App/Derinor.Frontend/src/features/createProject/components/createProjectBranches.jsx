import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../app/axiosInstance";
import { useCreateProjectStore } from "../store/createProjectStore";
import { useProjectRefreshStore } from "../store/projectRefreshStore";
import BackToRepositories from "./backToRepositoriesButton";

export default function CreateProjectBranches() {
  const navigate = useNavigate();
  const projectRepository = useCreateProjectStore(
    (state) => state.projectData.projectBranches.projectRepository
  );
  const productionBranch = useCreateProjectStore(
    (state) => state.projectData.projectBranches.projectProductionBranch
  );
  const selectBranch = useCreateProjectStore((state) => state.selectBranch);
  const createProject = useCreateProjectStore((state) => state.createProject);
  const isCreating = useCreateProjectStore((state) => state.isCreating);
  const triggerRefresh = useProjectRefreshStore(
    (state) => state.triggerRefresh
  );

  const [branches, setBranches] = useState([]);

  useEffect(() => {
    if (!projectRepository) {
      navigate("/projects/create-project/repositories");
      return;
    }

    const fetchBranches = async () => {
      try {
        const response = await api.get("projects/fetch-branches", {
          params: { repositoryName: projectRepository },
        });
        setBranches(response.data);
      } catch (error) {
        console.error("Failed to fetch branches:", error);
      }
    };

    fetchBranches();
  }, [projectRepository, navigate]);

  const handleBranchClick = (branch) => (e) => {
    e.preventDefault();
    selectBranch(branch);
  };

  const handleFinish = async () => {
    const success = await createProject();
    if (success) {
      triggerRefresh();
      navigate("/projects", { replace: true });
    }
  };

  return (
    <div className="flex flex-col gap-[2rem] flex-grow min-h-0">
      <h2 className="text-[#23272A] font-bold text-[1.75rem] sm:text-[2rem] flex-shrink-0">
        Select production branch
      </h2>
      <div className="flex-grow min-h-0 overflow-y-auto">
        <div className="flex flex-col gap-[1rem]">
          {branches.length > 0 ? (
            branches.map((branch) => (
              <div
                key={branch.name}
                onClick={handleBranchClick(branch)}
                className={`cursor-pointer h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center w-auto text-[#F8FAFC] ${
                  productionBranch === branch.name
                    ? "bg-[#D570CC]"
                    : "bg-[#3D6BC6]"
                }`}
              >
                <label className="cursor-pointer">{branch.name}</label>
              </div>
            ))
          ) : (
            <p className="text-gray-500">Loading branches...</p>
          )}
        </div>
      </div>
      <div className="flex flex-col sm:flex-row gap-[1rem] flex-shrink-0 pt-[1rem]">
        <BackToRepositories />
        <button
          onClick={handleFinish}
          disabled={!productionBranch || isCreating}
          className="bg-[#3D6BC6] h-[2.5rem] w-full sm:w-[11.125rem] text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] disabled:bg-gray-400"
        >
          {isCreating ? "..." : "Finish"}
        </button>
      </div>
    </div>
  );
}
