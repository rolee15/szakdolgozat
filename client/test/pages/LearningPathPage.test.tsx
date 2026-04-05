import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const navigateSpy = vi.fn();
vi.mock('react-router-dom', async (orig) => {
  const actual = await orig();
  return {
    ...actual,
    useNavigate: () => navigateSpy,
  };
});

vi.mock('@/services/pathService', () => ({
  default: {
    getPath: vi.fn(),
  },
}));

import LearningPathPage from '@/pages/LearningPathPage';
import pathService from '@/services/pathService';

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
        <LearningPathPage />
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const unlockedUnit: LearningUnit = {
  id: 1,
  title: 'Unit 1: Hiragana Basics',
  description: 'Learn the basics.',
  sortOrder: 1,
  contentCount: 5,
  isPassed: false,
  bestScore: 0,
  isUnlocked: true,
};

const passedUnit: LearningUnit = {
  id: 2,
  title: 'Unit 2: Katakana',
  description: 'Learn katakana.',
  sortOrder: 2,
  contentCount: 4,
  isPassed: true,
  bestScore: 90,
  isUnlocked: true,
};

const lockedUnit: LearningUnit = {
  id: 3,
  title: 'Unit 3: Advanced',
  description: 'Advanced topics.',
  sortOrder: 3,
  contentCount: 6,
  isPassed: false,
  bestScore: 0,
  isUnlocked: false,
};

describe('LearningPathPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
    navigateSpy.mockClear();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching', () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('shows error state on fetch error', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load learning path/i)).toBeInTheDocument();
  });

  it('renders unit cards after loading', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([unlockedUnit, lockedUnit]);

    renderPage();

    expect(await screen.findByText('Unit 1: Hiragana Basics')).toBeInTheDocument();
    expect(screen.getByText('Unit 3: Advanced')).toBeInTheDocument();
  });

  it('shows Passed badge for passed units', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([passedUnit]);

    renderPage();

    expect(await screen.findByText('Passed')).toBeInTheDocument();
  });

  it('shows Unlocked badge for unlocked-but-not-passed units', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([unlockedUnit]);

    renderPage();

    expect(await screen.findByText('Unlocked')).toBeInTheDocument();
  });

  it('shows Locked badge for locked units', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([lockedUnit]);

    renderPage();

    expect(await screen.findByText('Locked')).toBeInTheDocument();
  });

  it('shows best score when bestScore > 0', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([passedUnit]);

    renderPage();

    expect(await screen.findByText(/best score: 90%/i)).toBeInTheDocument();
  });

  it('does not show best score when bestScore is 0', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([unlockedUnit]);

    renderPage();

    await screen.findByText('Unit 1: Hiragana Basics');
    expect(screen.queryByText(/best score:/i)).not.toBeInTheDocument();
  });

  it('navigates to unit detail when unlocked unit is clicked', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([unlockedUnit]);

    renderPage();

    const card = await screen.findByText('Unit 1: Hiragana Basics');
    fireEvent.click(card);

    expect(navigateSpy).toHaveBeenCalledWith('/path/1');
  });

  it('does not navigate when locked unit is clicked', async () => {
    const svc = pathService as unknown as { getPath: ReturnType<typeof vi.fn> };
    svc.getPath.mockResolvedValue([lockedUnit]);

    renderPage();

    const card = await screen.findByText('Unit 3: Advanced');
    fireEvent.click(card);

    expect(navigateSpy).not.toHaveBeenCalled();
  });
});
