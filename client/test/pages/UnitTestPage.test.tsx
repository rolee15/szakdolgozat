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
    getUnitTest: vi.fn(),
    submitTest: vi.fn(),
  },
}));

import UnitTestPage from '@/pages/UnitTestPage';
import pathService from '@/services/pathService';

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

function renderPage(unitId = '1') {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter initialEntries={[`/path/${unitId}/test`]}>
        <Routes>
          <Route path="/path/:unitId/test" element={<UnitTestPage />} />
          <Route path="/path/:unitId" element={<div>Unit Detail</div>} />
          <Route path="/path" element={<div>Path List</div>} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>,
  );
}

const unlockedUnitDetail: LearningUnitDetail = {
  id: 1,
  title: 'Unit 1',
  description: 'Desc',
  sortOrder: 1,
  contentCount: 2,
  isPassed: false,
  bestScore: 0,
  isUnlocked: true,
  contents: [],
  testQuestionCount: 2,
};

const lockedUnitDetail: LearningUnitDetail = {
  ...unlockedUnitDetail,
  isUnlocked: false,
};

const sampleTest: UnitTest = {
  questions: [
    { id: 1, questionText: 'Q1?', options: { A: 'Opt A', B: 'Opt B', C: 'Opt C', D: 'Opt D' } },
    { id: 2, questionText: 'Q2?', options: { A: 'Ans A', B: 'Ans B', C: 'Ans C', D: 'Ans D' } },
  ],
};

const passedResult: UnitTestResult = {
  score: 100,
  isPassed: true,
  correctCount: 2,
  totalQuestions: 2,
};

const failedResult: UnitTestResult = {
  score: 50,
  isPassed: false,
  correctCount: 1,
  totalQuestions: 2,
};

describe('UnitTestPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
    navigateSpy.mockClear();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading state while fetching unit detail', () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockReturnValue(new Promise(() => {}));
    svc.getUnitTest.mockReturnValue(new Promise(() => {}));

    renderPage();

    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it('redirects to /path when unit is locked', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(lockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);

    renderPage();

    expect(await screen.findByText('Path List')).toBeInTheDocument();
  });

  it('shows unit error when unit detail fetch fails', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockRejectedValue(new Error('Network error'));
    svc.getUnitTest.mockResolvedValue(sampleTest);

    renderPage();

    expect(await screen.findByText(/failed to load unit/i)).toBeInTheDocument();
  });

  it('shows test error when test fetch fails', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockRejectedValue(new Error('Network error'));

    renderPage();

    expect(await screen.findByText(/failed to load test/i)).toBeInTheDocument();
  });

  it('renders questions with options after loading', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);

    renderPage();

    expect(await screen.findByText('Q1?')).toBeInTheDocument();
    expect(screen.getByText(/A: Opt A/)).toBeInTheDocument();
    expect(screen.getByText('Q2?')).toBeInTheDocument();
  });

  it('shows no questions message when test has empty questions', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue({ questions: [] });

    renderPage();

    expect(await screen.findByText(/no questions available/i)).toBeInTheDocument();
  });

  it('submits and shows passed result', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
      submitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);
    svc.submitTest.mockResolvedValue(passedResult);

    renderPage();

    await screen.findByText('Q1?');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText('100%')).toBeInTheDocument();
    expect(screen.getByText('Passed')).toBeInTheDocument();
    expect(await screen.findByText(/congratulations/i)).toBeInTheDocument();
  });

  it('submits and shows failed result', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
      submitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);
    svc.submitTest.mockResolvedValue(failedResult);

    renderPage();

    await screen.findByText('Q1?');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText('50%')).toBeInTheDocument();
    expect(screen.getByText('Failed')).toBeInTheDocument();
    expect(await screen.findByText(/you need 70%/i)).toBeInTheDocument();
  });

  it('shows submit error when mutation fails', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
      submitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);
    svc.submitTest.mockRejectedValue(new Error('Network error'));

    renderPage();

    await screen.findByText('Q1?');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));

    expect(await screen.findByText(/failed to submit test/i)).toBeInTheDocument();
  });

  it('resets to question form after clicking Retake Test', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
      submitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);
    svc.submitTest.mockResolvedValue(passedResult);

    renderPage();

    await screen.findByText('Q1?');
    fireEvent.click(screen.getByRole('button', { name: /submit/i }));
    await screen.findByText('100%');

    fireEvent.click(screen.getByRole('button', { name: /retake test/i }));

    expect(screen.getByText('Unit Test')).toBeInTheDocument();
    expect(screen.getByText('Q1?')).toBeInTheDocument();
  });

  it('navigates back to unit when Back to Unit button is clicked', async () => {
    const svc = pathService as unknown as {
      getUnitDetail: ReturnType<typeof vi.fn>;
      getUnitTest: ReturnType<typeof vi.fn>;
    };
    svc.getUnitDetail.mockResolvedValue(unlockedUnitDetail);
    svc.getUnitTest.mockResolvedValue(sampleTest);

    renderPage();

    await screen.findByText('Q1?');
    fireEvent.click(screen.getByRole('button', { name: /back to unit/i }));

    expect(navigateSpy).toHaveBeenCalledWith('/path/1');
  });
});
