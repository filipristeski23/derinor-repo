import { createBrowserRouter } from "react-router-dom";
import { authRoutes } from "../features/auth/routes/authRoutes.jsx";
import LandingPage from "../features/landingPage/pages/landingPage.jsx";
import { projectSectionRoutes } from "../features/projectsSection/routes/projectSectionRoutes.jsx";
import { reportsPageRoutes } from "../features/reportsPage/routes/reportsPageRoutes.jsx";
import ProtectedRoute from "../components/ProtectedRoute.jsx";

const protectedProjectSectionRoutes = projectSectionRoutes.map((route) => ({
  ...route,
  element: <ProtectedRoute>{route.element}</ProtectedRoute>,
}));

export const router = createBrowserRouter([
  { path: "/", element: <LandingPage /> },
  ...authRoutes,
  ...protectedProjectSectionRoutes,
  ...reportsPageRoutes,
]);
