import ProjectSectionNavigation from "../components/projectSectionNavigation.jsx";
import ProjectSectionMainSection from "../components/projectSectionMainSection.jsx";
import { Outlet } from "react-router-dom";
import { UseSendGeminiData } from "../../generateReport/hooks/useSendGeminiData.jsx";

function ProjectSectionPage() {
  return (
    <>
      <ProjectSectionNavigation />
      <ProjectSectionMainSection />
      <Outlet />
    </>
  );
}

export default ProjectSectionPage;
