using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly Contacts _contacts = new();

        public ContactController() 
        {
            _contacts.FetchContacts();
        }  


        [HttpGet]
        public ActionResult<Contacts> GetContacts()
        {
            return Ok(_contacts.GetContacts());
        }

        [Authorize]
        [HttpPost("update")]
        public ActionResult UpdateContact([FromBody] ContactDto contact)
        {
            if(_contacts.UpdateContact(contact, contact.Email))
            {
                return Ok();
            }
            return BadRequest(
                "User with given email wasn't present in the database or the data was invalid.");
        }

        [Authorize]
        [HttpPost("add")]
        public ActionResult AddContact([FromBody] ContactDto contact)
        {
            if(_contacts.AddContact(contact))
            {
                return Ok();
            }
            return BadRequest(
                "User with given email already exists or data was invalid");
        }

        [Authorize]
        [HttpDelete]
        public ActionResult DropContact([FromBody] string email)
        {
            if(_contacts.DropContact(email))
            {
                return Ok();
            }
            return BadRequest("User is not present in the database!");
        }

    }
}
