function EachReport({ report }) {
  return (
    <div className="w-full max-w-[37.5rem] h-auto bg-[#EEF2F6] rounded-[0.5rem] text-[#23272A] py-[1.5rem] px-[1rem] md:px-[2rem]">
      <div className="whitespace-pre-wrap">
        {report.projectReportDescription}
      </div>
    </div>
  );
}

export default EachReport;
