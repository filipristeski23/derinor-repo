import { useEffect, useState } from "react";
import { geminiDataService } from "../services/geminiDataService";

export const UseSendGeminiData = () => {
  const [projectID, setProjectID] = useState("");
  const [geminiData, setGeminiData] = useState("");

  useEffect(() => {
    const fetchGeminiData = async () => {
      const geminiData = await geminiDataService.fetchGemini(projectID);
      setGeminiData(geminiData);
    };

    fetchGeminiData();
  }, [projectID]);

  return { setProjectID, geminiData };
};
