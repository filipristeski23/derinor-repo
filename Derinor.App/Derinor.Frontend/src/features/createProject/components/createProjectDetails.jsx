import React from "react";
import NextToSetBranchesButton from "./nextToSetBranchesButton";
import { useOutletContext } from "react-router-dom";

export default function CreateProjectDetails() {
  const { projectData, updateProjectDetails } = useOutletContext();

  const handleInputChange = (field) => (e) => {
    updateProjectDetails({ [field]: e.target.value });
  };

  return (
    <div className="flex flex-col gap-[2rem]">
      <h2 className="text-[#23272A] font-bold text-[2rem]">
        Create your project
      </h2>
      <div>
        <div className="flex flex-col gap-[1rem]">
          <div className="flex flex-col gap-[0.5rem]">
            <h4 className="text-[#23272A] font-semibold text-[1rem]">
              Project Owner
            </h4>
            <div className="flex gap-[1rem]">
              <label className="cursor-pointer bg-[#D570CC] h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center inline-flex w-auto text-[#F8FAFD]">
                <input
                  type="radio"
                  name="projectOwner"
                  value="Client"
                  checked={projectData.projectOwner === "Client"}
                  onChange={handleInputChange("projectOwner")}
                  className="hidden"
                />
                Client
              </label>
              <label className="cursor-pointer bg-[#D570CC] h-[2.5rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center inline-flex w-auto text-[#F8FAFD]">
                <input
                  type="radio"
                  name="projectOwner"
                  value="Mine"
                  checked={projectData.projectOwner === "Mine"}
                  onChange={handleInputChange("projectOwner")}
                  className="hidden"
                />
                Mine
              </label>
            </div>
          </div>
          <div className="flex flex-col gap-[2rem]">
            <div className="flex flex-col gap-[1rem]">
              <div className="flex flex-col gap-[0.5rem]">
                <label className="text-[#23272A] font-semibold text-[1rem]">
                  Project Name
                </label>
                <input
                  type="text"
                  placeholder="Jobsky Web App"
                  value={projectData.projectName}
                  onChange={handleInputChange("projectName")}
                  className="w-full h-[2.5rem] pl-[1rem] pr-[3rem] text-[#23272A] text-opacity-25 text-[0.875rem] font-medium bg-[#EEF2F6] rounded-[0.5rem] outline-none"
                />
              </div>
              <div>
                <div className="flex flex-col gap-[0.5rem]">
                  <label className="text-[#23272A] font-semibold text-[1rem]">
                    Short Description
                  </label>
                  <textarea
                    placeholder="Jobsky Web App"
                    value={projectData.projectDescription}
                    onChange={handleInputChange("projectDescription")}
                    className="w-full h-[7.5rem] pl-[1rem] pr-[3rem] pt-[0.75rem] text-[#23272A] text-opacity-25 text-[0.875rem] font-medium bg-[#EEF2F6] rounded-[0.5rem] outline-none align-top resize-none"
                  ></textarea>
                </div>
              </div>
            </div>
            <NextToSetBranchesButton />
          </div>
        </div>
      </div>
    </div>
  );
}
