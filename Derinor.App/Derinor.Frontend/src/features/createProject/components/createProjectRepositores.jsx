import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../app/axiosInstance";
import { useCreateProjectStore } from "../store/createProjectStore";

export default function CreateProjectRepositories() {
  const selectRepository = useCreateProjectStore(
    (state) => state.selectRepository
  );
  const [repositories, setRepositories] = useState([]);
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

  const handleRepoClick = (repo) => (e) => {
    e.preventDefault();
    selectRepository(repo);
    navigate("/projects/create-project/branches");
  };

  return (
    <div className="flex flex-col gap-[2rem]">
      <h2 className="text-[#23272A] font-bold text-[2rem]">
        Select a repository
      </h2>
      <div>
        <div className="flex flex-col gap-[1rem]">
          {repositories.length > 0 ? (
            repositories.map((repo) => (
              <div
                key={repo.repoID}
                onClick={handleRepoClick(repo)}
                className="cursor-pointer bg-[#3D6BC6] h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center justify-between w-auto text-[#F8FAFC]"
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
    </div>
  );
}
