import React from "react";

export default function Footer() {
  return (
    <div
      id="contact"
      className="w-full h-[22.5rem] bg-[#3D6BC6] flex items-center justify-center"
    >
      <div className="w-full max-w-[62.5rem] flex justify-between bg-[#3D6BC6]">
        <div>
          <ul className="flex flex-col gap-[1rem] w-fit bg-[#3D6BC6]">
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                Home
              </a>
            </li>
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                Features
              </a>
            </li>
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                Pricing
              </a>
            </li>
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                Request a feature
              </a>
            </li>
          </ul>
        </div>

        <div className="bg-[#3D6BC6]">
          <ul className="flex flex-col gap-[1rem] w-fit bg-[#3D6BC6]">
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                Terms Of Service
              </a>
            </li>
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                Privacy Policy
              </a>
            </li>
          </ul>
        </div>

        <div className="bg-[#3D6BC6]">
          <ul className="flex flex-col gap-[1rem] w-fit bg-[#3D6BC6]">
            <li>
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                hello@email.com
              </a>
            </li>
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                instagram
              </a>
            </li>
            <li className="bg-[#3D6BC6]">
              <a
                href=""
                className="block w-fit text-[1rem] font-semibold bg-[#3D6BC6] text-[#F8FAFD]"
              >
                facebook
              </a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
}
