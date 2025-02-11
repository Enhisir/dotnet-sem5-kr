import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private gameRoomId: string | null = null;

  constructor() {
    this.connection = null;
    this.gameRoomId = null;
  }

  async connect(gameRoomId: string) {
    this.gameRoomId = gameRoomId;
    const token = localStorage.getItem('accessToken'); // Получите токен из вашего хранилища

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_API_HOST}/hubs/gameRoom?gameRoomId=${gameRoomId}`, {
        accessTokenFactory: () => token || '',
        withCredentials: true,
        transport: HttpTransportType.LongPolling
      })
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log("SignalR connected");
    } catch (err) {
      console.error("SignalR connection error:", err);
    }
  }

  async disconnect() {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      this.gameRoomId = null;
    }
  }

  async send(method: string, ...args: any[]) {
    if (this.connection) {
      try {
        await this.connection.invoke(method, ...args);
      } catch (err) {
        console.error(`Error invoking method ${method}:`, err);
      }
    }
  }

  on(method: string, callback: (...args: any[]) => void) {
    if (this.connection) {
      this.connection.on(method, callback);
    }
  }

  off(method: string, callback: (...args: any[]) => void) {
    if (this.connection) {
      this.connection.off(method, callback);
    }
  }
}

export const signalRService = new SignalRService();