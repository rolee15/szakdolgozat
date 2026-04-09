import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import type { ShiritoriState } from '@/types/Shiritori';

vi.mock('@/hooks/useShiritori');

import { useShiritori } from '@/hooks/useShiritori';

const mockUseShiritori = useShiritori as ReturnType<typeof vi.fn>;

const mockStartGame = vi.fn();
const mockSubmitWord = vi.fn();
const mockAbandon = vi.fn();

function makeState(overrides: Partial<ShiritoriState> = {}): ShiritoriState {
  return {
    status: 'idle',
    currentWord: '',
    requiredChar: '',
    history: [],
    ...overrides,
  };
}

function mockHook(overrides: Partial<ShiritoriState> = {}, isConnected = true) {
  mockUseShiritori.mockReturnValue({
    gameState: makeState(overrides),
    startGame: mockStartGame,
    submitWord: mockSubmitWord,
    abandon: mockAbandon,
    isConnected,
  });
}

import ShiritoriPage from '@/pages/ShiritoriPage';

describe('ShiritoriPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders_start_game_button_when_status_is_idle', () => {
    mockHook({ status: 'idle' });
    render(<ShiritoriPage />);
    expect(screen.getByRole('button', { name: 'ゲームを始める' })).toBeInTheDocument();
  });

  it('calls_startGame_when_start_button_is_clicked', () => {
    mockHook({ status: 'idle' });
    render(<ShiritoriPage />);
    fireEvent.click(screen.getByRole('button', { name: 'ゲームを始める' }));
    expect(mockStartGame).toHaveBeenCalledOnce();
  });

  it('shows_connecting_message_when_status_is_connecting', () => {
    mockHook({ status: 'connecting' });
    render(<ShiritoriPage />);
    expect(screen.getByText('ゲームを開始しています...')).toBeInTheDocument();
  });

  it('renders_required_character_prompt_when_status_is_playing', () => {
    mockHook({ status: 'playing', requiredChar: 'さ' });
    render(<ShiritoriPage />);
    expect(screen.getByText(/「さ」で始まる言葉を入力してください/)).toBeInTheDocument();
  });

  it('renders_error_message_when_error_is_set', () => {
    mockHook({ status: 'playing', requiredChar: 'さ', error: '無効な言葉です' });
    render(<ShiritoriPage />);
    expect(screen.getByText('無効な言葉です')).toBeInTheDocument();
  });

  it('does_not_render_error_when_error_is_undefined', () => {
    mockHook({ status: 'playing', requiredChar: 'さ', error: undefined });
    render(<ShiritoriPage />);
    expect(screen.queryByText('無効な言葉です')).not.toBeInTheDocument();
  });

  it('calls_abandon_when_abandon_button_is_clicked', () => {
    mockHook({ status: 'playing', requiredChar: 'さ' });
    render(<ShiritoriPage />);
    fireEvent.click(screen.getByRole('button', { name: 'ゲームを終了する' }));
    expect(mockAbandon).toHaveBeenCalledOnce();
  });

  it('renders_player_winner_announcement_when_status_is_over', () => {
    mockHook({ status: 'over', winner: 'player' });
    render(<ShiritoriPage />);
    expect(screen.getByText('あなたの勝ち！')).toBeInTheDocument();
  });

  it('renders_computer_winner_announcement_when_status_is_over', () => {
    mockHook({ status: 'over', winner: 'computer' });
    render(<ShiritoriPage />);
    expect(screen.getByText('コンピューターの勝ち！')).toBeInTheDocument();
  });

  it('calls_startGame_when_play_again_is_clicked', () => {
    mockHook({ status: 'over', winner: 'player' });
    render(<ShiritoriPage />);
    fireEvent.click(screen.getByRole('button', { name: 'もう一度プレイ' }));
    expect(mockStartGame).toHaveBeenCalledOnce();
  });

  it('renders_word_history_when_history_is_present', () => {
    mockHook({
      status: 'playing',
      requiredChar: 'ら',
      history: [
        { word: 'りんご', by: 'computer' },
        { word: 'ごりら', by: 'player' },
      ],
    });
    render(<ShiritoriPage />);
    expect(screen.getByText('りんご')).toBeInTheDocument();
    expect(screen.getByText('ごりら')).toBeInTheDocument();
  });

  it('does_not_render_history_section_when_history_is_empty', () => {
    mockHook({ status: 'playing', requiredChar: 'さ', history: [] });
    render(<ShiritoriPage />);
    expect(screen.queryByText('履歴')).not.toBeInTheDocument();
  });

  it('shows_connected_indicator_when_isConnected_is_true', () => {
    mockHook({ status: 'idle' }, true);
    render(<ShiritoriPage />);
    expect(screen.getByText('接続済み')).toBeInTheDocument();
  });

  it('shows_connecting_indicator_when_isConnected_is_false', () => {
    mockHook({ status: 'idle' }, false);
    render(<ShiritoriPage />);
    expect(screen.getByText('接続中')).toBeInTheDocument();
  });

  it('renders_history_for_over_status', () => {
    mockHook({
      status: 'over',
      winner: 'player',
      history: [
        { word: 'さくら', by: 'computer' },
        { word: 'らくだ', by: 'player' },
      ],
    });
    render(<ShiritoriPage />);
    expect(screen.getByText('さくら')).toBeInTheDocument();
    expect(screen.getByText('らくだ')).toBeInTheDocument();
  });
});
