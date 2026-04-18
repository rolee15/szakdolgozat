import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

vi.mock('@/services/readingService', () => ({
  default: {
    getPassageDetail: vi.fn(),
    submitAnswers: vi.fn(),
  },
}));

import ReadingDetailPage from '@/pages/ReadingDetailPage';
import readingService from '@/services/readingService';

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

function renderPage(id = '1') {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter initialEntries={[`/reading/${id}`]}>
        <Routes>
          <Route path="/reading/:id" element={<ReadingDetailPage />} />
          <Route path="/reading" element={<div>Reading List</div>} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const sampleDetail: ReadingPassageDetail = {
  id: 1,
  title: 'Spring Day',
  jlptLevel: 5,
  isPassed: false,
  score: 0,
  attemptCount: 0,
  content: '春の日はとても暖かいです。',
  source: 'NHK',
  questions: [
    {
      id: 10,
      questionText: 'What season is described?',
      options: { A: 'Spring', B: 'Summer', C: 'Autumn', D: 'Winter' },
    },
    {
      id: 11,
      questionText: 'How is the day described?',
      options: { A: 'Cold', B: 'Warm', C: 'Rainy', D: 'Windy' },
    },
  ],
};

const sampleResult: ReadingResult = {
  score: 100,
  isPassed: true,
  correctCount: 2,
  totalQuestions: 2,
  results: [
    { questionId: 10, isCorrect: true, correctOption: 'A', chosenOption: 'A' },
    { questionId: 11, isCorrect: false, correctOption: 'B', chosenOption: 'C' },
  ],
};

describe('ReadingDetailPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching', () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows error state when fetch fails', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load passage detail/i)).toBeInTheDocument();
  });

  it('renders passage title and content after loading', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(await screen.findByText('Spring Day')).toBeInTheDocument();
    expect(screen.getByText('春の日はとても暖かいです。')).toBeInTheDocument();
  });

  it('renders source when present', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(await screen.findByText(/source: NHK/i)).toBeInTheDocument();
  });

  it('does not render source line when source is empty', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockResolvedValue({ ...sampleDetail, source: '' });

    renderPage();

    await screen.findByText('Spring Day');
    expect(screen.queryByText(/source:/i)).not.toBeInTheDocument();
  });

  it('renders comprehension questions with radio options', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(await screen.findByText('What season is described?')).toBeInTheDocument();
    expect(screen.getByText(/A: Spring/)).toBeInTheDocument();
    expect(screen.getByText(/B: Summer/)).toBeInTheDocument();
  });

  it('shows no questions message when questions list is empty', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockResolvedValue({ ...sampleDetail, questions: [] });

    renderPage();

    expect(await screen.findByText(/no questions available/i)).toBeInTheDocument();
  });

  it('submits answers and shows results', async () => {
    const svc = readingService as unknown as {
      getPassageDetail: ReturnType<typeof vi.fn>;
      submitAnswers: ReturnType<typeof vi.fn>;
    };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);
    svc.submitAnswers.mockResolvedValue(sampleResult);

    renderPage();

    await screen.findByText('Spring Day');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText('Results')).toBeInTheDocument();
    expect(screen.getByText(/100%/)).toBeInTheDocument();
    expect(screen.getByText(/Passed/)).toBeInTheDocument();
    expect(screen.getByText(/2 \/ 2 correct/)).toBeInTheDocument();
  });

  it('shows failed badge when not passed', async () => {
    const svc = readingService as unknown as {
      getPassageDetail: ReturnType<typeof vi.fn>;
      submitAnswers: ReturnType<typeof vi.fn>;
    };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);
    svc.submitAnswers.mockResolvedValue({
      ...sampleResult,
      score: 50,
      isPassed: false,
    });

    renderPage();

    await screen.findByText('Spring Day');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText('Failed')).toBeInTheDocument();
  });

  it('shows per-question correct result in results view', async () => {
    const svc = readingService as unknown as {
      getPassageDetail: ReturnType<typeof vi.fn>;
      submitAnswers: ReturnType<typeof vi.fn>;
    };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);
    svc.submitAnswers.mockResolvedValue(sampleResult);

    renderPage();

    await screen.findByText('Spring Day');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText('Correct')).toBeInTheDocument();
  });

  it('shows per-question incorrect result in results view', async () => {
    const svc = readingService as unknown as {
      getPassageDetail: ReturnType<typeof vi.fn>;
      submitAnswers: ReturnType<typeof vi.fn>;
    };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);
    svc.submitAnswers.mockResolvedValue(sampleResult);

    renderPage();

    await screen.findByText('Spring Day');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(
      await screen.findByText(/incorrect — correct answer: B, your answer: C/i),
    ).toBeInTheDocument();
  });

  it('shows submit error when mutation fails', async () => {
    const svc = readingService as unknown as {
      getPassageDetail: ReturnType<typeof vi.fn>;
      submitAnswers: ReturnType<typeof vi.fn>;
    };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);
    svc.submitAnswers.mockRejectedValue(new Error('Network error'));

    renderPage();

    await screen.findByText('Spring Day');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText(/failed to submit answers/i)).toBeInTheDocument();
  });

  it('resets to question form after clicking Try Again', async () => {
    const svc = readingService as unknown as {
      getPassageDetail: ReturnType<typeof vi.fn>;
      submitAnswers: ReturnType<typeof vi.fn>;
    };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);
    svc.submitAnswers.mockResolvedValue(sampleResult);

    renderPage();

    await screen.findByText('Spring Day');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));
    await screen.findByText('Results');

    fireEvent.click(screen.getByRole('button', { name: /try again/i }));

    expect(screen.getByText('Comprehension Questions')).toBeInTheDocument();
  });
});
