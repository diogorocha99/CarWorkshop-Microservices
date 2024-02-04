﻿using Microsoft.AspNetCore.Mvc;

namespace MSRepairs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        // GET: api/<PartController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PartController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }

}
