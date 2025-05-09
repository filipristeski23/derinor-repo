import SearchIcon from "../../../assets/icons/SearchIcon.svg";
import PageArrowButton from "../../../assets/icons/PageArrowButton.svg";
import { Link } from "react-router-dom";

function ProjectSectionMainSection() {
  return (
    <div className="max-w-full pt-[2rem] pb-[2rem] bg-[#F8FAFD]">
      <div className="max-w-[78.5rem] h-[20rem] max-h-screen mx-auto my-0 flex flex-col gap-[2rem]">
        <div className="w-full flex justify-between">
          <div className="relative w-full max-w-[37.5rem]">
            <input
              type="text"
              placeholder="Search for your project.."
              className="w-full h-[2.5rem] pl-[1rem] pr-[3rem] text-[#23272A] text-opacity-25 text-[0.875rem] font-medium bg-[#EEF2F6] rounded-[0.5rem] outline-none"
            />
            <img
              src={SearchIcon}
              alt="Search"
              className="absolute right-3 top-1/2 transform -translate-y-1/2 w-[1.5rem]"
            />
          </div>
          <Link
            to="create-project"
            className="bg-[#3D6BC6] pl-[2rem] pr-[2rem] flex justify-center text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem]"
          >
            <button className="cursor-pointer">New Project</button>
          </Link>
        </div>

        <div className=" w-full flex justify-between gap-[1rem]">
          <div className="flex flex-col w-full max-w-[18.875rem] h-[24.063rem] rounded-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)]">
            <div className="bg-[#3D6BC6] w-full h-[9.5rem] rounded-tr-[1rem] rounded-tl-[1rem] rounded-bl-[0rem] rounded-br-[0rem] pt-[1rem] pl-[1rem] pr-[1rem] pb-[1rem] flex flex-col gap-[0.5rem] ">
              <div className="bg-[#D570CC] w-fit   inline-block pt-[0.125rem] pb-[0.125rem] pl-[0.75rem] pr-[0.75rem] rounded-[50rem] text-[#F8FAFD] font-semibold ">
                Mine
              </div>
              <h2 className="text-[#F8FAFD] text-[2rem] font-bold h-[5rem] overflow-hidden">
                App Name
              </h2>
            </div>
            <div className="flex flex-col justify-between h-full pb-[1rem]">
              <div className="flex flex-col gap-[0.5rem] pl-[1rem] pt-[1rem] pr-[1rem]">
                <text className="text-[#23272A] font-medium text-[0.875rem] w-full max-w-[16.875rem] leading-[1.75rem]">
                  A sleek and intuitive task management web app that helps
                  individuals and teams organize.
                </text>
                <h4 className="font-bold text-[#23272A] text-[0.875rem]">
                  10 Reports
                </h4>
              </div>
              <div className="flex gap-[1rem] pl-[1rem] pr-[1rem] mt-auto">
                <Link
                  to="/register"
                  className="flex align-middle justify-center h-[2.5rem]"
                >
                  <button className="bg-[#3D6BC6] pl-[2rem] pr-[2rem] text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem] ">
                    New Report
                  </button>
                </Link>
                <Link
                  to="/register"
                  className="flex align-middle justify-center h-[2.5rem]"
                >
                  <button className="bg-[#3D6BC6] pl-[2rem] pr-[2rem] text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem]">
                    Open
                  </button>
                </Link>
              </div>
            </div>
          </div>
        </div>

        <div className="flex gap-[1.5rem]">
          <button className="h-[2.5rem] bg-[#3D6BC6] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] cursor-pointer">
            <img src={PageArrowButton} alt="Arrow" />
          </button>
          <button className="h-[2.5rem] bg-[#3D6BC6] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] cursor-pointer">
            <img src={PageArrowButton} alt="Arrow" className="rotate-180" />
          </button>
        </div>
      </div>
    </div>
  );
}

export default ProjectSectionMainSection;
