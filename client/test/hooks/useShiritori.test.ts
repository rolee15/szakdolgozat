import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook, act } from '@testing-library/react';

const mockConnection = vi.hoisted(() => ({
  start: vi.fn().mockResolvedValue(undefined),
  stop: vi.fn().mockResolvedValue(undefined),
  invoke: vi.fn().mockResolvedValue(undefined),
  on: vi.fn(),
  off: vi.fn(),
  state: 'Connected',
}));

vi.mock('@/services/shiritoriService', () => ({
  createShiritoriConnection: vi.fn(() => mockConnection),
}));

vi.mock('@microsoft/signalr', () => ({
  HubConnectionBuilder: vi.fn().mockReturnValue({
    withUrl: vi.fn().mockReturnThis(),
    withAutomaticReconnect: vi.fn().mockReturnThis(),
    build: vi.fn().mockReturnValue(mockConnection),
  }),
  HubConnectionState: { Connected: 'Connected' },
}));

import { useShiritori } from '@/hooks/useShiritori';

describe('useShiritori', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockConnection.start.mockResolvedValue(undefined);
    mockConnection.stop.mockResolvedValue(undefined);
    mockConnection.invoke.mockResolvedValue(undefined);
    mockConnection.on.mockReset();
  });

  it('starts_connection_on_mount', async () => {
    renderHook(() => useShiritori());
    await act(async () => {});
    expect(mockConnection.start).toHaveBeenCalledOnce();
  });

  it('startGame_invokes_StartGame_on_the_hub', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    await act(async () => {
      result.current.startGame();
    });

    expect(mockConnection.invoke).toHaveBeenCalledWith('StartGame');
  });

  it('startGame_sets_status_idle_when_invoke_fails', async () => {
    mockConnection.invoke.mockRejectedValue(new Error('hub error'));
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    await act(async () => {
      result.current.startGame();
    });

    expect(result.current.gameState.status).toBe('idle');
  });

  it('submitWord_invokes_SubmitWord_with_the_word', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    await act(async () => {
      result.current.submitWord('さくら');
    });

    expect(mockConnection.invoke).toHaveBeenCalledWith('SubmitWord', 'さくら');
  });

  it('submitWord_adds_player_word_to_history', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    await act(async () => {
      result.current.submitWord('さくら');
    });

    expect(result.current.gameState.history).toContainEqual({ word: 'さくら', by: 'player' });
  });

  it('submitWord_sets_error_when_invoke_fails', async () => {
    mockConnection.invoke.mockRejectedValue(new Error('network error'));
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    await act(async () => {
      result.current.submitWord('さくら');
    });

    expect(result.current.gameState.error).toBe('送信に失敗しました。');
  });

  it('abandon_invokes_Abandon_and_resets_state', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    await act(async () => {
      result.current.abandon();
    });

    expect(mockConnection.invoke).toHaveBeenCalledWith('Abandon');
    expect(result.current.gameState.status).toBe('idle');
  });

  it('stops_connection_on_unmount', async () => {
    const { unmount } = renderHook(() => useShiritori());
    await act(async () => {});

    unmount();

    expect(mockConnection.stop).toHaveBeenCalledOnce();
  });

  it('GameStarted_handler_sets_playing_state_with_computerWord', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const gameStartedHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'GameStarted',
    )?.[1];

    await act(async () => {
      gameStartedHandler({
        isValid: true,
        computerWord: 'りんご',
        nextChar: 'ご',
        gameOver: false,
        winner: null,
      });
    });

    expect(result.current.gameState.status).toBe('playing');
    expect(result.current.gameState.currentWord).toBe('りんご');
    expect(result.current.gameState.requiredChar).toBe('ご');
    expect(result.current.gameState.history).toContainEqual({ word: 'りんご', by: 'computer' });
  });

  it('GameStarted_handler_sets_empty_history_when_no_computerWord', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const gameStartedHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'GameStarted',
    )?.[1];

    await act(async () => {
      gameStartedHandler({
        isValid: true,
        computerWord: undefined,
        nextChar: 'あ',
        gameOver: false,
        winner: null,
      });
    });

    expect(result.current.gameState.history).toHaveLength(0);
  });

  it('MoveResult_handler_sets_error_when_invalid_move', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const moveResultHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'MoveResult',
    )?.[1];

    await act(async () => {
      moveResultHandler({ isValid: false, reason: '無効な言葉', gameOver: false });
    });

    expect(result.current.gameState.error).toBe('無効な言葉');
  });

  it('MoveResult_handler_adds_computerWord_to_history_on_valid_move', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const moveResultHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'MoveResult',
    )?.[1];

    await act(async () => {
      moveResultHandler({
        isValid: true,
        computerWord: 'ごりら',
        nextChar: 'ら',
        gameOver: false,
      });
    });

    expect(result.current.gameState.history).toContainEqual({ word: 'ごりら', by: 'computer' });
    expect(result.current.gameState.requiredChar).toBe('ら');
  });

  it('MoveResult_handler_does_not_add_computerWord_when_absent', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const moveResultHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'MoveResult',
    )?.[1];

    await act(async () => {
      moveResultHandler({ isValid: true, computerWord: undefined, nextChar: 'ら', gameOver: false });
    });

    expect(result.current.gameState.history).toHaveLength(0);
  });

  it('MoveResult_handler_sets_game_over_with_winner', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const moveResultHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'MoveResult',
    )?.[1];

    await act(async () => {
      moveResultHandler({ isValid: true, gameOver: true, winner: 'player', computerWord: undefined });
    });

    expect(result.current.gameState.status).toBe('over');
    expect(result.current.gameState.winner).toBe('player');
  });

  it('MoveResult_handler_adds_computerWord_to_history_on_game_over', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const moveResultHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'MoveResult',
    )?.[1];

    await act(async () => {
      moveResultHandler({
        isValid: true,
        gameOver: true,
        winner: 'computer',
        computerWord: 'らくだ',
      });
    });

    expect(result.current.gameState.history).toContainEqual({ word: 'らくだ', by: 'computer' });
  });

  it('MoveResult_handler_game_over_with_invalid_does_not_add_computerWord', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const moveResultHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'MoveResult',
    )?.[1];

    await act(async () => {
      moveResultHandler({ isValid: false, gameOver: true, winner: 'computer', computerWord: 'らくだ' });
    });

    expect(result.current.gameState.status).toBe('over');
    expect(result.current.gameState.history).toHaveLength(0);
  });

  it('GameAbandoned_handler_resets_state', async () => {
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    const gameAbandonedHandler = mockConnection.on.mock.calls.find(
      ([event]) => event === 'GameAbandoned',
    )?.[1];

    await act(async () => {
      gameAbandonedHandler();
    });

    expect(result.current.gameState.status).toBe('idle');
  });

  it('connection_start_failure_sets_status_idle', async () => {
    mockConnection.start.mockRejectedValue(new Error('connect failed'));
    const { result } = renderHook(() => useShiritori());
    await act(async () => {});

    expect(result.current.gameState.status).toBe('idle');
  });
});
