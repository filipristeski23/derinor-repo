import React, { useRef } from "react";

const LeftArrowIcon = (props) => (
  <svg
    {...props}
    xmlns="http://www.w3.org/2000/svg"
    width="1.5rem"
    height="1.5rem"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
  >
    <polyline points="15 18 9 12 15 6"></polyline>
  </svg>
);

const RightArrowIcon = (props) => (
  <svg
    {...props}
    xmlns="http://www.w3.org/2000/svg"
    width="1.5rem"
    height="1.5rem"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
  >
    <polyline points="9 18 15 12 9 6"></polyline>
  </svg>
);

const ProjectCard = ({ project }) => (
  <div className="flex flex-col h-[16rem] rounded-[0.5rem] border border-[#D8DFEC] bg-white p-[1.5rem] shadow-sm">
    <h3 className="font-bold text-[1.25rem] mb-[0.5rem] text-gray-800">
      {project.name}
    </h3>
    <p className="text-gray-600 flex-grow text-[0.9rem]">
      {project.description}
    </p>
    <a href="/projects" className="font-semibold text-[#3B82F6] mt-[1rem]">
      View Project
    </a>
  </div>
);

function ProjectsCarousel() {
  const scrollContainerRef = useRef(null);
  const projects = Array.from({ length: 12 }, (_, i) => ({
    id: i + 1,
    name: `Project Alpha ${i + 1}`,
    description: `This is a sample description for the project. Details about the project goals and outcomes are listed here.`,
  }));

  const handleScroll = (scrollOffset) => {
    scrollContainerRef.current.scrollBy({
      left: scrollOffset,
      behavior: "smooth",
    });
  };

  const scrollLeft = () => {
    handleScroll(-scrollContainerRef.current.clientWidth);
  };

  const scrollRight = () => {
    handleScroll(scrollContainerRef.current.clientWidth);
  };

  return (
    <div className="relative max-w-[82.5rem] mx-auto my-[2rem]">
      <button
        onClick={scrollLeft}
        className="hidden md:flex items-center justify-center absolute top-1/2 left-[-1.5rem] transform -translate-y-1/2 bg-white rounded-full shadow-lg w-[3rem] h-[3rem] z-10 hover:bg-gray-100 transition-colors"
      >
        <LeftArrowIcon />
      </button>

      <div
        ref={scrollContainerRef}
        className="flex overflow-x-auto snap-x snap-mandatory [&::-webkit-scrollbar]:hidden [-ms-overflow-style:none] [scrollbar-width:none]"
      >
        {projects.map((project) => (
          <div
            key={project.id}
            className="flex-shrink-0 w-full md:w-1/2 lg:w-1/4 snap-start p-[0.5rem]"
          >
            <ProjectCard project={project} />
          </div>
        ))}
      </div>

      <button
        onClick={scrollRight}
        className="hidden md:flex items-center justify-center absolute top-1/2 right-[-1.5rem] transform -translate-y-1/2 bg-white rounded-full shadow-lg w-[3rem] h-[3rem] z-10 hover:bg-gray-100 transition-colors"
      >
        <RightArrowIcon />
      </button>
    </div>
  );
}

export default ProjectsCarousel;
