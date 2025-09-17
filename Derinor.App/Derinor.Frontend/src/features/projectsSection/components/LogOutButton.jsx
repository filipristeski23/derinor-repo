import React from "react";
import { useNavigate } from "react-router-dom";
import useAuthStore from "../../../app/store/useAuthStore";

function LogoutButton() {
  const navigate = useNavigate();
  const logout = useAuthStore((state) => state.logout);

  const handleLogout = () => {
    logout();
    navigate("/login", { replace: true });
  };

  return (
    <button
      onClick={handleLogout}
      className="h-[2.5rem] pl-[1.5rem] pr-[1.5rem] text-[0.875rem] text-[#3D6BC6] font-semibold cursor-pointer rounded-[0.4rem] hover:bg-gray-100"
    >
      Log Out
    </button>
  );
}

export default LogoutButton;
