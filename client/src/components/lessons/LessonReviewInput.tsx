import { useState, KeyboardEvent, FormEvent } from "react";

interface LessonReviewInputProps {
  onSubmit: (answer: string) => void;
}

const LessonReviewInput: React.FC<LessonReviewInputProps> = ({ onSubmit }) => {
  const [answer, setAnswer] = useState<string>("");

  const [warning, setWarning] = useState<string>("");

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
        e.preventDefault();
        checkAnswer(answer);
    }
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    checkAnswer(answer);
  };

    const checkAnswer = (answer: string) => {
        if (answer.trim()) {
            onSubmit(answer.trim());
          } else {
            setWarning("Type your answer first.");
          }
    }

  return (
    <div className="w-full max-w-2xl mx-auto py-6">
        <form onSubmit={handleSubmit} className="flex items-center">
        {warning && (
          <div className="absolute top-0 left-0 w-full bg-red-100 text-orange-600 p-2 rounded-md mb-4">
            {warning}
          </div>
        )}
        <div className="relative flex-grow">
          <input
            type="text"
            value={answer}
            onChange={(e) => setAnswer(e.target.value)}
            onKeyDown={handleKeyDown}
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-center"
            placeholder="Type your answer..."
            aria-label="Review answer"
          />
        </div>

        <button
          type="submit"
          className="ml-2 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
          aria-label="Submit answer"
        >
          &gt;
        </button>
      </form>
    </div>
  );
};

export default LessonReviewInput;
