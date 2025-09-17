function EachReport({ report }) {
  return (
    <div className="w-full max-w-[37.5rem] min-h-[23.063rem] bg-[#EEF2F6] rounded-[0.5rem] text-[#23272A] pt-[1rem] pb-[1rem] pl-[2rem] pr-[2rem]">
      <div className="whitespace-pre-wrap">
        {report.projectReportDescription}
      </div>
    </div>
  );
}

export default EachReport;
