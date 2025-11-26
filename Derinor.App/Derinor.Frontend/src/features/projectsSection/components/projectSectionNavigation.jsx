import React, { useState } from "react";
import Logo from "../../../assets/images/DerinorLogo.svg";
import LogoutButton from "../../../features/projectsSection/components/LogOutButton";

const HamburgerIcon = (props) => (
  <svg
    {...props}
    stroke="currentColor"
    fill="none"
    strokeWidth="2"
    viewBox="0 0 24 24"
    strokeLinecap="round"
    strokeLinejoin="round"
    height="1.5em"
    width="1.5em"
    xmlns="http://www.w3.org/2000/svg"
  >
    <line x1="3" y1="12" x2="21" y2="12"></line>
    <line x1="3" y1="6" x2="21" y2="6"></line>
    <line x1="3" y1="18" x2="21" y2="18"></line>
  </svg>
);

const CloseIcon = (props) => (
  <svg
    {...props}
    stroke="currentColor"
    fill="none"
    strokeWidth="2"
    viewBox="0 0 24 24"
    strokeLinecap="round"
    strokeLinejoin="round"
    height="1.5em"
    width="1.5em"
    xmlns="http://www.w3.org/2000/svg"
  >
    <line x1="18" y1="6" x2="6" y2="18"></line>
    <line x1="6" y1="6" x2="18" y2="18"></line>
  </svg>
);

function ProjectSectionNavigation() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <div className="max-w-full border-b border-b-[#D8DFEC] border-b-[0.015625rem] px-[1rem] sm:px-[2rem] relative">
      <div className="max-w-[82.5rem] h-[5.5rem] mx-auto my-0 flex justify-between items-center">
        <div className="flex items-center gap-[2rem] lg:gap-[6rem]">
          <a
            href="/projects"
            className="flex justify-center items-center h-[2.5rem] flex-shrink-0"
          >
            <img src={Logo} alt="logo" className="w-[8.875rem] h-[2.5rem]" />
          </a>
          <nav className="hidden lg:flex">
            <ul className="flex justify-around gap-[1.5rem]">
              <li>
                <a href="/projects" className="font-semibold whitespace-nowrap">
                  My Projects
                </a>
              </li>
              <li>
                <a
                  href="mailto:risteski.filip@uklo.edu.mk"
                  className="font-semibold whitespace-nowrap"
                >
                  Request Feature
                </a>
              </li>
            </ul>
          </nav>
        </div>

        <div className="hidden lg:flex items-center gap-[1rem]">
          <LogoutButton />
        </div>

        <div className="lg:hidden">
          <button onClick={() => setIsMenuOpen(!isMenuOpen)}>
            {isMenuOpen ? <CloseIcon /> : <HamburgerIcon />}
          </button>
        </div>
      </div>
      {isMenuOpen && (
        <div className="lg:hidden absolute top-[5.5rem] left-0 w-full bg-[#F8FAFD] shadow-md z-20">
          <nav className="flex flex-col items-center gap-[1.5rem] py-[2rem]">
            <a href="/projects" className="font-semibold text-[1.25rem]">
              My Projects
            </a>
            <a
              href="mailto:risteski.filip@uklo.edu.mk"
              className="font-semibold text-[1.25rem]"
            >
              Request Feature
            </a>
            <div className="mt-[1rem]">
              <LogoutButton />
            </div>
          </nav>
        </div>
      )}
    </div>
  );
}

export default ProjectSectionNavigation;
