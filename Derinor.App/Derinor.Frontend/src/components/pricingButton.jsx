import React from "react";
import { Link } from "react-router-dom";

export default function PricingButton() {
  return (
    <>
      <Link
        to="/register"
        className="flex align-middle justify-center h-[2.5rem] w-[10rem]"
      >
        <button className="bg-[#F8FAFD] pl-[2.5rem] pr-[2.5rem] font-[0.875rem] text-[#23272A] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem] border-[0.063rem] border-[#3D6BC6]">
          Pricing
        </button>
      </Link>
    </>
  );
}
