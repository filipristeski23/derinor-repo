import React, { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import api from "../../../app/axiosInstance";
import { useProjectRefreshStore } from "../../createProject/store/projectRefreshStore";
import SearchIcon from "../../../assets/icons/SearchIcon.svg";
import PageArrowButton from "../../../assets/icons/PageArrowButton.svg";

function ProjectSectionMainSection() {
  const [searchProjectData, setSearchProjectData] = useState("");
  const [projectData, setProjectData] = useState([]);
  const scrollContainerRef = useRef(null);
  const refreshTrigger = useProjectRefreshStore(
    (state) => state.refreshTrigger
  );

  useEffect(() => {
    let isMounted = true;
    const fetchProjects = async () => {
      try {
        const params = {
          _t: new Date().getTime(),
        };
        if (searchProjectData) {
          params.search = searchProjectData;
        }

        const response = await api.get("projects/all-projects", { params });
        if (isMounted) {
          setProjectData(response.data);
        }
      } catch (error) {
        if (isMounted) {
          setProjectData([]);
        }
      }
    };

    fetchProjects();
    return () => {
      isMounted = false;
    };
  }, [searchProjectData, refreshTrigger]);

  useEffect(() => {
    if (scrollContainerRef.current) {
      scrollContainerRef.current.scrollLeft = 0;
    }
  }, [projectData]);

  const handleScroll = (direction) => {
    const container = scrollContainerRef.current;
    const scrollAmount = container.clientWidth * direction;
    container.scrollBy({ left: scrollAmount, behavior: "smooth" });
  };

  return (
    <div className="w-full pt-[2rem] pb-[2rem] bg-[#F8FAFD] px-[1rem] sm:px-[2rem]">
      <div className="max-w-[78.5rem] mx-auto my-0 flex flex-col gap-[2rem]">
        <div className="w-full flex flex-col md:flex-row gap-[1rem] md:justify-between">
          <div className="relative w-full md:max-w-[37.5rem]">
            <input
              type="text"
              placeholder="Search for your project.."
              className="w-full h-[2.5rem] pl-[1rem] pr-[3rem] text-[#23272A] text-opacity-25 text-[0.875rem] font-medium bg-[#EEF2F6] rounded-[0.5rem] outline-none"
              value={searchProjectData}
              onChange={(e) => setSearchProjectData(e.target.value)}
            />
            <img
              src={SearchIcon}
              alt="Search"
              className="absolute right-3 top-1/2 transform -translate-y-1/2 w-[1.5rem]"
            />
          </div>
          <Link
            to="create-project"
            className="bg-[#3D6BC6] h-[2.5rem] px-[2rem] flex justify-center items-center text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] flex-shrink-0"
          >
            <button className="cursor-pointer">New Project</button>
          </Link>
        </div>

        <div
          ref={scrollContainerRef}
          className="flex overflow-x-auto snap-x snap-mandatory scroll-smooth -mx-[0.75rem] [&::-webkit-scrollbar]:hidden [-ms-overflow-style:none] [scrollbar-width:none]"
        >
          {projectData && projectData.length > 0 ? (
            projectData.map((project) => (
              <div
                key={project.projectID}
                className="flex-shrink-0 snap-start w-full md:w-1/2 lg:w-1/4 p-[0.75rem]"
              >
                <div className="flex flex-col w-full h-[24.063rem] rounded-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)] bg-white">
                  <div className="bg-[#3D6BC6] w-full h-[9.5rem] rounded-t-[1rem] pt-[1rem] px-[1rem] pb-[1rem] flex flex-col gap-[0.5rem]">
                    <div className="bg-[#D570CC] w-fit inline-block pt-[0.125rem] pb-[0.125rem] px-[0.75rem] rounded-[50rem] text-[#F8FAFD] font-semibold text-[0.875rem]">
                      {project.projectOwner}
                    </div>
                    <h2 className="text-[#F8FAFD] text-[1.75rem] font-bold h-[5rem] overflow-hidden leading-[2.5rem]">
                      {project.projectName}
                    </h2>
                  </div>
                  <div className="flex flex-col justify-between h-full pb-[1rem]">
                    <div className="flex flex-col gap-[0.5rem] px-[1rem] pt-[1rem]">
                      <p className="text-[#23272A] font-medium text-[0.875rem] w-full h-[7rem] leading-[1.75rem]">
                        {project.projectDescription}
                      </p>
                      <h4 className="font-bold text-[23272A] text-[0.875rem]">
                        {project.reports ? project.reports : 0} Reports
                      </h4>
                    </div>
                    <div className="flex flex-col sm:flex-row gap-[1rem] px-[1rem] mt-auto">
                      <Link
                        to={`/projects/${project.projectID}/generate-report`}
                        className="flex items-center justify-center h-[2.5rem] w-full"
                      >
                        <button className="bg-[#3D6BC6] w-full h-full text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem]">
                          New Report
                        </button>
                      </Link>
                      <Link
                        to={`/reports/${project.projectID}`}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="flex items-center justify-center h-[2.5rem] w-full"
                      >
                        <button className="bg-[#3D6BC6] w-full h-full text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem]">
                          Open
                        </button>
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            ))
          ) : (
            <p className="w-full text-center py-[2rem]">No projects found</p>
          )}
        </div>

        <div className="hidden md:flex gap-[1.5rem]">
          <button
            className="h-[2.5rem] bg-[#3D6BC6] px-[1.125rem] rounded-[0.5rem] cursor-pointer"
            onClick={() => handleScroll(-1)}
          >
            <img src={PageArrowButton} alt="Previous Page" />
          </button>
          <button
            className="h-[2.5rem] bg-[#3D6BC6] px-[1.125rem] rounded-[0.5rem] cursor-pointer"
            onClick={() => handleScroll(1)}
          >
            <img src={PageArrowButton} alt="Next Page" className="rotate-180" />
          </button>
        </div>
      </div>
    </div>
  );
}

export default ProjectSectionMainSection;
