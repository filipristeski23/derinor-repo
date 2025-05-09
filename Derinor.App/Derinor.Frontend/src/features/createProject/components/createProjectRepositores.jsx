import React from "react";
import { useNavigate } from "react-router-dom";
import { useOutletContext } from "react-router-dom";
import { useRepositories } from "../../createProject/hooks/useRepositories";

export default function CreateProjectRepositories() {
  const { selectRepository } = useOutletContext();
  const { repositories } = useRepositories();
  const navigate = useNavigate();

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
                className="cursor-pointer bg-[#3D6BC6] h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center inline-flex justify-between w-auto text-[#F8FAFC]"
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
