import React from "react";
import { useBackToRepositories } from "../hooks/useBackToRepositories";

export default function BackToRepositories() {
  const backToRepositories = useBackToRepositories();

  return (
    <button
      className="bg-[#3D6BC6] h-[2.5rem] w-[11.125rem] text-[0.875rem] text-[#F8FAFC] font-semibold cursor-pointer rounded-[0.4rem] leading-[1.75rem] "
      onClick={backToRepositories}
    >
      Back
    </button>
  );
}
