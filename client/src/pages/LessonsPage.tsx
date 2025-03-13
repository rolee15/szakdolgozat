import api from "@/services/lessonService";
import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";

const LessonsPage = () => {
  const [lessonsCount, setLessonsCount] = useState<number>(0);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLessonCount = async () => {
      try {
        const count = await api.getTodayLessonCount();
        console.log(count);
        setLessonsCount(count.count);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load lesson count");
      }
    };

    fetchLessonCount();
  }, []);

  if (error) {
    return <div className="p-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div>
      <h1>Lessons</h1>
      <p>Today's lessons {lessonsCount ? lessonsCount : "Done"}
        {/* print the number if it is not zero, otherwise a label 'Done' */}

      </p>
      <NavLink to={"/lessons/new"}>Start lessons</NavLink>
    </div>
  );
};
export default LessonsPage;
