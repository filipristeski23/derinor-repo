import React, { forwardRef } from "react";

const CalendarIcon = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 20 20"
    fill="currentColor"
    className="w-[1.25rem] h-[1.25rem] text-[#6B7280]"
  >
    <path
      fillRule="evenodd"
      d="M5.75 2a.75.75 0 01.75.75V4h7V2.75a.75.75 0 011.5 0V4h.25A2.75 2.75 0 0118 6.75v8.5A2.75 2.75 0 0115.25 18H4.75A2.75 2.75 0 012 15.25v-8.5A2.75 2.75 0 014.75 4H5V2.75A.75.75 0 015.75 2zM4.5 6.75A1.25 1.25 0 015.75 5.5h8.5A1.25 1.25 0 0115.5 6.75v1.405h-11V6.75zM3.5 15.25a1.25 1.25 0 011.25-1.25h10.5a1.25 1.25 0 011.25 1.25v-5.095h-13v5.095z"
      clipRule="evenodd"
    />
  </svg>
);

const DateRangePickerCustomInput = forwardRef(({ value, onClick }, ref) => (
  <button
    onClick={onClick}
    ref={ref}
    className="w-full h-[3rem] bg-[#EEF2F6] rounded-[0.5rem] flex items-center justify-between px-[1rem] text-[#23272A] font-medium"
  >
    <span>{value || "Select a date range"}</span>
    <CalendarIcon />
  </button>
));

export default DateRangePickerCustomInput;
