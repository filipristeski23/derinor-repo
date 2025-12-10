import React from "react";

export default function GetStartedButtonPricing({ plan }) {
  const handleSelectPlan = () => {
    window.location.href = `https://localhost:7113/auth/select-plan?plan=${plan}`;
  };

  return (
    <button
      onClick={handleSelectPlan}
      className="w-full bg-[#3D6BC6] pl-[2.5rem] pr-[2.5rem] font-[0.875rem] text-[#F8FAFC] font-semibold flex items-center justify-center cursor-pointer rounded-[0.4rem] leading-[1.75rem] h-[2.5rem]"
    >
      Get Started
    </button>
  );
}
