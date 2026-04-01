import { useState, useEffect, useRef } from 'react';
import { useInfiniteQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import kanjiService from '@/services/kanjiService';

type JlptFilter = number | null;

const JLPT_LEVELS: { label: string; value: JlptFilter }[] = [
  { label: 'All', value: null },
  { label: 'N5', value: 5 },
  { label: 'N4', value: 4 },
  { label: 'N3', value: 3 },
  { label: 'N2', value: 2 },
  { label: 'N1', value: 1 },
];

const SRS_STAGE_COLORS: Record<string, string> = {
  Apprentice: 'bg-pink-600',
  Guru: 'bg-purple-600',
  Master: 'bg-blue-600',
  Enlightened: 'bg-indigo-600',
  Burned: 'bg-gray-600',
};

const KanjiListPage = () => {
  const [selectedLevel, setSelectedLevel] = useState<JlptFilter>(null);
  const navigate = useNavigate();
  const sentinelRef = useRef<HTMLDivElement>(null);

  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isLoading,
    isError,
  } = useInfiniteQuery({
    queryKey: ['kanji', 'paged', selectedLevel],
    queryFn: ({ pageParam }) => kanjiService.getKanjiPaged(pageParam, selectedLevel),
    initialPageParam: 1,
    getNextPageParam: (lastPage) =>
      lastPage.hasNextPage ? lastPage.page + 1 : undefined,
  });

  const allKanji = data?.pages.flatMap((p) => p.items) ?? [];
  const totalCount = data?.pages[0]?.totalCount ?? 0;

  // [5] Intersection Observer pattern for infinite scroll — https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API (accessed 2026-03-30)
  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        if (entries[0].isIntersecting && hasNextPage && !isFetchingNextPage) {
          fetchNextPage();
        }
      },
      { threshold: 0.1 }
    );
    const el = sentinelRef.current;
    if (el) observer.observe(el);
    return () => {
      if (el) observer.unobserve(el);
    };
  }, [hasNextPage, isFetchingNextPage, fetchNextPage]);

  return (
    <div className="max-w-4xl mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">Kanji</h1>

      <div className="flex gap-2 mb-6 flex-wrap">
        {JLPT_LEVELS.map(({ label, value }) => (
          <button
            key={label}
            onClick={() => setSelectedLevel(value)}
            className={`px-5 py-2 rounded-lg font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2 ${
              selectedLevel === value
                ? 'bg-indigo-500 text-white'
                : 'bg-gray-700 text-gray-300 hover:bg-gray-600'
            }`}
          >
            {label}
          </button>
        ))}
      </div>

      {isLoading && (
        <div className="flex justify-center p-12">
          <span className="text-white text-lg">Loading kanji...</span>
        </div>
      )}

      {isError && (
        <div className="flex justify-center p-12">
          <span className="text-red-400 text-lg">Failed to load kanji.</span>
        </div>
      )}

      {!isLoading && !isError && (
        <>
          <p className="text-sm text-gray-400 mb-4">
            Showing {allKanji.length} of {totalCount} kanji
          </p>

          <div className="grid grid-cols-4 sm:grid-cols-6 md:grid-cols-8 gap-3">
            {allKanji.map((kanji) => {
              const stageColor = SRS_STAGE_COLORS[kanji.srsStage] ?? 'bg-gray-700';
              return (
                <button
                  key={kanji.character}
                  onClick={() => navigate(`/kanji/${encodeURIComponent(kanji.character)}`)}
                  className="flex flex-col items-center gap-1 p-2 rounded border border-blue-500 hover:bg-blue-500 hover:text-white transition-colors focus:outline-none focus:ring-2 focus:ring-blue-400"
                  aria-label={`Kanji ${kanji.character}, meaning: ${kanji.meaning}`}
                >
                  <span className="text-3xl font-bold">{kanji.character}</span>
                  <span className="text-xs text-gray-400 text-center leading-tight line-clamp-1">
                    {kanji.meaning}
                  </span>
                  <span className={`text-xs px-1.5 py-0.5 rounded text-white font-medium ${stageColor}`}>
                    {kanji.srsStage}
                  </span>
                </button>
              );
            })}
          </div>

          <div ref={sentinelRef} className="h-8" />

          {isFetchingNextPage && (
            <div className="flex justify-center py-6">
              <span className="text-white text-sm">Loading more kanji...</span>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default KanjiListPage;
