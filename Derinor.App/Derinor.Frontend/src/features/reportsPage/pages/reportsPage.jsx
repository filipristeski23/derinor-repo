import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../../../app/axiosInstance";
import EachReport from "../components/EachReport";

function ReportsPage() {
  const { projectID } = useParams();
  const [reports, setReports] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (!projectID) return;
    const fetchReports = async () => {
      setIsLoading(true);
      try {
        const response = await api.get(`/projects/all-by-project/${projectID}`);
        setReports(response.data);
      } catch (error) {
        console.error("Failed to fetch reports:", error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchReports();
  }, [projectID]);

  return (
    <div className="px-[1rem] md:px-[2rem]">
      <div className="w-full flex flex-col gap-[0.5rem] pt-[2rem] pb-[2rem] text-center">
        <h2 className="text-[#23272A] text-[1.75rem] md:text-[2rem] font-bold">
          Project Reports
        </h2>
        <h4 className="text-[1rem] font-medium">
          latest news and reports can be found here
        </h4>
      </div>

      <section className="flex flex-col items-center justify-center gap-[1.5rem] pb-[2rem]">
        {isLoading ? (
          <p>Loading reports...</p>
        ) : reports.length > 0 ? (
          reports.map((report) => (
            <EachReport key={report.projectReportID} report={report} />
          ))
        ) : (
          <p>No reports found for this project.</p>
        )}
      </section>
    </div>
  );
}

export default ReportsPage;
