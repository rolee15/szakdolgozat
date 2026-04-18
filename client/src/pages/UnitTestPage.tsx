import { useState } from 'react';
import { useParams, useNavigate, Link, Navigate } from 'react-router-dom';
import { useQuery, useMutation } from '@tanstack/react-query';
import pathService from '@/services/pathService';

const UnitTestPage = () => {
  const { unitId } = useParams<{ unitId: string }>();
  const navigate = useNavigate();
  const id = Number(unitId);

  const [answers, setAnswers] = useState<Record<number, string>>({});
  const [result, setResult] = useState<UnitTestResult | null>(null);

  const {
    data: unitDetail,
    isLoading: isUnitLoading,
    isError: isUnitError,
  } = useQuery({
    queryKey: ['path', id],
    queryFn: () => pathService.getUnitDetail(id),
  });

  const {
    data: testData,
    isLoading: isTestLoading,
    isError: isTestError,
  } = useQuery({
    queryKey: ['path', id, 'test'],
    queryFn: () => pathService.getUnitTest(id),
    enabled: unitDetail !== undefined,
  });

  const submitMutation = useMutation({
    mutationFn: () => pathService.submitTest(id, answers),
    onSuccess: (data) => {
      setResult(data);
    },
  });

  const handleOptionChange = (questionId: number, option: string) => {
    setAnswers((prev) => ({ ...prev, [questionId]: option }));
  };

  const handleRetake = () => {
    setAnswers({});
    setResult(null);
  };

  if (isUnitLoading || isTestLoading) {
    return <div className="p-4 text-white">Loading...</div>;
  }

  if (isUnitError) {
    return <div className="p-4 text-red-500">Failed to load unit.</div>;
  }

  if (unitDetail && !unitDetail.isUnlocked) {
    return <Navigate to="/path" replace />;
  }

  if (isTestError || !testData) {
    return <div className="p-4 text-red-500">Failed to load test.</div>;
  }

  if (result) {
    const passed = result.isPassed;
    return (
      <div className="p-4 max-w-2xl mx-auto text-white">
        <h1 className="text-2xl font-bold mb-6">Test Results</h1>
        <div className="bg-gray-800 rounded-lg p-6 text-center mb-6">
          <p className="text-4xl font-bold mb-2">{result.score}%</p>
          <p className="text-gray-400 mb-3">
            {result.correctCount} / {result.totalQuestions} correct
          </p>
          <span
            className={`inline-block px-4 py-2 rounded-full font-medium text-lg ${
              passed ? 'bg-green-700 text-green-100' : 'bg-red-700 text-red-100'
            }`}
          >
            {passed ? 'Passed' : 'Failed'}
          </span>
          {passed && (
            <p className="text-green-400 mt-3 text-sm">
              Congratulations! You passed with {result.score}%.
            </p>
          )}
          {!passed && (
            <p className="text-red-400 mt-3 text-sm">
              You need 70% or more to pass. Keep studying!
            </p>
          )}
        </div>
        <div className="flex gap-4">
          <button
            onClick={handleRetake}
            className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2 rounded-lg font-medium"
          >
            Retake Test
          </button>
          <Link
            to={`/path/${id}`}
            className="bg-gray-600 hover:bg-gray-500 text-white px-6 py-2 rounded-lg font-medium"
          >
            Back to Unit
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="p-4 max-w-2xl mx-auto text-white">
      <button
        onClick={() => navigate(`/path/${id}`)}
        className="text-gray-400 hover:text-white mb-6 flex items-center gap-1"
      >
        ← Back to Unit
      </button>

      <h1 className="text-2xl font-bold mb-6">Unit Test</h1>

      {testData.questions.length === 0 ? (
        <p className="text-gray-400">No questions available.</p>
      ) : (
        <div className="space-y-6">
          {testData.questions.map((q, idx) => (
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
            <p className="text-red-400">Failed to submit test. Please try again.</p>
          )}

          <button
            onClick={() => submitMutation.mutate()}
            disabled={submitMutation.isPending}
            className="bg-blue-600 hover:bg-blue-500 disabled:opacity-50 text-white px-6 py-2 rounded-lg font-medium"
          >
            {submitMutation.isPending ? 'Submitting...' : 'Submit'}
          </button>
        </div>
      )}
    </div>
  );
};

export default UnitTestPage;
