import * as signalR from '@microsoft/signalr';
import { HUB_BASE_URL } from './routes';

export function createShiritoriConnection(): signalR.HubConnection {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${HUB_BASE_URL}/hubs/shiritori`, {
      accessTokenFactory: () => localStorage.getItem('token') ?? '',
    })
    .withAutomaticReconnect()
    .build();
}
