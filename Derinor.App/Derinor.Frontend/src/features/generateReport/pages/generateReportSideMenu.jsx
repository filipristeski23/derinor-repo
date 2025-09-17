import { Outlet } from "react-router-dom";
import CloseMenuButton from "../../../assets/icons/CloseMenuButton.svg";
import { useNavigate } from "react-router-dom";
import GenerateWriteReport from "../components/generateWriteReport";

export default function GenerateReportSideMenu() {
  const navigate = useNavigate();

  const closeMenu = () => {
    navigate("/projects");
  };

  return (
    <div className="fixed top-0 right-0 h-screen w-full max-w-[39.75rem] pl-[4rem] pr-[4rem] pt-[2rem] bg-[#F8FAFD] shadow-[0_4px_8px_rgba(0,0,0,0.1)] overflow-y-auto">
      <div className="flex flex-col gap-[3.25rem]">
        <button
          className="bg-[#3D6BC6] h-[2.5rem] w-[3.75rem] pl-[1.125rem] pr-[1.125rem] rounded-[0.5rem] flex items-center justify-center cursor-pointer"
          onClick={closeMenu}
        >
          <img
            src={CloseMenuButton}
            alt="close menu button"
            className="w-[1.5rem]"
          />
        </button>
        <GenerateWriteReport />
      </div>
    </div>
  );
}
