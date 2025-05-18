import React from "react";
import { Link } from "react-router-dom";

export default function GenerateReportButton({ onClick }) {
  return (
    <>
      <button
        className="bg-[#3D6BC6] w-max h-[2.5rem] pl-[2.5rem] pr-[2.5rem] font-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem] "
        onClick={onClick}
      >
        Generate Report
      </button>
    </>
  );
}
