using System;
using Microsoft.AspNetCore.Mvc;
using TestAPI.API.Models;
using TestAPI.DAL;

namespace TestAPI.API.Controllers 
{
   
    [ApiController]
    public class ActivityLogger : ControllerBase
    {
        
        private Dal _dal;
        
        public ActivityLogger(Dal dal)
        {
            _dal = dal;
        }
        
        [HttpGet("get")]
        public IActionResult GetActivity()
        {
           
            return Ok(_dal.GetAllData());

        }
        
        [HttpPost("add")]
        public IActionResult AddActivity([FromBody]Activity activityInfo)
        {
            return Ok(_dal.InsertData(activityInfo.Name, DateTime.Now));

        }

        [HttpPatch("edit")]
        public IActionResult EditActivity([FromBody] Activity activityInfo)
        {
            var activity = _dal.EditActivity(activityInfo.Id, activityInfo.Name, activityInfo.CurrentDate);
            if (activity != null)
            {
                
                return Ok(activity);
            }
            else
            {
                return Ok("Id does not exist (:");
            }
        }
        
        [HttpDelete("delete")]
        public IActionResult DeleteActivity([FromBody] Activity activityInfo)
        {
            return Ok(_dal.DeleteActivity(activityInfo.Id));
        }
        
        
        
        [HttpPost("add-session")]
        public IActionResult AddSession([FromHeader] long PersonId)
        {
            var SessionId = Guid.NewGuid().ToString();
            _dal.CreateSession(PersonId, SessionId);
            return Ok(SessionId);
        }
        
        [HttpPost("get-session-id")]
        public IActionResult CheckSession([FromHeader] string SessionId)
        {
            return Ok(_dal.CheckSession(SessionId));
        }
    }
}
