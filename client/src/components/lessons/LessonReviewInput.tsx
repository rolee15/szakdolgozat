import { useState, KeyboardEvent, FormEvent } from "react";

interface LessonReviewInputProps {
  onSubmit: (answer: string) => void;
}

const LessonReviewInput: React.FC<LessonReviewInputProps> = ({ onSubmit }) => {
  const [answer, setAnswer] = useState<string>("");


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
    // Always submit, even if empty (counts as wrong)
    onSubmit(answer.trim());
  };

  return (
    <div className="w-full max-w-2xl mx-auto py-6">
      <form onSubmit={handleSubmit} className="flex items-center mb-6">
        <div className="relative flex-grow">
          <input
            type="text"
            value={answer}
            onChange={(e) => setAnswer(e.target.value)}
            onKeyDown={handleKeyDown}
            className="w-full px-4 py-2 border border-gray-300 rounded-l-md text-center"
            placeholder="Type your answer..."
            aria-label="Review answer"
          />
        </div>

        <button
          type="submit"
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 rounded-r-md"
          aria-label="Submit answer"
        >
          &gt;
        </button>
      </form>
    </div>
  );
};

export default LessonReviewInput;
