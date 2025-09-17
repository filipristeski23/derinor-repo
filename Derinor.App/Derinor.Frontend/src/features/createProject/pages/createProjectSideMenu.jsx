import React, { useEffect } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import CloseMenuButton from "../../../assets/icons/CloseMenuButton.svg";
import { useCreateProjectStore } from "../store/createProjectStore";

export default function CreateProjectSideMenu() {
  const navigate = useNavigate();
  const reset = useCreateProjectStore((state) => state.reset);

  useEffect(() => {
    return () => {
      reset();
    };
  }, [reset]);

  const closeMenu = () => {
    navigate("/projects");
  };

  return (
    <div className="fixed top-0 right-0 h-screen w-full max-w-[39.75rem] bg-[#F8FAFD] shadow-[0_4px_8px_rgba(0,0,0,0.1)] overflow-y-hidden px-[1.5rem] py-[2rem] md:px-[4rem]">
      <div className="flex flex-col gap-[3.25rem] h-full">
        <button
          className="bg-[#3D6BC6] h-[2.5rem] w-[3.75rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center justify-center cursor-pointer flex-shrink-0"
          onClick={closeMenu}
        >
          <img
            src={CloseMenuButton}
            alt="close menu button"
            className="w-[1.5rem]"
          />
        </button>
        <Outlet />
      </div>
    </div>
  );
}
