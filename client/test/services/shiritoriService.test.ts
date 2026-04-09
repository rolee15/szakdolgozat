import { describe, it, expect, vi } from 'vitest';

vi.mock('@/services/routes', () => ({
  HUB_BASE_URL: 'https://test.local',
}));

vi.mock('@microsoft/signalr', () => {
  const mockConnection = {
    start: vi.fn().mockResolvedValue(undefined),
    stop: vi.fn().mockResolvedValue(undefined),
    invoke: vi.fn().mockResolvedValue(undefined),
    on: vi.fn(),
    off: vi.fn(),
    state: 'Connected',
  };
  return {
    HubConnectionBuilder: vi.fn().mockReturnValue({
      withUrl: vi.fn().mockReturnThis(),
      withAutomaticReconnect: vi.fn().mockReturnThis(),
      build: vi.fn().mockReturnValue(mockConnection),
    }),
    HubConnectionState: { Connected: 'Connected' },
  };
});

import * as signalR from '@microsoft/signalr';
import { createShiritoriConnection } from '@/services/shiritoriService';

describe('shiritoriService', () => {
  it('creates_connection_with_shiritori_hub_url', () => {
    createShiritoriConnection();
    const builder = new signalR.HubConnectionBuilder();
    expect(builder.withUrl).toBeDefined();
    const MockBuilder = signalR.HubConnectionBuilder as ReturnType<typeof vi.fn>;
    const instance = MockBuilder.mock.results[0]?.value as { withUrl: ReturnType<typeof vi.fn> };
    expect(instance.withUrl).toHaveBeenCalledWith(
      'https://test.local/hubs/shiritori',
      expect.objectContaining({ accessTokenFactory: expect.any(Function) }),
    );
  });
});
