
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataStore;

namespace FlatReaver.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        public DivisionsController(IStore store)
        {
            Store = store;
        }

        IStore Store;

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Division>> GetGet(int? pageSize, int? pageIndex, string name)
        {
            dynamic prototype = !String.IsNullOrEmpty(name) ? new { Name = name } : null;

            return Store.Read<Division>(prototype, false, false, pageSize, pageIndex);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Division> Get(long id)
        {
            return Store.ReadSingle<Division>(new { Id = id });
        }

        [Authorize]
        [HttpPost]
        public void Post([FromBody] Division division)
        {
            Store.Create(division);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void Put([FromBody] Division division)
        {
            Store.Update(division);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            Store.Delete(new Division { Id = id });
        }
    }
}
