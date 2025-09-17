import LoginPage from "../pages/LoginPage";
import AuthCallback from "../pages/authCallback";

export const authRoutes = [
  { path: "/login", element: <LoginPage /> },
  { path: "/auth/callback", element: <AuthCallback /> },
];
