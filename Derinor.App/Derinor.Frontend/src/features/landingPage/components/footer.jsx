import React from "react";

export default function Footer() {
  return (
    <div
      id="contact"
      className="w-full h-auto md:h-[22.5rem] bg-[#3D6BC6] flex items-center justify-center py-[3rem] md:py-0"
    >
      <div className="w-full max-w-[62.5rem] flex flex-col md:flex-row justify-center md:justify-between items-center text-center md:text-left gap-[3rem] md:gap-0 px-[1rem]">
        <div>
          <ul className="flex flex-col gap-[1rem] w-fit">
            <li>
              <a
                href="#home"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Home
              </a>
            </li>
            <li>
              <a
                href="#features"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Features
              </a>
            </li>
            <li>
              <a
                href="#pricing"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Pricing
              </a>
            </li>
            <li>
              <a
                href="#request-a-feature"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Request a feature
              </a>
            </li>
          </ul>
        </div>

        <div>
          <ul className="flex flex-col gap-[1rem] w-fit">
            <li>
              <a
                href="#"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Terms Of Service
              </a>
            </li>
            <li>
              <a
                href="#"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Privacy Policy
              </a>
            </li>
          </ul>
        </div>

        <div>
          <ul className="flex flex-col gap-[1rem] w-fit">
            <li>
              <a
                href="mailto:hello@email.com"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                hello@email.com
              </a>
            </li>
            <li>
              <a
                href="#"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Instagram
              </a>
            </li>
            <li>
              <a
                href="#"
                className="block w-fit text-[1rem] font-semibold text-[#F8FAFD]"
              >
                Facebook
              </a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
}
