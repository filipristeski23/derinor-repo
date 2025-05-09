import { useNavigate } from "react-router-dom";

export const useBackToRepositories = () => {
  const navigate = useNavigate();

  const backToRepositories = () => {
    navigate("/projects/create-project/repositories");
  };

  return backToRepositories;
};
