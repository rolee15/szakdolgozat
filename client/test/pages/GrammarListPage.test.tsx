import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const navigateSpy = vi.fn();
vi.mock('react-router-dom', async (orig) => {
  const actual = await orig() as Record<string, unknown>;
  return {
    ...actual,
    useNavigate: () => navigateSpy,
  };
});

vi.mock('@/services/grammarService', () => ({
  default: {
    getGrammarPoints: vi.fn(),
  },
}));

import GrammarListPage from '@/pages/GrammarListPage';
import grammarService from '@/services/grammarService';

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
    },
  });
}

function renderPage() {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter>
        <GrammarListPage />
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const samplePoints: GrammarPoint[] = [
  {
    id: 1,
    title: 'は (wa) — Topic Marker',
    pattern: 'Noun + は + Predicate',
    jlptLevel: 5,
    correctCount: 3,
    attemptCount: 4,
    isCompleted: true,
  },
  {
    id: 2,
    title: 'が (ga) — Subject Marker',
    pattern: 'Noun + が + Predicate',
    jlptLevel: 5,
    correctCount: 1,
    attemptCount: 2,
    isCompleted: false,
  },
];

describe('GrammarListPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
    navigateSpy.mockClear();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching', () => {
    const svc = grammarService as unknown as { getGrammarPoints: ReturnType<typeof vi.fn> };
    svc.getGrammarPoints.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows grammar point cards after loading', async () => {
    const svc = grammarService as unknown as { getGrammarPoints: ReturnType<typeof vi.fn> };
    svc.getGrammarPoints.mockResolvedValue(samplePoints);

    renderPage();

    expect(await screen.findByText('は (wa) — Topic Marker')).toBeInTheDocument();
    expect(screen.getByText('が (ga) — Subject Marker')).toBeInTheDocument();
  });

  it('shows completed badge for completed grammar points', async () => {
    const svc = grammarService as unknown as { getGrammarPoints: ReturnType<typeof vi.fn> };
    svc.getGrammarPoints.mockResolvedValue(samplePoints);

    renderPage();

    expect(await screen.findByText('Completed')).toBeInTheDocument();
  });

  it('shows X/3 correct for incomplete grammar points', async () => {
    const svc = grammarService as unknown as { getGrammarPoints: ReturnType<typeof vi.fn> };
    svc.getGrammarPoints.mockResolvedValue(samplePoints);

    renderPage();

    expect(await screen.findByText('1/3 correct')).toBeInTheDocument();
  });

  it('shows error state on fetch error', async () => {
    const svc = grammarService as unknown as { getGrammarPoints: ReturnType<typeof vi.fn> };
    svc.getGrammarPoints.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load grammar points/i)).toBeInTheDocument();
  });

  it('navigates to grammar detail on card click', async () => {
    const svc = grammarService as unknown as { getGrammarPoints: ReturnType<typeof vi.fn> };
    svc.getGrammarPoints.mockResolvedValue(samplePoints);

    renderPage();

    const card = await screen.findByText('は (wa) — Topic Marker');
    fireEvent.click(card);

    expect(navigateSpy).toHaveBeenCalledWith('/grammar/1');
  });
});
