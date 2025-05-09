import React from "react";
import Logo from "../../../assets/images/DerinorLogo.svg";
import LogInButton from "../../../components/logInButton";

export default function NavigationBar() {
  return (
    <div id="home" className="max-w-full">
      <div className="max-w-[82.5rem] h-[5.5rem] mx-auto my-0 flex justify-between items-center">
        <a
          href="#home"
          className=" flex justify-middle items-center h-[2.5rem]"
        >
          <img src={Logo} alt="logo" className="w-[8.875rem] h-[2.5rem]" />
        </a>
        <div className="flex align-middle justify-center">
          <ul className="flex justify-around gap-[1.5rem]">
            <li className="flex items-center justify-center">
              <a href="#home" className="font-semibold">
                Home
              </a>
            </li>
            <li className="flex items-center justify-center">
              <a href="#features" className=" font-semibold">
                Features
              </a>
            </li>
            <li className="flex items-center justify-center">
              <a href="#pricing" className=" font-semibold">
                Pricing
              </a>
            </li>
            <li className="flex items-center justify-center">
              <a href="#request-a-feature" className="font-semibold">
                Request a feature
              </a>
            </li>
            <li className="flex items-center justify-center">
              <a href="#contact" className="font-semibold">
                Contact
              </a>
            </li>
          </ul>
        </div>
        <LogInButton />
      </div>
    </div>
  );
}
