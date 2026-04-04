import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useQuery, useMutation } from '@tanstack/react-query';
import grammarService from '@/services/grammarService';

const GrammarDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const grammarId = Number(id);

  const [exerciseIndex, setExerciseIndex] = useState(0);
  const [feedback, setFeedback] = useState<GrammarExerciseResult | null>(null);
  const [score, setScore] = useState(0);
  const [done, setDone] = useState(false);

  const { data: detail, isLoading, isError } = useQuery({
    queryKey: ['grammar', grammarId],
    queryFn: () => grammarService.getGrammarDetail(grammarId),
  });

  const checkMutation = useMutation({
    mutationFn: ({ exerciseId, answer }: { exerciseId: number; answer: string }) =>
      grammarService.checkExercise(grammarId, exerciseId, answer),
    onSuccess: (result) => {
      setFeedback(result);
      if (result.isCorrect) setScore((s) => s + 1);
    },
  });

  const handleOptionClick = (option: string) => {
    if (feedback !== null) return;
    const exercise = detail!.exercises[exerciseIndex];
    checkMutation.mutate({ exerciseId: exercise.id, answer: option });
  };

  const handleNext = () => {
    const exercises = detail!.exercises;
    if (exerciseIndex + 1 >= exercises.length) {
      setDone(true);
    } else {
      setExerciseIndex((i) => i + 1);
      setFeedback(null);
    }
  };

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError || !detail) {
    return <div className="p-4 text-red-500">Failed to load grammar detail.</div>;
  }

  const exercises = detail.exercises;
  const currentExercise = exercises[exerciseIndex];

  return (
    <div className="p-4 max-w-3xl mx-auto text-white">
      <button
        onClick={() => navigate('/grammar')}
        className="text-gray-400 hover:text-white mb-6 flex items-center gap-1"
      >
        ← Back
      </button>

      {/* Section 1 — Explanation */}
      <section className="mb-8">
        <h1 className="text-3xl font-bold mb-2">{detail.title}</h1>
        <p className="font-mono bg-gray-800 rounded px-3 py-2 text-yellow-300 mb-4 inline-block">
          {detail.pattern}
        </p>
        <p className="text-gray-300 mb-6">{detail.explanation}</p>

        {detail.examples.length > 0 && (
          <div>
            <h2 className="text-xl font-semibold mb-3">Examples</h2>
            <ul className="space-y-4">
              {detail.examples.map((ex, i) => (
                <li key={i} className="bg-gray-800 rounded-lg p-4">
                  <p className="text-2xl mb-1">{ex.japanese}</p>
                  <p className="text-sm italic text-gray-400 mb-1">{ex.reading}</p>
                  <p className="text-gray-300">{ex.english}</p>
                </li>
              ))}
            </ul>
          </div>
        )}
      </section>

      {/* Section 2 — Exercises */}
      <section>
        <h2 className="text-xl font-semibold mb-4">Exercises</h2>

        {exercises.length === 0 ? (
          <p className="text-gray-400">No exercises available</p>
        ) : done ? (
          <div className="bg-gray-800 rounded-lg p-6 text-center">
            <p className="text-2xl font-bold mb-2">
              Score: {score}/{exercises.length}
            </p>
            {detail.isCompleted || feedback?.isCompleted ? (
              <p className="text-green-400 font-medium">Grammar point completed!</p>
            ) : (
              <p className="text-yellow-400 font-medium">Keep practicing to complete this grammar point.</p>
            )}
          </div>
        ) : (
          <div className="bg-gray-800 rounded-lg p-6">
            <p className="text-sm text-gray-400 mb-3">
              Question {exerciseIndex + 1} of {exercises.length}
            </p>
            <p className="text-lg mb-5">
              {currentExercise.sentence}
            </p>
            <div className="grid grid-cols-2 gap-3 mb-4">
              {currentExercise.options.map((option) => {
                let buttonClass =
                  'rounded-lg px-4 py-3 text-left font-medium transition-colors ';
                if (feedback !== null) {
                  if (option === feedback.correctAnswer) {
                    buttonClass += 'bg-green-700 text-white';
                  } else {
                    buttonClass += 'bg-gray-700 text-gray-400';
                  }
                } else {
                  buttonClass += 'bg-gray-700 hover:bg-gray-600 text-white';
                }
                return (
                  <button
                    key={option}
                    onClick={() => handleOptionClick(option)}
                    className={buttonClass}
                    disabled={feedback !== null}
                  >
                    {option}
                  </button>
                );
              })}
            </div>
            {feedback !== null && (
              <div>
                {feedback.isCorrect ? (
                  <div className="bg-green-800 text-green-200 rounded-lg px-4 py-3 mb-4">
                    Correct!
                  </div>
                ) : (
                  <div className="bg-red-800 text-red-200 rounded-lg px-4 py-3 mb-4">
                    Incorrect. Correct answer: {feedback.correctAnswer}
                  </div>
                )}
                <button
                  onClick={handleNext}
                  className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2 rounded-lg font-medium"
                >
                  Next
                </button>
              </div>
            )}
          </div>
        )}
      </section>
    </div>
  );
};

export default GrammarDetailPage;
