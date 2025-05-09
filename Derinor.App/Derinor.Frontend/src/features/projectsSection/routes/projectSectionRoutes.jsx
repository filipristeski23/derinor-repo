import { Route } from "react-router-dom";
import ProjectSectionPage from "../../projectsSection/pages/projectSectionPage";
import CreateProjectSideMenu from "../../createProject/pages/createProjectSideMenu";
import { createProjectRoutes } from "../../createProject/routes/createProjectRoutes";
import { generateReportsRoutes } from "../../generateReport/routes/generateReportsRoutes";

export const projectSectionRoutes = [
  {
    path: "/projects",
    element: <ProjectSectionPage />,
    children: [...createProjectRoutes, ...generateReportsRoutes],
  },
];
