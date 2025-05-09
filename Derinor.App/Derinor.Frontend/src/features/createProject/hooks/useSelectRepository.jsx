import { useState } from "react";

export const useSelectRepository = () => {
  const [selectedRepository, setSelectedRepository] = useState("");

  const handleRepositorySelection = (repositoryName) => {
    setSelectedRepository(repositoryName);
  };

  return { selectedRepository, handleRepositorySelection };
};
