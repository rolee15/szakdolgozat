import { useEffect, useRef, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { createShiritoriConnection } from '@/services/shiritoriService';
import type { MoveResult, ShiritoriState } from '@/types/Shiritori';

const initialState: ShiritoriState = {
  status: 'idle',
  currentWord: '',
  requiredChar: '',
  history: [],
};

export function useShiritori() {
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const [gameState, setGameState] = useState<ShiritoriState>(initialState);

  useEffect(() => {
    const connection = createShiritoriConnection();
    connectionRef.current = connection;

    connection.on('GameStarted', (result: MoveResult) => {
      setGameState({
        status: 'playing',
        currentWord: result.computerWord ?? '',
        requiredChar: result.nextChar ?? '',
        history: result.computerWord
          ? [{ word: result.computerWord, by: 'computer' }]
          : [],
      });
    });

    connection.on('MoveResult', (result: MoveResult) => {
      setGameState((prev) => {
        if (result.gameOver) {
          const newHistory = [...prev.history];
          if (result.isValid && result.computerWord) {
            newHistory.push({ word: result.computerWord, by: 'computer' });
          }
          return {
            ...prev,
            status: 'over',
            winner: result.winner,
            history: newHistory,
            error: undefined,
          };
        }

        if (!result.isValid) {
          return { ...prev, error: result.reason };
        }

        const newHistory = [...prev.history];
        if (result.computerWord) {
          newHistory.push({ word: result.computerWord, by: 'computer' });
        }

        return {
          ...prev,
          requiredChar: result.nextChar ?? prev.requiredChar,
          history: newHistory,
          error: undefined,
        };
      });
    });

    connection.on('GameAbandoned', () => {
      setGameState(initialState);
    });

    connection.start().catch(() => {
      setGameState((prev) => ({ ...prev, status: 'idle' }));
    });

    return () => {
      connection.stop();
    };
  }, []);

  const isConnected =
    connectionRef.current?.state === signalR.HubConnectionState.Connected;

  const startGame = () => {
    setGameState((prev) => ({ ...prev, status: 'connecting' }));
    connectionRef.current?.invoke('StartGame').catch(() => {
      setGameState((prev) => ({ ...prev, status: 'idle' }));
    });
  };

  const submitWord = (word: string) => {
    setGameState((prev) => {
      const newHistory = [...prev.history, { word, by: 'player' as const }];
      return { ...prev, history: newHistory, error: undefined };
    });
    connectionRef.current?.invoke('SubmitWord', word).catch(() => {
      setGameState((prev) => ({ ...prev, error: '送信に失敗しました。' }));
    });
  };

  const abandon = () => {
    connectionRef.current?.invoke('Abandon').catch(() => {});
    setGameState(initialState);
  };

  return { gameState, startGame, submitWord, abandon, isConnected };
}
