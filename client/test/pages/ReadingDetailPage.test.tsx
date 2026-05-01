import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

vi.mock('@/services/readingService', () => ({
  default: {
    getPassageDetail: vi.fn(),
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

  it('points the user to the Learning Path final unit instead of rendering comprehension questions', async () => {
    const svc = readingService as unknown as { getPassageDetail: ReturnType<typeof vi.fn> };
    svc.getPassageDetail.mockResolvedValue(sampleDetail);

    renderPage();

    expect(
      await screen.findByText(/test your comprehension in the final unit of the learning path/i),
    ).toBeInTheDocument();
    // Comprehension question UI should be gone — no submit button, no question text, no radios.
    expect(screen.queryByRole('button', { name: /submit/i })).not.toBeInTheDocument();
    expect(screen.queryByText('What season is described?')).not.toBeInTheDocument();
    expect(screen.queryByRole('radio')).not.toBeInTheDocument();
  });
});
