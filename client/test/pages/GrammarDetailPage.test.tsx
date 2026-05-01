import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

vi.mock('@/services/grammarService', () => ({
  default: {
    getGrammarDetail: vi.fn(),
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
  exercises: [],
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

  it('shows title, pattern, explanation, and examples after loading', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(await screen.findByText('は (wa) — Topic Marker')).toBeInTheDocument();
    expect(screen.getByText('Noun + は + Predicate')).toBeInTheDocument();
    expect(screen.getByText('は marks the topic of a sentence.')).toBeInTheDocument();
    expect(screen.getByText('私は学生です。')).toBeInTheDocument();
    expect(screen.getByText('Watashi wa gakusei desu.')).toBeInTheDocument();
    expect(screen.getByText('I am a student.')).toBeInTheDocument();
  });

  it('points the user to the Learning Path for practice instead of showing exercises here', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(await screen.findByText(/practice this grammar point in the learning path/i)).toBeInTheDocument();
    // Exercise UI should be gone — no option buttons or "Question X of Y" header.
    expect(screen.queryByRole('button', { name: 'は' })).not.toBeInTheDocument();
    expect(screen.queryByText(/question \d+ of \d+/i)).not.toBeInTheDocument();
  });

  it('shows error state when fetch fails', async () => {
    const svc = grammarService as unknown as { getGrammarDetail: ReturnType<typeof vi.fn> };
    svc.getGrammarDetail.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load grammar detail/i)).toBeInTheDocument();
  });
});
