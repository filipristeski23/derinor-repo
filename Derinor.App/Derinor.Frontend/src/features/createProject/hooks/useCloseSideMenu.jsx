import { useNavigate } from "react-router-dom";

export default function useCloseSideMenu() {
  const navigate = useNavigate();

  const closeMenu = () => {
    navigate("/projects");
  };

  return {
    closeMenu,
  };
}
