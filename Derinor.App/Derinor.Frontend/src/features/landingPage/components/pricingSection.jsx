import React from "react";
import CheckMark from "../../../assets/icons/checkMark.svg";
import GetStartedButtonPricing from "../../../components/getStartedButtonPricing.jsx";

export default function PricingSection() {
  return (
    <div id="pricing" className="w-full mt-[10rem] mb-[15rem]">
      <div className="max-w-[62.5rem] mx-auto my-0 flex flex-col items-center">
        <div className="flex flex-col gap-[1rem]">
          <h2 className="text-[2.5rem] font-semibold w-full max-w-[40.625rem] text-center">
            Pricing
          </h2>
          <h3 className="text-[1.25rem] font-medium text-center w-full max-w-[40.625rem]">
            Currently we are under beta and everyone can use the app for free,
            when beta expired pricing down applies
          </h3>
        </div>
        <div className=" w-full flex gap-[2.5rem] justify-center mt-[3.5rem]">
          <div className="w-full max-w-[20.625rem]">
            <div className="max-w-[20.625rem] rounded-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)]">
              <div className="bg-[#3D6BC6] flex-col gap-[0.5rem] rounded-tl-[1rem] rounded-tr-[1rem]  pt-[2rem] pb-[2rem] pl-[1.5rem]">
                <h5 className="text-[1.25rem] font-medium bg-[#3D6BC6] text-[#F8FAFD]">
                  Starter Plan
                </h5>
                <h4 className="text-[2.5rem] font-bold bg-[#3D6BC6] text-[#F8FAFD]">
                  10$
                </h4>
              </div>
              <div className="flex flex-col gap-[1.5rem] pt-[2rem] pb-[2rem] pl-[1.5rem] pr-[1.5rem] rounded-bl-[1rem] rounded-br-[1rem] bg-[#f8fafd]">
                <div className="flex gap-[0.5rem]">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium">5 Projects</h5>
                </div>
                <div className="flex gap-[0.5rem]">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium ">
                    25 Reports Per Day Total
                  </h5>
                </div>
                <div className="flex gap-[0.5rem] align-middle">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium">
                    Publish Page Per Project
                  </h5>
                </div>
                <GetStartedButtonPricing />
              </div>
            </div>
          </div>
          <div className="w-full max-w-[20.625rem]">
            <div className="max-w-[20.625rem] rounded-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)]">
              <div className="bg-[#3D6BC6] flex-col gap-[0.5rem] rounded-tl-[1rem] rounded-tr-[1rem]  pt-[2rem] pb-[2rem] pl-[1.5rem]">
                <h5 className="text-[1.25rem] font-medium bg-[#3D6BC6] text-[#F8FAFD]">
                  Starter Plan
                </h5>
                <h4 className="text-[2.5rem] font-bold bg-[#3D6BC6] text-[#F8FAFD]">
                  10$
                </h4>
              </div>
              <div className="flex flex-col gap-[1.5rem] pt-[2rem] pb-[2rem]  pl-[1.5rem] pr-[1.5rem] rounded-bl-[1rem] rounded-br-[1rem] bg-[#f8fafd]">
                <div className="flex gap-[0.5rem]">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium">5 Projects</h5>
                </div>
                <div className="flex gap-[0.5rem]">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium ">
                    25 Reports Per Day Total
                  </h5>
                </div>
                <div className="flex gap-[0.5rem] align-middle">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium">
                    Publish Page Per Project
                  </h5>
                </div>
                <GetStartedButtonPricing />
              </div>
            </div>
          </div>
          <div className="w-full max-w-[20.625rem]">
            <div className="max-w-[20.625rem] rounded-[1rem] shadow-[0_4px_8px_rgba(0,0,0,0.1)]">
              <div className="bg-[#3D6BC6] flex-col gap-[0.5rem] rounded-tl-[1rem] rounded-tr-[1rem]  pt-[2rem] pb-[2rem] pl-[1.5rem]">
                <h5 className="text-[1.25rem] font-medium bg-[#3D6BC6] text-[#F8FAFD]">
                  Starter Plan
                </h5>
                <h4 className="text-[2.5rem] font-bold bg-[#3D6BC6] text-[#F8FAFD]">
                  10$
                </h4>
              </div>
              <div className="flex flex-col gap-[1.5rem] pt-[2rem] pb-[2rem]  pl-[1.5rem] pr-[1.5rem] rounded-bl-[1rem] rounded-br-[1rem] bg-[#f8fafd]">
                <div className="flex gap-[0.5rem]">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium">5 Projects</h5>
                </div>
                <div className="flex gap-[0.5rem]">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium ">
                    25 Reports Per Day Total
                  </h5>
                </div>
                <div className="flex gap-[0.5rem] align-middle">
                  <img src={CheckMark} alt="check mark" />
                  <h5 className="text-[0.875rem] font-medium">
                    Publish Page Per Project
                  </h5>
                </div>
                <GetStartedButtonPricing />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
