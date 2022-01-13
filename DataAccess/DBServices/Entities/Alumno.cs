using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DBServices.Entities
{
   public  class Alumno
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string FechaNacimiento { get; set; }
        public string email { get; set; }
        public string carrera { get; set; }
        public byte[] profilePicture { get; set; }
    }
}
