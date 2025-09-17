import GenerateReportSideMenu from "../pages/generateReportSideMenu";

export const generateReportsRoutes = [
  {
    path: ":projectID/generate-report",
    element: <GenerateReportSideMenu />,
  },
];
