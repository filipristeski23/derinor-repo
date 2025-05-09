import React from "react";
import { Link } from "react-router-dom";

export default function GetStartedButtonPricing() {
  return (
    <>
      <Link
        to="/register"
        className="flex align-middle justify-center h-[2.5rem]"
      >
        <button className="w-full bg-[#3D6BC6] pl-[2.5rem] pr-[2.5rem] font-[0.875rem] text-[#F8FAFC] font-semibold flex items-center justify-center cursor-pointer rounded-[0.4rem] leading-[1.75rem] ">
          Get Started
        </button>
      </Link>
    </>
  );
}
