import { useRef } from 'react';
import { useShiritori } from '@/hooks/useShiritori';
import { useForm } from 'react-hook-form';

interface WordForm {
  word: string;
}

const ShiritoriPage = () => {
  const { gameState, startGame, submitWord, abandon, isConnected } = useShiritori();
  const { status, requiredChar, history, winner, error } = gameState;
  const historyEndRef = useRef<HTMLDivElement>(null);

  const { register, handleSubmit, reset, formState: { isSubmitting } } = useForm<WordForm>();

  const onSubmit = (data: WordForm) => {
    const trimmed = data.word.trim();
    if (!trimmed) return;
    submitWord(trimmed);
    reset();
  };

  return (
    <div className="flex flex-col items-center gap-8 p-8 max-w-2xl mx-auto">
      <div className="flex items-center gap-2 self-start">
        <span
          className={`inline-block w-2 h-2 rounded-full ${isConnected ? 'bg-green-400' : 'bg-yellow-400'}`}
          aria-hidden="true"
        />
        <span className="text-gray-400 text-sm">
          {isConnected ? '接続済み' : '接続中'}
        </span>
      </div>

      <h1 className="text-3xl font-bold text-white">しりとり</h1>

      {status === 'idle' && (
        <div className="flex flex-col items-center gap-6 text-center">
          <p className="text-gray-400 text-lg">
            1文字ずつ言葉を繋げよう！（ん で終わったら負け）
          </p>
          <button
            onClick={startGame}
            className="px-8 py-3 bg-indigo-500 text-white font-medium rounded-lg hover:bg-indigo-600 transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2"
          >
            ゲームを始める
          </button>
        </div>
      )}

      {status === 'connecting' && (
        <div className="text-gray-400">ゲームを開始しています...</div>
      )}

      {(status === 'playing' || status === 'over') && (
        <div className="flex flex-col gap-6 w-full">
          {status === 'playing' && (
            <>
              <div className="bg-gray-800 rounded-lg p-4 text-center">
                <p className="text-gray-400 text-sm mb-1">次の言葉</p>
                <p className="text-white text-xl font-semibold">
                  「{requiredChar}」で始まる言葉を入力してください
                </p>
              </div>

              <form onSubmit={handleSubmit(onSubmit)} className="flex gap-3">
                <input
                  {...register('word', { required: true })}
                  type="text"
                  placeholder="ひらがなで入力..."
                  autoComplete="off"
                  className="flex-1 px-4 py-2 bg-gray-700 text-white rounded-lg border border-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                />
                <button
                  type="submit"
                  disabled={isSubmitting}
                  className="px-6 py-2 bg-indigo-500 text-white font-medium rounded-lg hover:bg-indigo-600 transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 disabled:opacity-50"
                >
                  送信
                </button>
              </form>

              {error && (
                <p className="text-red-400 text-sm text-center">{error}</p>
              )}

              <button
                onClick={abandon}
                className="text-gray-500 text-sm hover:text-gray-300 transition-colors self-center"
              >
                ゲームを終了する
              </button>
            </>
          )}

          {status === 'over' && (
            <div className="flex flex-col items-center gap-4">
              <div className="bg-gray-800 rounded-lg p-6 text-center w-full">
                <p className="text-2xl font-bold text-white">
                  {winner === 'player' ? 'あなたの勝ち！' : 'コンピューターの勝ち！'}
                </p>
              </div>
              <button
                onClick={startGame}
                className="px-8 py-3 bg-indigo-500 text-white font-medium rounded-lg hover:bg-indigo-600 transition-colors focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-2"
              >
                もう一度プレイ
              </button>
            </div>
          )}

          {history.length > 0 && (
            <div className="flex flex-col gap-2 max-h-80 overflow-y-auto">
              <h2 className="text-gray-400 text-sm uppercase tracking-wider">履歴</h2>
              {history.map((entry, i) => (
                <div
                  key={i}
                  className={`flex items-center gap-3 px-4 py-2 rounded-lg ${
                    entry.by === 'player' ? 'bg-indigo-900' : 'bg-gray-800'
                  }`}
                >
                  <span className="text-gray-400 text-xs w-28 shrink-0">
                    {entry.by === 'player' ? 'あなた' : 'コンピューター'}
                  </span>
                  <span className="text-white font-medium">{entry.word}</span>
                </div>
              ))}
              <div ref={historyEndRef} />
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default ShiritoriPage;
