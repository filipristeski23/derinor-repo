import { RouterProvider } from "react-router-dom";
import { router } from "../app/router";
import useAuthStore from "../app/store/useAuthStore";
import { useEffect } from "react";

function App() {
  const initialize = useAuthStore((state) => state.initialize);
  const isLoading = useAuthStore((state) => state.isLoading);

  useEffect(() => {
    initialize();
  }, [initialize]);

  if (isLoading) {
    return (
      <div className="w-full h-screen flex items-center justify-center bg-[#F8FAFD]">
        <p className="text-[1.5rem] text-[#23272A] font-bold">
          Loading Application...
        </p>
      </div>
    );
  }

  return <RouterProvider router={router} />;
}

export default App;
