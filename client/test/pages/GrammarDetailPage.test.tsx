import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

vi.mock('@/services/grammarService', () => ({
  default: {
    getGrammarDetail: vi.fn(),
    checkExercise: vi.fn(),
  },
}));

import GrammarDetailPage from '@/pages/GrammarDetailPage';
import grammarService from '@/services/grammarService';

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
      <MemoryRouter initialEntries={[`/grammar/${id}`]}>
        <Routes>
          <Route path="/grammar/:id" element={<GrammarDetailPage />} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const sampleDetail: GrammarPointDetail = {
  id: 1,
  title: 'は (wa) — Topic Marker',
  pattern: 'Noun + は + Predicate',
  explanation: 'は marks the topic of a sentence.',
  jlptLevel: 5,
  correctCount: 0,
  attemptCount: 0,
  isCompleted: false,
  examples: [
    {
      japanese: '私は学生です。',
      reading: 'Watashi wa gakusei desu.',
      english: 'I am a student.',
    },
  ],
  exercises: [
    {
      id: 10,
      sentence: '私 ___ 学生です。',
      options: ['は', 'が', 'を', 'に'],
    },
    {
      id: 11,
      sentence: 'これ ___ 本です。',
      options: ['は', 'が', 'を', 'に'],
    },
  ],
};

describe('GrammarDetailPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching', () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows title, explanation, and examples after loading', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(await screen.findByText('は (wa) — Topic Marker')).toBeInTheDocument();
    expect(screen.getByText('は marks the topic of a sentence.')).toBeInTheDocument();
    expect(screen.getByText('私は学生です。')).toBeInTheDocument();
    expect(screen.getByText('Watashi wa gakusei desu.')).toBeInTheDocument();
    expect(screen.getByText('I am a student.')).toBeInTheDocument();
  });

  it('shows exercise with 4 option buttons', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);

    renderPage();

    await screen.findByText('は (wa) — Topic Marker');

    expect(screen.getByRole('button', { name: 'は' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'が' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'を' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'に' })).toBeInTheDocument();
  });

  it('shows Correct feedback when correct option is clicked', async () => {
    const svc = grammarService as unknown as {
      getGrammarDetail: ReturnType<typeof vi.fn>;
      checkExercise: ReturnType<typeof vi.fn>;
    };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);
    svc.checkExercise.mockResolvedValue({
      isCorrect: true,
      correctAnswer: 'は',
      correctCount: 1,
      attemptCount: 1,
      isCompleted: false,
    } satisfies GrammarExerciseResult);

    renderPage();

    await screen.findByText('は (wa) — Topic Marker');
    fireEvent.click(screen.getByRole('button', { name: 'は' }));

    expect(await screen.findByText(/correct!/i)).toBeInTheDocument();
  });

  it('shows Incorrect feedback with correct answer when wrong option is clicked', async () => {
    const svc = grammarService as unknown as {
      getGrammarDetail: ReturnType<typeof vi.fn>;
      checkExercise: ReturnType<typeof vi.fn>;
    };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);
    svc.checkExercise.mockResolvedValue({
      isCorrect: false,
      correctAnswer: 'は',
      correctCount: 0,
      attemptCount: 1,
      isCompleted: false,
    } satisfies GrammarExerciseResult);

    renderPage();

    await screen.findByText('は (wa) — Topic Marker');
    fireEvent.click(screen.getByRole('button', { name: 'が' }));

    expect(await screen.findByText(/incorrect/i)).toBeInTheDocument();
    expect(await screen.findByText(/correct answer: は/i)).toBeInTheDocument();
  });

  it('shows error state when fetch fails', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load grammar detail/i)).toBeInTheDocument();
  });

  it('shows no exercises message when exercises list is empty', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockResolvedValue({
      ...sampleDetail,
      exercises: [],
    });

    renderPage();

    expect(await screen.findByText(/no exercises available/i)).toBeInTheDocument();
  });

  it('advances to next exercise after clicking Next', async () => {
    const svc = grammarService as unknown as {
      getGrammarDetail: ReturnType<typeof vi.fn>;
      checkExercise: ReturnType<typeof vi.fn>;
    };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);
    svc.checkExercise.mockResolvedValue({
      isCorrect: true,
      correctAnswer: 'は',
      correctCount: 1,
      attemptCount: 1,
      isCompleted: false,
    } satisfies GrammarExerciseResult);

    renderPage();

    await screen.findByText('は (wa) — Topic Marker');
    expect(screen.getByText('Question 1 of 2')).toBeInTheDocument();

    fireEvent.click(screen.getByRole('button', { name: 'は' }));
    await screen.findByText(/correct!/i);

    fireEvent.click(screen.getByRole('button', { name: /next/i }));

    expect(await screen.findByText('Question 2 of 2')).toBeInTheDocument();
  });

  it('shows score summary after completing all exercises', async () => {
    const svc = grammarService as unknown as {
      getGrammarDetail: ReturnType<typeof vi.fn>;
      checkExercise: ReturnType<typeof vi.fn>;
    };
    const oneExerciseDetail = { ...sampleDetail, exercises: [sampleDetail.exercises[0]] };
    svc.getGrammarDetail.mockResolvedValue(oneExerciseDetail);
    svc.checkExercise.mockResolvedValue({
      isCorrect: true,
      correctAnswer: 'は',
      correctCount: 1,
      attemptCount: 1,
      isCompleted: false,
    } satisfies GrammarExerciseResult);

    renderPage();

    await screen.findByText('は (wa) — Topic Marker');
    fireEvent.click(screen.getByRole('button', { name: 'は' }));
    await screen.findByText(/correct!/i);

    fireEvent.click(screen.getByRole('button', { name: /next/i }));

    expect(await screen.findByText(/score: 1\/1/i)).toBeInTheDocument();
  });

  it('shows completed message when isCompleted is true in result', async () => {
    const svc = grammarService as unknown as {
      getGrammarDetail: ReturnType<typeof vi.fn>;
      checkExercise: ReturnType<typeof vi.fn>;
    };
    const oneExerciseDetail = { ...sampleDetail, exercises: [sampleDetail.exercises[0]] };
    svc.getGrammarDetail.mockResolvedValue(oneExerciseDetail);
    svc.checkExercise.mockResolvedValue({
      isCorrect: true,
      correctAnswer: 'は',
      correctCount: 3,
      attemptCount: 3,
      isCompleted: true,
    } satisfies GrammarExerciseResult);

    renderPage();

    await screen.findByText('は (wa) — Topic Marker');
    fireEvent.click(screen.getByRole('button', { name: 'は' }));
    await screen.findByText(/correct!/i);

    fireEvent.click(screen.getByRole('button', { name: /next/i }));

    expect(await screen.findByText(/grammar point completed/i)).toBeInTheDocument();
  });
});
