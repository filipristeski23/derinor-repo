import React, { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import useAuthStore from "../../../app/store/useAuthStore";

export default function AuthCallback() {
  const navigate = useNavigate();
  const location = useLocation();
  const setAuth = useAuthStore((state) => state.setAuth);

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const token = params.get("token");
    const userParam = params.get("user");

    if (token && userParam) {
      try {
        const decodedUser = atob(userParam);
        const userData = JSON.parse(decodedUser);

        setAuth(token, userData);

        navigate("/projects", { replace: true });
      } catch (error) {
        console.error("Failed to process auth data:", error);
        navigate("/login?error=auth_failed");
      }
    } else {
      navigate("/login?error=no_token");
    }
  }, []);

  return (
    <div className="w-full h-screen flex items-center justify-center bg-[#F8FAFD]">
      <p className="text-[1.5rem] text-[#23272A] font-bold">
        Authenticating, please wait...
      </p>
    </div>
  );
}
