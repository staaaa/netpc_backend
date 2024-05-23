using Backend.Services;

namespace Backend.Models
{
    public class Contacts
    {
        private List<ContactDto> _contactList { get; set; }
        private ContactsDbService _dbService { get; set; }
        public Contacts() 
        {
            _contactList = new List<ContactDto>();
            _dbService = new ContactsDbService();
        }

        //this method fetches the contacts from the database
        public void FetchContacts()
        {
            _contactList = _dbService.GetAllContacts();
        }

        public List<ContactDto> GetContacts()
        {
            return _contactList;
        }
        public bool AddContact(ContactDto contact)
        {
            //check if user with given email is not already present in the database
            if(_dbService.GetContact(contact.Email)  == null)
            {
                if(_dbService.InsertContact(contact))
                {
                    _contactList.Add(contact);
                    return true;
                }
                return false;
            }
            return false;
        }
        public bool DropContact(string email)
        {
            int index = _contactList.FindIndex(c => c.Email == email);
            if (index != -1)
            {
                _contactList.RemoveAt(index);
                _dbService.DropContact(email);
                return true;
            }
            return false;
        }
        public bool UpdateContact(ContactDto updatedContact, string email)
        {
            int index = _contactList.FindIndex(c => c.Email == email);
            if (index != -1)
            {
                // Update the existing contact with new values
                _contactList[index].Name = updatedContact.Name;
                _contactList[index].Surname = updatedContact.Surname;
                _contactList[index].Email = updatedContact.Email;
                _contactList[index].Password = updatedContact.Password;
                _contactList[index].Phone = updatedContact.Phone;
                _contactList[index].Category = updatedContact.Category;
                _contactList[index].Subcategory = updatedContact.Subcategory;
                _contactList[index].DateOfBirth = updatedContact.DateOfBirth;

                _dbService.UpdateContact(updatedContact, email);

                return true;
            }
            return false;
        }

    }
}
