#region Header
/*
 ************************************************************************************
 Name: ValuesController
 Description: This returns rest data to client 
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnePointRestAPI.Facade;

namespace OnePointRestAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseAPIController
    {

        private IValuesFacade _ValuesFacade;
        private IValuesFacade ValuesFacade
        {
            get
            {
                return _ValuesFacade ?? (_ValuesFacade = new ValuesFacade());
            }
        }

        // GET api/values
        
        [HttpGet]
        public dynamic Get()
        {
            return ValuesFacade.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public dynamic Post([FromBody] ExpandoObject value)
        {
            try
            {
                return ValuesFacade.Post((dynamic)value);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
