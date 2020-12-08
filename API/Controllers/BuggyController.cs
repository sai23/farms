using API.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class BuggyController :BaseApiController
    {
        
       private readonly DataContext _context;

       public BuggyController(DataContext context)
       {
           _context = context;
       }
       
       [HttpGet("Auth")]
       public ActionResult<string> GetSecret()
       {
        return "secret";
       }
       
       [HttpGet("not-found")]
       public ActionResult<string> GetNotFound()
       {
            var thing =  _context.Users.Find(-1);
            if(thing ==null)
            {
                return "secrect not found";
            }
            return Ok(thing);
       }
       
       [HttpGet("server-error")]
       public ActionResult<string> GetServerError()
       {
                var thing =  _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;
          
           
       }
       
       [HttpGet("bad-request")]
       public ActionResult<string> GetBadRequest()
       {
            return BadRequest("This is a bad request");
       }
    }
}