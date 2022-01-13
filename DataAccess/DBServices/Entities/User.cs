using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DBServices.Entities
{
   public class User
   {

       public int Id { get; set; }
       public string Username { get; set; }
       public string Password { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Position { get; set; }
       public string Email { get; set; }
       public byte[] Photo { get; set; }

    }
}
