import React from "react";
import { useRef } from "react";
import PublishReportButton from "./PublishReportButton";
import GenerateReportButton from "./GenerateReportButton";
import { useGenerateReportStore } from "../store/useGenerateReportStore";
import { useEffect } from "react";

export default function GenerateWriteReport() {
  const editableRef = useRef(null);
  const geminiData = useGenerateReportStore((state) => state.geminiData);
  const fetchGeminiData = useGenerateReportStore((s) => s.fetchGeminiData);

  useEffect(() => {
    if (editableRef.current && geminiData) {
      editableRef.current.innerText = geminiData.geminiMessage;
    }
  }, [geminiData]);

  return (
    <div className="flex flex-col gap-[2rem]">
      <div className="flex flex-row justify-between">
        <h2 className="text-[#23272A] font-bold text-[2rem]">New Report</h2>
        <GenerateReportButton onClick={fetchGeminiData} />
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
