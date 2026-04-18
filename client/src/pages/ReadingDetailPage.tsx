import { useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { useQuery, useMutation } from '@tanstack/react-query';
import readingService from '@/services/readingService';

const ReadingDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const passageId = Number(id);

  const [answers, setAnswers] = useState<Record<number, string>>({});
  const [result, setResult] = useState<ReadingResult | null>(null);

  const { data: detail, isLoading, isError } = useQuery({
    queryKey: ['reading', passageId],
    queryFn: () => readingService.getPassageDetail(passageId),
  });

  const submitMutation = useMutation({
    mutationFn: () => readingService.submitAnswers(passageId, answers),
    onSuccess: (data) => {
      setResult(data);
    },
  });

  const handleOptionChange = (questionId: number, option: string) => {
    setAnswers((prev) => ({ ...prev, [questionId]: option }));
  };

  const handleSubmit = () => {
    submitMutation.mutate();
  };

  const handleTryAgain = () => {
    setAnswers({});
    setResult(null);
  };

  if (isLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isError || !detail) {
    return <div className="p-4 text-red-500">Failed to load passage detail.</div>;
  }

  return (
    <div className="p-4 max-w-3xl mx-auto text-white">
      <button
        onClick={() => navigate('/reading')}
        className="text-gray-400 hover:text-white mb-6 flex items-center gap-1"
      >
        ← Back
      </button>

      <div className="flex items-center gap-3 mb-2">
        <h1 className="text-3xl font-bold">{detail.title}</h1>
        <span className="text-xs bg-blue-700 text-blue-100 px-2 py-1 rounded-full font-medium">
          N{detail.jlptLevel}
        </span>
      </div>
      {detail.source && (
        <p className="text-gray-400 text-sm mb-4">Source: {detail.source}</p>
      )}

      <div className="bg-gray-800 rounded-lg p-6 mb-8">
        <p className="text-xl leading-relaxed whitespace-pre-wrap">{detail.content}</p>
      </div>

      {result ? (
        <div className="bg-gray-800 rounded-lg p-6">
          <h2 className="text-2xl font-bold mb-2">Results</h2>
          <p className="text-lg mb-1">
            Score:{' '}
            <span className={result.isPassed ? 'text-green-400' : 'text-red-400'}>
              {result.score}%
            </span>
          </p>
          <p className="mb-1">
            {result.correctCount} / {result.totalQuestions} correct
          </p>
          <span
            className={`inline-block px-3 py-1 rounded-full text-sm font-medium mb-6 ${
              result.isPassed
                ? 'bg-green-700 text-green-100'
                : 'bg-red-700 text-red-100'
            }`}
          >
            {result.isPassed ? 'Passed' : 'Failed'}
          </span>

          <ul className="space-y-4 mb-6">
            {result.results.map((qr, idx) => {
              const question = detail.questions.find((q) => q.id === qr.questionId);
              return (
                <li key={qr.questionId} className="bg-gray-700 rounded-lg p-4">
                  <p className="font-medium mb-1">
                    Q{idx + 1}: {question?.questionText}
                  </p>
                  {qr.isCorrect ? (
                    <p className="text-green-400 text-sm">Correct</p>
                  ) : (
                    <p className="text-red-400 text-sm">
                      Incorrect — correct answer: {qr.correctOption}, your answer:{' '}
                      {qr.chosenOption}
                    </p>
                  )}
                </li>
              );
            })}
          </ul>

          <div className="flex gap-4">
            <button
              onClick={handleTryAgain}
              className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2 rounded-lg font-medium"
            >
              Try Again
            </button>
            <Link
              to="/reading"
              className="bg-gray-600 hover:bg-gray-500 text-white px-6 py-2 rounded-lg font-medium"
            >
              Back to Reading
            </Link>
          </div>
        </div>
      ) : (
        <section>
          <h2 className="text-xl font-semibold mb-4">Comprehension Questions</h2>
          {detail.questions.length === 0 ? (
            <p className="text-gray-400">No questions available.</p>
          ) : (
            <div className="space-y-6">
              {detail.questions.map((q, idx) => (
                <div key={q.id} className="bg-gray-800 rounded-lg p-5">
                  <p className="font-medium mb-3">
                    {idx + 1}. <span>{q.questionText}</span>
                  </p>
                  <div className="space-y-2">
                    {Object.entries(q.options).map(([key, value]) => (
                      <label
                        key={key}
                        className="flex items-center gap-3 cursor-pointer hover:text-gray-200"
                      >
                        <input
                          type="radio"
                          name={`question-${q.id}`}
                          value={key}
                          checked={answers[q.id] === key}
                          onChange={() => handleOptionChange(q.id, key)}
                          className="accent-blue-500"
                        />
                        <span>
                          {key}: {value}
                        </span>
                      </label>
                    ))}
                  </div>
                </div>
              ))}

              {submitMutation.isError && (
                <p className="text-red-400">Failed to submit answers. Please try again.</p>
              )}

              <button
                onClick={handleSubmit}
                disabled={submitMutation.isPending}
                className="bg-blue-600 hover:bg-blue-500 disabled:opacity-50 text-white px-6 py-2 rounded-lg font-medium"
              >
                {submitMutation.isPending ? 'Submitting...' : 'Submit'}
              </button>
            </div>
          )}
        </section>
      )}
    </div>
  );
};

export default ReadingDetailPage;
