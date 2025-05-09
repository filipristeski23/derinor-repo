import { createBrowserRouter } from "react-router-dom";
import { authRoutes } from "../features/auth/routes/authRoutes.jsx";
import LandingPage from "../features/landingPage/pages/landingPage.jsx";
import { projectSectionRoutes } from "../features/projectsSection/routes/projectSectionRoutes.jsx";
import { reportsPageRoutes } from "../features/reportsPage/routes/reportsPageRoutes.jsx";

export const router = createBrowserRouter([
  { path: "/", element: <LandingPage /> },
  ...authRoutes,
  ...projectSectionRoutes,
  ...reportsPageRoutes,
]);
