import React from "react";
import DesktopReports from "../../../assets/images/DesktopReports.svg";
import SetBranches from "../../../assets/images/SetBranches.svg";
import ReportsPage from "../../../assets/images/ReportsPage.svg";
import NewReport from "../../../assets/images/NewReport.svg";

export default function FeaturesSection() {
  return (
    <div id="features" className="w-full mt-[6rem] md:mt-[10rem] px-[1rem]">
      <div className="max-w-[82.5rem] mx-auto my-0 flex-col items-center ">
        <div className="max-w-[62.5rem] mx-auto w-full shadow-[0_4px_8px_rgba(0,0,0,0.1)] rounded-b-[1rem]">
          <div className="w-full h-[18rem] md:h-[28.75rem] bg-[#3D6BC6] rounded-tl-[1rem] rounded-tr-[1rem] rounded-bl-0 rounded-br-0 relative flex justify-end ">
            <img
              src={DesktopReports}
              alt="Projects Feature"
              className="w-[85%] h-auto absolute bottom-0 right-0 rounded-tl-[0.5rem] border-b-[1px] border-[#3D6BC6]"
            />
          </div>
          <div className="w-full p-[1.5rem] md:pt-[3rem] md:pl-[3rem] md:pb-[3rem] bg-[#F8FAFD] rounded-tl-0 rounded-tr-0 rounded-bl-[1rem] rounded-br-[1rem]">
            <div className="flex flex-col gap-[1.5rem]">
              <h2 className="text-[1.75rem] md:text-[2rem] text-[#23272A] font-semibold leading-[2.5rem]">
                As many projects as you need
              </h2>
              <h3 className="text-[1rem] md:text-[1.25rem] text-[#23272A] font-medium leadin-[1.75rem]">
                create as many different projects to easy manage your clients or
                your work.
              </h3>
            </div>
          </div>
        </div>
        <div className="max-w-[62.5rem] mx-auto w-full flex flex-col lg:flex-row gap-[1.5rem] mt-[3rem]">
          <div className="w-full">
            <div className="w-full h-[18rem] md:h-[28.75rem] bg-[#3D6BC6] rounded-tl-[1rem] rounded-tr-[1rem] rounded-bl-0 rounded-br-0 relative flex justify-start overflow-hidden">
              <img
                src={SetBranches}
                alt="Projects Feature"
                className="absolute bottom-0 w-[90%] h-auto object-contain rounded-tr-[0.5rem] border-b-[1px] border-[#3D6BC6]"
              />
            </div>
            <div className="w-full p-[1.5rem] md:pt-[2rem] md:pl-[1.5rem] md:pb-[2rem] bg-[#F8FAFD] rounded-tl-0 rounded-tr-0 rounded-bl-[1rem] rounded-br-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)]">
              <div className="flex flex-col gap-[1.5rem]">
                <h2 className="text-[1.75rem] md:text-[2rem] text-[#23272A] font-semibold leading-[2.5rem]">
                  Set Branches
                </h2>
                <h3 className="text-[1rem] text-[#23272A] font-medium leadin-[1.5rem]">
                  you can choose multiple different repositories, and set
                  different branches you want to create reports from.
                </h3>
              </div>
            </div>
          </div>
          <div className="w-full">
            <div className="w-full h-[18rem] md:h-[28.75rem] bg-[#3D6BC6] rounded-tl-[1rem] rounded-tr-[1rem] rounded-bl-0 rounded-br-0 relative flex justify-start overflow-hidden">
              <img
                src={NewReport}
                alt="Projects Feature"
                className="absolute bottom-0 w-[90%] h-auto object-contain rounded-tr-[0.5rem] border-b-[1px] border-[#3D6BC6]"
              />
            </div>
            <div className="w-full p-[1.5rem] md:pt-[2rem] md:pl-[1.5rem] md:pb-[2rem] bg-[#F8FAFD] rounded-tl-0 rounded-tr-0 rounded-bl-[1rem] rounded-br-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)]">
              <div className="flex flex-col gap-[1.5rem]">
                <h2 className="text-[1.75rem] md:text-[2rem] text-[#23272A] font-semibold leading-[2.5rem]">
                  Generate Reports
                </h2>
                <h3 className="text-[1rem] text-[#23272A] font-medium leadin-[1.5rem]">
                  From the chosen branches, generate. edit and publish immaculate reports
                  that everyone can understand without a problem.
                </h3>
              </div>
            </div>
          </div>
        </div>
        <div className="max-w-[62.5rem] mx-auto w-full shadow-[0_4px_8px_rgba(0,0,0,0.1)] rounded-b-[1rem] mt-[3rem]">
          <div className="w-full h-[18rem] md:h-[28.75rem] bg-[#3D6BC6] rounded-tl-[1rem] rounded-tr-[1rem] rounded-bl-0 rounded-br-0 relative flex justify-end ">
            <img
              src={ReportsPage}
              alt="Projects Feature"
              className="w-[85%] h-auto absolute bottom-0 right-0 rounded-tl-[0.5rem] border-b-[1px] border-[#3D6BC6]"
            />
          </div>
          <div className="w-full p-[1.5rem] md:pt-[3rem] md:pl-[3rem] md:pb-[3rem] bg-[#F8FAFD] rounded-tl-0 rounded-tr-0 rounded-bl-[1rem] rounded-br-[1rem]">
            <div className="flex flex-col gap-[1.5rem]">
              <h2 className="text-[1.75rem] md:text-[2rem] text-[#23272A] font-semibold leading-[2.5rem]">
                Dedicated reports page for each project
              </h2>
              <h3 className="text-[1rem] md:text-[1.25rem] text-[#23272A] font-medium leadin-[1.75rem]">
                Automatically publish your reports to dedicated page where your
                clients can check latest reports on how the project grows.
              </h3>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
