import React from "react";
import Logo from "../../../assets/images/DerinorLogo.svg";
import SettingsButton from "../../../components/SettingsButton";
import LogoutButton from "./LogOutButton";

function ProjectSectionNavigation() {
  return (
    <div className="max-w-full border-b border-b-[#D8DFEC] border-b-[0.015625rem]">
      <div className="max-w-[82.5rem] h-[5.5rem] mx-auto my-0 flex justify-between items-center">
        <div className="flex justify-between items-center gap-[6rem]">
          <a
            href="#home"
            className=" flex justify-middle items-center h-[2.5rem]"
          >
            <img src={Logo} alt="logo" className="w-[8.875rem] h-[2.5rem]" />
          </a>
          <div className="flex align-middle justify-center">
            <ul className="flex justify-around gap-[1.5rem]">
              <li className="flex items-center justify-center">
                <a href="#home" className="font-semibold">
                  My Projects
                </a>
              </li>
              <li className="flex items-center justify-center">
                <a href="#features" className=" font-semibold">
                  Request Feature
                </a>
              </li>
            </ul>
          </div>
        </div>
        <div className="flex items-center gap-[1rem]">
          <SettingsButton />
          <LogoutButton />
        </div>
      </div>
    </div>
  );
}

export default ProjectSectionNavigation;
