import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../app/axiosInstance";
import { useCreateProjectStore } from "../store/createProjectStore";

export default function CreateProjectRepositories() {
  const [repositories, setRepositories] = useState([]);
  const [selectedRepo, setSelectedRepo] = useState(null);
  const selectRepositoryInStore = useCreateProjectStore(
    (state) => state.selectRepository
  );
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRepositories = async () => {
      try {
        const response = await api.get("projects/fetch-repositories");
        const formattedRepoData = response.data.map((repo) => ({
          repoID: repo.id,
          repoName: repo.name,
        }));
        setRepositories(formattedRepoData);
      } catch (error) {
        console.error("Failed to fetch repositories", error);
      }
    };

    fetchRepositories();
  }, []);

  const handleRepoClick = (repo) => {
    setSelectedRepo(repo);
  };

  const handleNext = () => {
    selectRepositoryInStore(selectedRepo);
    navigate("/projects/create-project/branches");
  };

  return (
    <div className="flex flex-col gap-[2rem] flex-grow min-h-0">
      <h2 className="text-[#23272A] font-bold text-[1.75rem] sm:text-[2rem] flex-shrink-0">
        Select a repository
      </h2>
      <div className="flex-grow min-h-0 overflow-y-auto">
        <div className="flex flex-col gap-[1rem]">
          {repositories.length > 0 ? (
            repositories.map((repo) => (
              <div
                key={repo.repoID}
                onClick={() => handleRepoClick(repo)}
                className={`cursor-pointer h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center justify-between w-auto text-[#F8FAFC] ${
                  selectedRepo?.repoID === repo.repoID
                    ? "bg-[#D570CC]"
                    : "bg-[#3D6BC6]"
                }`}
              >
                <label className="cursor-pointer">{repo.repoName}</label>
                <span className="text-lg">→</span>
              </div>
            ))
          ) : (
            <p className="text-gray-500">Loading repositories...</p>
          )}
        </div>
      </div>
      <div className="flex-shrink-0 pt-[1rem]">
        <button
          onClick={handleNext}
          disabled={!selectedRepo}
          className="bg-[#3D6BC6] h-[2.5rem] w-full sm:w-[11.125rem] text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem] disabled:bg-gray-400 disabled:cursor-not-allowed"
        >
          Next
        </button>
      </div>
    </div>
  );
}
