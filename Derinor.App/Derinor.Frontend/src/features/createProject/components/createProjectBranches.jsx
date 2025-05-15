import React from "react";
import { useBranches } from "../hooks/useBranches";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { useOutletContext } from "react-router-dom";
import { useState } from "react";

export default function CreateProjectBranches() {
  const navigate = useNavigate();
  const { selectedRepository, sendProject, updateBranchStatuses } =
    useOutletContext();
  const branches = useBranches(selectedRepository?.repoName);
  const [selectedBranchName, setSelectedBranchName] = useState("");

  useEffect(() => {
    if (!selectedRepository) {
      navigate("../repositories");
    }
  }, [selectedRepository, navigate]);

  const handleSelect = (e) => {
    const name = e.target.value;
    setSelectedBranchName(name);
    updateBranchStatuses({ branchName: name });
  };

  return (
    <div className="flex flex-col gap-[2rem]">
      <h2 className="text-[#23272A] font-bold text-[2rem]">
        Select production branch
      </h2>
      <div>
        <div className="flex flex-col gap-[1rem]">
          <div className="flex flex-col gap-[0.5rem]">
            <div className="flex flex-col gap-[1rem] h-[20rem] overflow-hidden">
              {branches.length > 0 ? (
                <div className="flex flex-col gap-[1rem]">
                  {branches.map((branch) => (
                    <label
                      key={branch.branchName}
                      className="cursor-pointer flex items-center bg-[#3D6BC6] h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] text-[#F8FAFC]"
                    >
                      <input
                        type="radio"
                        name="productionBranch"
                        value={branch.branchName}
                        checked={selectedBranchName === branch.branchName}
                        onChange={handleSelect}
                        className="form-radio appearance-none"
                      />
                      <span className="font-[1rem] text-[#F8FAFC]">
                        {branch.branchName}
                      </span>
                    </label>
                  ))}
                </div>
              ) : (
                <p className="text-gray-500">No branches available</p>
              )}
            </div>
          </div>
          <div className="flex flex-col gap-[2rem] items-start">
            <button
              onClick={sendProject}
              className="bg-[#3D6BC6] text-white h-[2.5rem] px-[1.5rem] rounded-[0.5rem] cursor-pointer font-regular inline-block"
            >
              Finish
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
