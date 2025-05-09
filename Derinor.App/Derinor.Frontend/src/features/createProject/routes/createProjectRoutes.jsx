import { Route } from "react-router-dom";
import CreateProjectDetails from "../components/createProjectDetails";
import CreateProjectRepositories from "../components/createProjectRepositores";
import CreateProjectBranches from "../components/createProjectBranches";
import CreateProjectSideMenu from "../pages/createProjectSideMenu";

export const createProjectRoutes = [
  {
    path: "create-project",
    element: <CreateProjectSideMenu />,
    children: [
      { index: true, element: <CreateProjectDetails /> },
      {
        path: "details",
        element: <CreateProjectDetails />,
      },
      {
        path: "repositories",
        element: <CreateProjectRepositories />,
      },
      {
        path: "branches",
        element: <CreateProjectBranches />,
      },
    ],
  },
];
