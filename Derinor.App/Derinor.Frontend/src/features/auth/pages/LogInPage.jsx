import React from "react";
import { Link } from "react-router-dom";
import GitHubLogo from "../../../assets/images/github-mark-white.svg";
import DerinorLogo from "../../../assets/images/DerinorLogo.svg";
import WelcomeBackBackground from "../../../assets/images/background.svg";

export default function LoginPage() {
  return (
    <div className="w-full h-screen flex">
      <div className=" w-[50%] bg-[#F8FAFD] flex flex-col items-center justify-center text-center">
        <div className="w-[38.75rem] flex flex-col align-middle justify-center items-center">
          <img src={DerinorLogo} alt="Derinor Logo" className="w-[7.875rem] " />
          <h2 className="text-[1.875rem] text-[#23272A] font-bold mt-[0.5rem]">
            Connect & Sign In
          </h2>
          <h4 className="text-[1rem] text-[#23272A] font-medium">
            By connecting your account you sign in with us and connect your
            account at the same time
          </h4>
          <Link to="/projects">
            <button className="bg-black text-white flex w-[25rem] h-[2.5rem] items-center justify-center gap-[1rem] rounded-[0.5rem] cursor-pointer mt-[4rem] text-[1rem] font-medium">
              Continue with GitHub{" "}
              <img src={GitHubLogo} className="w-[1.5rem] bg-black" />
            </button>
          </Link>
        </div>
      </div>
      <div
        className="w-[50%] h-full bg-[#3D6BC6] bg-contain bg-center bg-no-repeat flex items-center justify-center"
        style={{ backgroundImage: `url(${WelcomeBackBackground})` }}
      >
        <h3 className="bg-transparent font-bold text-[5rem] text-[#F8FAFD] text-center">
          WELCOME <br /> HOME
        </h3>
      </div>
    </div>
  );
}
