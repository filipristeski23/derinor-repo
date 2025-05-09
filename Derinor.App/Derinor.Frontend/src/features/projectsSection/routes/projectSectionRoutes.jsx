import { Route } from "react-router-dom";
import ProjectSectionPage from "../../projectsSection/pages/projectSectionPage";
import CreateProjectSideMenu from "../../createProject/pages/createProjectSideMenu";
import { createProjectRoutes } from "../../createProject/routes/createProjectRoutes";

export const projectSectionRoutes = [
  {
    path: "/projects",
    element: <ProjectSectionPage />,
    children: [createProjectRoutes],
  },
];
