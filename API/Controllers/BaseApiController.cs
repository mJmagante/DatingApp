using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUsersActivity))]
    [ApiController]
    [Route("api/[controller]")] // /api/users
    public class BaseApiController : ControllerBase
    {
        
    }
}