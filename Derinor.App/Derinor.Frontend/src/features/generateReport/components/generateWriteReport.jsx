import React from "react";
import { useRef } from "react";
import PublishReportButton from "./PublishReportButton";
import GenerateReportButton from "./GenerateReportButton";

export default function GenerateWriteReport() {
  const editableRef = useRef(null);
  return (
    <div className="flex flex-col gap-[2rem]">
      <div className="flex flex-row justify-between">
        <h2 className="text-[#23272A] font-bold text-[2rem]">New Report</h2>
        <GenerateReportButton />
      </div>
      <div>
        <div
          ref={editableRef}
          contentEditable
          suppressContentEditableWarning
          className="flex flex-col gap-[1rem] bg-[#EEF2F6] text-[#23272A] outline-none p-[1rem] rounded-[0.375rem] min-h-[23rem]"
        >
          Report goes here
        </div>
      </div>
      <PublishReportButton />
    </div>
  );
}
