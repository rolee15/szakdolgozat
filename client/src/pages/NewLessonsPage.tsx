import api from "@/services/lessonService";
import { useEffect, useRef, useState } from "react";

const NewLessonsPage = () => {
    const [lessons, setLessons] = useState<Lesson[]>([]);
    const [error, setError] = useState<string | null>(null);

    const [currentLesson, setCurrentLesson] = useState<Lesson | null>(null);
    const [currentLessonIndex, setCurrentLessonIndex] = useState<number>(0);

    const answerInputField = useRef<HTMLInputElement>(null);

    useEffect(() => {
        const fetchNewLessons = async () => {
            try {
                const newLessons = await api.getNewLessons(0, 10);
                setLessons(newLessons);
            } catch (err) {
                setError(err instanceof Error ? err.message : "Failed to load new lessons");
            }
        };

        fetchNewLessons();
    }, []);

    useEffect(() => {
        if (lessons.length > 0) {
            setCurrentLesson(lessons[currentLessonIndex]);
        }
    }, [lessons, currentLessonIndex]);

    if (error) {
        return <div className="p-4 text-red-500">Error: {error}</div>;
    }

    const handleSubmit = () => {
        if (currentLesson) {
            const answer = answerInputField.current?.value;
            api.postLessonReviewAnswer(currentLesson.symbol, answer || "")
            .then((result) => {
                console.log("Answer result: ", result);
                if (result) {
                    setCurrentLessonIndex(currentLessonIndex + 1);
                }
            });
        }
    };

  return (
    <div className="max-w-4xl mx-auto p-4">
      <p className="text-3xl">{currentLesson?.symbol}</p>
      <input type="text" ref={answerInputField}></input>
      <button onClick={handleSubmit}>Submit answer</button>
    </div>
  );
};
export default NewLessonsPage;
