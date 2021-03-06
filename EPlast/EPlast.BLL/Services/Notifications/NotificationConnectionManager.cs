﻿using EPlast.BLL.DTO.Notification;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Notifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Notifications
{
    public class NotificationConnectionManager : INotificationConnectionManager
    {
        private static readonly ConcurrentDictionary<string, HashSet<ConnectionDTO>> userMap = new ConcurrentDictionary<string, HashSet<ConnectionDTO>>();
        private readonly IUniqueIdService _uniqueId;

        public NotificationConnectionManager(IUniqueIdService uniqueId)
        {
            _uniqueId = uniqueId;
        }

        public IEnumerable<string> OnlineUsers { get { return userMap.Keys; } }

        public WebSocket GetSocketByConnectionId(string id)
        {
            foreach (var item in userMap)
            {
                var connection = item.Value.FirstOrDefault(conn => conn.ConnectionId == id);
                if (connection != null)
                {
                    return connection.WebSocket;
                }
            }
            throw new InvalidOperationException();
        }

        public HashSet<ConnectionDTO> GetConnectionsByUserId(string userId)
        {
            HashSet<ConnectionDTO> connections;
            if (userMap.TryGetValue(userId, out connections))
            {
                return connections;
            }
            return new HashSet<ConnectionDTO>();
        }

        public ConcurrentDictionary<string, HashSet<ConnectionDTO>> GetAll()
        {
            return userMap;
        }

        public string GetUserId(WebSocket socket)
        {
            var userConnections = userMap.FirstOrDefault(p => p.Value.FirstOrDefault(conn => conn.WebSocket == socket) != null);
            if(userConnections.Equals(default(KeyValuePair<string, HashSet<ConnectionDTO>>)))
            {
                throw new ArgumentException("Dictionary doesn`t contain this WebSocket", "socket");
            }
            return userConnections.Key;
        }

        public string GetConnectionId(WebSocket socket)
        {
            foreach (var item in userMap)
            {
                var connection = item.Value.FirstOrDefault(conn => conn.WebSocket == socket);
                if (connection != null)
                {
                    return connection.ConnectionId;
                }
            }
            throw new InvalidOperationException();
        }

        public string AddSocket(string userId, WebSocket socket)
        {
            var connectionId = _uniqueId.GetUniqueId().ToString();
            if (!userMap.ContainsKey(userId))
            {
                userMap.TryAdd(userId, new HashSet<ConnectionDTO>());
            }
            userMap[userId].Add(new ConnectionDTO {ConnectionId = connectionId, WebSocket = socket });
            return connectionId;
        }

        public async Task<bool> RemoveSocketAsync(string userId, string connectionId)
        {
            try
            {
                WebSocket socket = GetSocketByConnectionId(connectionId);
                var removedSocket = userMap[userId].FirstOrDefault(e => e.ConnectionId == connectionId);
                if (removedSocket != null)
                {
                    userMap[userId].Remove(removedSocket);
                    await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                            statusDescription: "Closed by the WebSocketManager",
                                            cancellationToken: CancellationToken.None);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RemoveAllUserIdSockets(string userId)
        {
            if (userMap.TryRemove(userId, out HashSet<ConnectionDTO> connections))
            {
                var tasks = connections.Select(c =>
                c.WebSocket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the WebSocketManager",
                                        cancellationToken: CancellationToken.None)
                );
                Task.WhenAll(tasks);
            }
        }

        public async Task SendMessageAsync(string userId, string message)
        {
            if (userMap.ContainsKey(userId))
            {
                var tasks = userMap[userId].Where(c => c.WebSocket.State == WebSocketState.Open)
                .Select(c =>
                    {
                        var byteArray = Encoding.UTF8.GetBytes(message);
                        return c.WebSocket.SendAsync(buffer: new ArraySegment<byte>(array: byteArray,
                                                                                    offset: 0,
                                                                                    count: byteArray.Length),
                                                    messageType: WebSocketMessageType.Text,
                                                    endOfMessage: true,
                                                    cancellationToken: CancellationToken.None);
                    });
                await Task.WhenAll(tasks);
            }
        }
    }
}
