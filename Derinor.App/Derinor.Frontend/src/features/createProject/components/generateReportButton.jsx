import React from "react";
import { Link } from "react-router-dom";

export default function GenerateReportButton() {
  return (
    <Link to="/home/side-menu/project-report">
      <button className="bg-[#3D6BC6] h-[2.5rem] w-full sm:w-[11.125rem] text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem] ">
        Generate Report
      </button>
    </Link>
  );
}
