using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TestAPI.API.Models;
using TestAPI.DAL;

namespace TestAPI.API.MW
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Dal _dal;

        public AuthMiddleware(RequestDelegate next, Dal dal)
        {
            _next = next;
            _dal = dal;
        }
        
        
        public async Task Invoke(HttpContext context)
        {
            var Url = context.Request.Path.Value;
            var ContentType = context.Request.ContentType;
            var SessionId = context.Request.Headers["SessionId"].ToString();
            var PersonId = context.Request.Headers["PersonId"].ToString();
            
            if (Url.Contains("/add-session"))
            {
                // if (SessionId == null || SessionId.Length != 36 )
                // {
                //     throw new Exception("Session Id is missing or Wrong");
                // }
                if (PersonId == null || PersonId.Length != 10 )
                {
                    throw new Exception("Id is Wrong or Empty");
                }
               
            }

            if (Url.Contains("/get-session-id"))
            {
                if (SessionId == null)
                {
                    throw new Exception("Empty Session Id");
                }
                if (SessionId.Length != 36)
                {
                    throw new Exception("Invalid Session Id");
                }

                Session session = _dal.CheckSession(SessionId);
                if (session == null)
                {
                    throw new Exception("Session Id Does not exist");
                }else if (session.ExpirationDate < DateTime.Now)
                {
                    throw new Exception("Session expired");   
                }
                
            }
            
            await _next(context);
        }
    }
    
}