function ReportsPage() {
  return (
    <>
      <div className="w-full flex flex-col gap-[0.5rem] pt-[2rem] pb-[2rem]">
        <h2 className="text-[#23272] text-[2rem] font-bold flex justify-center">
          Derinor Web App
        </h2>
        <h4 className="flex justify-center text-[1rem] font-medium">
          latest news and reports can be found here
        </h4>
      </div>

      <section className="flex flex-col items-center justify-center gap-[1.5rem]">
        <div className="w-full max-w-[37.5rem] h-[23.063rem] bg-[#EEF2F6] rounded-[0.5rem] text-[#23272] pt-[1rem] pb-[1rem] pl-[2rem] pr-[2rem]">
          The report text goes here
        </div>
        <div className="w-full max-w-[37.5rem] h-[23.063rem] bg-[#EEF2F6] rounded-[0.5rem] text-[#23272] pt-[1rem] pb-[1rem] pl-[2rem] pr-[2rem]">
          The report text goes here
        </div>
      </section>
    </>
  );
}

export default ReportsPage;
