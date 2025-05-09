import React from "react";
import GetStartedButton from "../../../components/getStartedButton";
import PricingButton from "../../../components/getStartedButton";

export default function MainHeroSection() {
  return (
    <div className="w-full mt-[5rem]  ">
      <div className="max-w-[82.5rem] mx-auto my-0 ">
        <div className="max-w-[62.5rem] mx-auto flex flex-col gap-[1.5rem] items-center">
          <h3 className="text-center bg-[#D570CC] text-[#F8FAFD] w-fit rounded-[1rem] pl-[2rem] pr-[2rem] pt-[0.25rem] pb-[0.25rem]">
            currently in beta and free for everyone
          </h3>
          <div className="flex flex-col gap-[3rem]">
            <div className="flex flex-col gap-[1rem]">
              <h1 className="text-[4.375rem] text-[#23272A] leading-[4.875rem] font-semibold text-center">
                From Coding Activity to Meaningfull Reports
              </h1>
              <h2 className="text-center text-[1rem] text-[#23272A] font-medium">
                directly turn your latest code updates from your repository into
                clear reports, so non-technical people can easily understand
                what you’ve been working on.
              </h2>
            </div>

            <div className="flex items-center justify-center gap-[1rem]">
              <GetStartedButton />
              <PricingButton />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
