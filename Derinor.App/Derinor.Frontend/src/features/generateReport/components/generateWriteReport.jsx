import React, { useEffect, useState, useRef } from "react";
import { useParams } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

import PublishReportButton from "./PublishReportButton";
import GenerateReportButton from "./GenerateReportButton";
import { useGenerateReportStore } from "../store/useGenerateReportStore";
import DateRangePickerCustomInput from "./DateRangePickerCustomInput.jsx";

export default function GenerateWriteReport() {
  const { projectID } = useParams();
  const editableRef = useRef(null);
  const [editedText, setEditedText] = useState("");

  const setProjectID = useGenerateReportStore((state) => state.setProjectID);
  const geminiData = useGenerateReportStore((state) => state.geminiData);
  const isLoading = useGenerateReportStore((state) => state.isLoading);
  const isPublishing = useGenerateReportStore((state) => state.isPublishing);
  const fetchGeminiData = useGenerateReportStore(
    (state) => state.fetchGeminiData
  );
  const startDate = useGenerateReportStore((state) => state.startDate);
  const endDate = useGenerateReportStore((state) => state.endDate);
  const setDateRange = useGenerateReportStore((state) => state.setDateRange);
  const publishReport = useGenerateReportStore((state) => state.publishReport);

  useEffect(() => {
    if (projectID) {
      setProjectID(projectID);
    }
  }, [projectID, setProjectID]);

  useEffect(() => {
    let currentText = "";
    if (isLoading) {
      currentText = "Generating report, please wait...";
    } else if (geminiData) {
      currentText = geminiData.geminiMessage;
    } else {
      currentText = "Select a date range and click 'Generate Report' to begin.";
    }
    setEditedText(currentText);
    if (editableRef.current) {
      editableRef.current.innerText = currentText;
    }
  }, [geminiData, isLoading]);

  const handlePublish = () => {
    publishReport(editedText);
  };

  const handleInput = (e) => {
    setEditedText(e.currentTarget.innerText);
  };

  return (
    <div className="flex flex-col gap-[2rem]">
      <div className="flex flex-row justify-between items-center">
        <h2 className="text-[#23272A] font-bold text-[2rem]">New Report</h2>
        <GenerateReportButton
          onClick={fetchGeminiData}
          disabled={isLoading || isPublishing || !startDate || !endDate}
        />
      </div>

      <div className="flex flex-col gap-[0.5rem]">
        <label className="text-[#23272A] font-semibold">
          Select Date Range
        </label>
        <DatePicker
          selected={startDate}
          onChange={setDateRange}
          startDate={startDate}
          endDate={endDate}
          selectsRange
          dateFormat="MMM d, yyyy"
          customInput={<DateRangePickerCustomInput />}
          popperPlacement="bottom-end"
        />
      </div>

      <div>
        <div
          ref={editableRef}
          onInput={handleInput}
          contentEditable={!isLoading}
          suppressContentEditableWarning={true}
          className="w-full bg-[#EEF2F6] text-[#23272A] outline-none p-[1rem] rounded-[0.375rem] min-h-[23rem] resize-none"
        ></div>
      </div>
      <PublishReportButton
        onClick={handlePublish}
        disabled={isPublishing || isLoading || !geminiData}
      />
    </div>
  );
}
