import React from "react";
import NavigationBar from "../components/navigationBar";
import MainHeroSection from "../components/mainHeroSection";
import FeaturesSection from "../components/featuresSection";
import PricingSection from "../components/pricingSection";
import Footer from "../components/footer";

export default function LandingPage() {
  return (
    <div className="relative min-h-screen">
      <div className="absolute inset-0 bg-[#f8fafd]; ">
        <div
          className="absolute inset-0 opacity-75"
          style={{
            backgroundImage:
              "radial-gradient(circle at center, #94a3b8 1px, transparent 1px)",
            backgroundSize: "30px 30px",
          }}
        />
      </div>

      <div className="relative z-10">
        <NavigationBar />
        <MainHeroSection />
        <FeaturesSection />
        <PricingSection />
        <Footer />
      </div>
    </div>
  );
}
