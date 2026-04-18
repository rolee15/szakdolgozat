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

vi.mock('@/services/readingService', () => ({
  default: {
    getPassages: vi.fn(),
  },
}));

import ReadingListPage from '@/pages/ReadingListPage';
import readingService from '@/services/readingService';

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
        <ReadingListPage />
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const samplePassages: ReadingPassage[] = [
  {
    id: 1,
    title: 'Spring Day',
    jlptLevel: 5,
    isPassed: true,
    score: 85,
    attemptCount: 2,
  },
  {
    id: 2,
    title: 'Summer Rain',
    jlptLevel: 4,
    isPassed: false,
    score: 40,
    attemptCount: 1,
  },
  {
    id: 3,
    title: 'Winter Snow',
    jlptLevel: 5,
    isPassed: false,
    score: 0,
    attemptCount: 0,
  },
];

describe('ReadingListPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
    navigateSpy.mockClear();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching', () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows error state on fetch error', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load reading passages/i)).toBeInTheDocument();
  });

  it('shows empty state when no passages returned', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue([]);

    renderPage();

    expect(await screen.findByText(/no passages available/i)).toBeInTheDocument();
  });

  it('renders passage cards after loading', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue(samplePassages);

    renderPage();

    expect(await screen.findByText('Spring Day')).toBeInTheDocument();
    expect(screen.getByText('Summer Rain')).toBeInTheDocument();
    expect(screen.getByText('Winter Snow')).toBeInTheDocument();
  });

  it('shows JLPT level badge on each card', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue([samplePassages[0]]);

    renderPage();

    expect(await screen.findByText('N5')).toBeInTheDocument();
  });

  it('shows Passed badge with score for passed passages', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue([samplePassages[0]]);

    renderPage();

    expect(await screen.findByText(/passed — 85%/i)).toBeInTheDocument();
  });

  it('shows score for attempted-but-not-passed passages', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue([samplePassages[1]]);

    renderPage();

    expect(await screen.findByText(/score: 40%/i)).toBeInTheDocument();
  });

  it('shows Not attempted for passages with zero attempts', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue([samplePassages[2]]);

    renderPage();

    expect(await screen.findByText(/not attempted/i)).toBeInTheDocument();
  });

  it('navigates to passage detail on card click', async () => {
    const svc = readingService as unknown as { getPassages: ReturnType<typeof vi.fn> };
    svc.getPassages.mockResolvedValue([samplePassages[0]]);

    renderPage();

    const card = await screen.findByText('Spring Day');
    fireEvent.click(card);

    expect(navigateSpy).toHaveBeenCalledWith('/reading/1');
  });
});
