import React from "react";
import NextToSetBranchesButton from "./nextToSetBranchesButton";
import { useCreateProjectStore } from "../store/createProjectStore";

export default function CreateProjectDetails() {
  const updateProjectDetails = useCreateProjectStore(
    (state) => state.updateProjectDetails
  );
  const projectOwner = useCreateProjectStore(
    (state) => state.projectData.projectOwner
  );
  const projectName = useCreateProjectStore(
    (state) => state.projectData.projectName
  );
  const projectDescription = useCreateProjectStore(
    (state) => state.projectData.projectDescription
  );

  const handleChange = (e) => {
    const { name, value } = e.target;
    updateProjectDetails({ [name]: value });
  };

  return (
    <div className="flex flex-col flex-grow min-h-0">
      <h2 className="text-[#23272A] font-bold text-[1.75rem] sm:text-[2rem] flex-shrink-0 mb-[2rem]">
        Create your project
      </h2>
      <div className="flex flex-col gap-[1rem] flex-grow min-h-0">
        <input
          type="text"
          name="projectOwner"
          value={projectOwner}
          onChange={handleChange}
          placeholder="Project Owner (e.g., your team or company name)"
          required
          className="w-full h-[2.5rem] px-[1rem] bg-[#EEF2F6] rounded-[0.5rem] outline-none flex-shrink-0"
        />
        <input
          type="text"
          name="projectName"
          value={projectName}
          onChange={handleChange}
          placeholder="Project Name"
          required
          className="w-full h-[2.5rem] px-[1rem] bg-[#EEF2F6] rounded-[0.5rem] outline-none flex-shrink-0"
        />
        <textarea
          name="projectDescription"
          value={projectDescription}
          onChange={handleChange}
          placeholder="Project Description"
          required
          className="w-full p-[1rem] bg-[#EEF2F6] rounded-[0.5rem] outline-none resize-none flex-grow"
        />
      </div>
      <div className="flex-shrink-0 mt-[2rem]">
        <NextToSetBranchesButton />
      </div>
    </div>
  );
}
