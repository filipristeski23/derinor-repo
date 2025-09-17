import React from "react";
import { useLocation } from "react-router-dom";
import GitHubLogo from "../../../assets/images/github-mark-white.svg";
import DerinorLogo from "../../../assets/images/DerinorLogo.svg";
import WelcomeBackBackground from "../../../assets/images/background.svg";

const api = `https://localhost:7113/`;

export default function LoginPage() {
  const location = useLocation();

  const handleGitHubLogin = () => {
    window.location.href = `${api}auth/github-signin`;
  };

  const urlParams = new URLSearchParams(location.search);
  const error = urlParams.get("error");

  return (
    <div className="w-full h-screen flex flex-col lg:flex-row">
      <div className="w-full lg:w-[50%] bg-[#F8FAFD] flex flex-col items-center justify-center text-center p-[2rem]">
        <div className="w-full max-w-[38.75rem] flex flex-col items-center">
          <img src={DerinorLogo} alt="Derinor Logo" className="w-[7.875rem]" />
          <h2 className="text-[1.875rem] text-[#23272A] font-bold mt-[0.5rem]">
            Connect & Sign In
          </h2>
          <h4 className="text-[1rem] text-[#23272A] font-medium max-w-[25rem] mt-[0.5rem]">
            By connecting your account you sign in with us and connect your
            account at the same time
          </h4>

          {error && (
            <div className="mt-[2rem] p-[1rem] bg-red-100 border border-red-300 rounded-[0.5rem] text-red-700 w-full">
              {error === "no_token" &&
                "Authentication failed: No token received from server."}
              {error === "auth_failed" &&
                "Authentication failed: Could not process user data."}
            </div>
          )}

          <button
            onClick={handleGitHubLogin}
            className="bg-black text-white flex w-full max-w-[25rem] h-[2.5rem] items-center justify-center gap-[1rem] rounded-[0.5rem] cursor-pointer mt-[4rem] text-[1rem] font-medium hover:bg-gray-800 transition-colors"
          >
            Continue with GitHub
            <img src={GitHubLogo} className="w-[1.5rem]" alt="GitHub Logo" />
          </button>
        </div>
      </div>
      <div
        className="hidden lg:flex w-[50%] h-full bg-[#3D6BC6] bg-cover bg-center bg-no-repeat items-center justify-center"
        style={{ backgroundImage: `url(${WelcomeBackBackground})` }}
      >
        <h3 className="bg-transparent font-bold text-[3rem] xl:text-[5rem] text-[#F8FAFD] text-center">
          WELCOME <br /> HOME
        </h3>
      </div>
    </div>
  );
}
