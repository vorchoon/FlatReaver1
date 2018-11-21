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
    public class RieltorsController : ControllerBase
    {
        public RieltorsController(IStore store)
        {
            Store = store;
        }

        IStore Store;

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Rieltor>> Get(int? pageSize,  int? pageIndex, string firstname, string lastname)
        {
            dynamic prototype = GetPrototype( firstname, lastname);


            return Store.Read<Rieltor>(prototype, false, false, pageSize, pageIndex);
        }

        private dynamic GetPrototype(string firstname, string lastname)
        {
            if (!String.IsNullOrWhiteSpace(firstname) && !String.IsNullOrWhiteSpace(lastname))
            {
                return new { Firstname = firstname, Lastname = lastname };
            } else
            if (!String.IsNullOrWhiteSpace(firstname) && String.IsNullOrWhiteSpace(lastname))
            {
                return new { Firstname = firstname };
            }
            else
            if (String.IsNullOrWhiteSpace(firstname) && !String.IsNullOrWhiteSpace(lastname))
            {
                return new { Lastname = lastname };
            }
            else
            {
                return null;
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Rieltor> Get(long id)
        {
            return Store.ReadSingle<Rieltor>(new { Id = id });
        }

        [Authorize]
        [HttpPost]
        public void Post([FromBody] Rieltor rieltor)
        {
            Store.Create(rieltor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void Put([FromBody] Rieltor rieltor)
        {
            Store.Update(rieltor);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            
            Store.Delete(new Rieltor { Id = id });
        }
    }
}
