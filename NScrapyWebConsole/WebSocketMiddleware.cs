using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NScrapyWebConsole
{
    public class WebSocketMiddleware
    {
        public static Dictionary<string,Func<HttpContext,Task<string>>> MessageProducers=new Dictionary<string, Func<HttpContext, Task<string>>>();             

        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await this._next(context);                
            }
            else
            {
                WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                while (true)
                {

                    if (context.RequestAborted.IsCancellationRequested)
                    {
                        break;
                    }
                    if (ws.State == WebSocketState.Closed)
                    {
                        break;
                    }
                    //Let's have a simple WebSocket Route approach to distribute request to different MessageProducer
                    if (MessageProducers.ContainsKey(context.Request.Path))
                    {
                        var message = await MessageProducers[context.Request.Path](context);
                        await SendMessageAsync(ws, message);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
                await ws.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
            }
            
        }

        private async Task SendMessageAsync(WebSocket ws,string message,CancellationToken ct=default(CancellationToken))
        {
            var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, ct);
        }
    }
}
