import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const navigateSpy = vi.fn();
vi.mock('react-router-dom', async (orig) => {
  const actual = await orig() as Record<string, unknown>;
  return {
    ...actual,
    useNavigate: () => navigateSpy,
  };
});

vi.mock('@/services/pathService', () => ({
  default: {
    getUnitDetail: vi.fn(),
  },
}));

import UnitDetailPage from '@/pages/UnitDetailPage';
import pathService from '@/services/pathService';

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
    },
  });
}

function renderPage(unitId = '1') {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter initialEntries={[`/path/${unitId}`]}>
        <Routes>
          <Route path="/path/:unitId" element={<UnitDetailPage />} />
          <Route path="/path" element={<div>Path List</div>} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const sampleUnit: LearningUnitDetail = {
  id: 1,
  title: 'Unit 1: Hiragana Basics',
  description: 'Learn the hiragana alphabet.',
  sortOrder: 1,
  contentCount: 2,
  isPassed: false,
  bestScore: 0,
  isUnlocked: true,
  contents: [
    { contentType: 'Kana', contentId: 1, title: 'あ (a)' },
    { contentType: 'Grammar', contentId: 5, title: 'は particle' },
  ],
  testQuestionCount: 5,
};

describe('UnitDetailPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
    navigateSpy.mockClear();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching', () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows error state when fetch fails', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load unit detail/i)).toBeInTheDocument();
  });

  it('renders unit title and description after loading', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockResolvedValue(sampleUnit);

    renderPage();

    expect(await screen.findByText('Unit 1: Hiragana Basics')).toBeInTheDocument();
    expect(screen.getByText('Learn the hiragana alphabet.')).toBeInTheDocument();
  });

  it('renders content items', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockResolvedValue(sampleUnit);

    renderPage();

    expect(await screen.findByText('あ (a)')).toBeInTheDocument();
    expect(screen.getByText('は particle')).toBeInTheDocument();
  });

  it('shows content type badges', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockResolvedValue(sampleUnit);

    renderPage();

    expect(await screen.findByText('Kana')).toBeInTheDocument();
    expect(screen.getByText('Grammar')).toBeInTheDocument();
  });

  it('shows no content items message when contents is empty', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockResolvedValue({ ...sampleUnit, contents: [] });

    renderPage();

    expect(await screen.findByText(/no content items/i)).toBeInTheDocument();
  });

  it('navigates to test page when Take Test is clicked', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockResolvedValue(sampleUnit);

    renderPage();

    await screen.findByText('Unit 1: Hiragana Basics');
    fireEvent.click(screen.getByRole('button', { name: /take test/i }));

    expect(navigateSpy).toHaveBeenCalledWith('/path/1/test');
  });

  it('navigates back to path when Back button is clicked', async () => {
    const svc = pathService as unknown as { getUnitDetail: ReturnType<typeof vi.fn> };
    svc.getUnitDetail.mockResolvedValue(sampleUnit);

    renderPage();

    await screen.findByText('Unit 1: Hiragana Basics');
    fireEvent.click(screen.getByRole('button', { name: /back to learning path/i }));

    expect(navigateSpy).toHaveBeenCalledWith('/path');
  });
});
