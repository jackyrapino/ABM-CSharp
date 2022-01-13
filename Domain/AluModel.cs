using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DBServices;
using DataAccess.DBServices.Entities;
using DataAccess.MailServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class AluModel
    {

        #region -> Atributos

        private int _id;
        private string _firstName;
        private string _lastName;
        private string _dni;
        private string _FechaNacimiento;
        private string _email;
        private string _carrera;
        private Alumnos _alumnos;
        #endregion

        #region -> Constructores

        public AluModel()
        {
            _alumnos = new Alumnos();
        }

        public AluModel(int id, string firstName, string lastName, string dni, string fechaNacimiento, string email, string carrera, byte[] profilePicture)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DNI = dni;
            FechaNacimiento = fechaNacimiento;
            _email = email;
            _carrera = carrera;
         

            _alumnos = new Alumnos();
        }
        #endregion

        #region -> Propiedades + Validacíon y Visualización de Datos

        //Posición 0 
        [DisplayName("Num")]//Nombre a visualizar (Por ejemplo en la columna del datagridview se mostrará como Num).
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //Posición 1 
        [DisplayName("Nombre")]//Nombre a visualizar
        [Required(ErrorMessage = "Por favor ingrese nombre")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "El nombre debe ser solo letras")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe contener un mínimo de 3 caracteres.")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        ////Posición 2
        [DisplayName("Apellido")]//Nombre a visualizar.
        [Required(ErrorMessage = "Por favor ingrese apellido.")]//Validaciones
        [RegularExpression("[0-9]{7,8}", ErrorMessage = "El Dni debe estar compuesto solo de numeros ")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        //Posición 3
        [DisplayName("DNI")]//Nombre a visualizar.
      
        [Required(ErrorMessage = "Por favor ingrese DNI.")]//Validaciones
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "El apellido debe ser solo letras")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe contener un mínimo de 3 caracteres.")]
        public string DNI
        {
            get { return _dni; }
            set { _dni = value; }
        }

        //Posición 4
        [DisplayName("Fecha de nacimiento")]//Nombre a visualizar.
        [Required(ErrorMessage = "Por favor ingrese su fecha de nacimiento.")]//Validaciones
        [RegularExpression("^([0-2][0-9]|3[0-1])(//|-)(0[1-9]|1[0-2])/2(/d{4})$", ErrorMessage = "su fecha de nacimiento debe estar expresada dd/mm/aaaa")]
       public string FechaNacimiento
        {
            get { return _FechaNacimiento; }
            set { _FechaNacimiento = value; }
        }

        //Posición 5
        [DisplayName("Email")]//Nombre a visualizar.
        [Required(ErrorMessage = "Por favor ingrese correo electrónico.")]//Validaciones
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido: example@gmail.com")]
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        //Posición 6
        [DisplayName("Carrera")]//Nombre a visualizar.
        [Browsable(false)]//Ocultar visualización
        [Required(ErrorMessage = "Por favor ingrese nombre de la carrera")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "El nombre debe ser solo letras")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe contener un mínimo de 5 caracteres.")]
        public string carrera
        {
            get { return _carrera; }
            set { _carrera = value; }
        }

   
      
        #endregion

        #region -> Métodos Públicos

       

        public int CreateAlu()
        {//Agregar nuevo usuario.
            
            Alumno alumnoEntity = MapAlumnoEntity(this);
            return _alumnos.CreateAlu(alumnoEntity);
        }
        public int ModifyAlumno()
        {//Actualizar usuario.

            Alumno alumnosEntity = MapAlumnoEntity(this);
            return _alumnos.ModifyAlu(alumnosEntity);
        }
        public int RemoveUser(int id)
        {//Eliminar usuario.

            return _alumnos.RemoveAlu(id);
        }
        public AluModel GetAluById(int id)
        {//Obtener usuario por ID.
            var result = _alumnos.GetAluById(id);
            if (result != null)
                return MapAluModel(result);
            else
                return null;
        }
        public IEnumerable<AluModel> GetAllAlu()
        {//Listar todos los usuarios.


            var result = _alumnos.GetAllAlu();
            
            return MapAluModel(result);
        }
       
        #endregion

        #region -> Métodos Privados (Mapear datos)

        //Mapear modelo de dominio a modelo de entidad.
        private Alumno MapAlumnoEntity(AluModel aluModel)
        {
            var aluEntity = new Alumno
            {
                Id = aluModel.Id,
                FirstName = aluModel.FirstName,
                LastName = aluModel.LastName,
                DNI = aluModel.DNI,
                FechaNacimiento = aluModel.FechaNacimiento,
                email = aluModel.email,
                carrera  = aluModel.carrera 
            };
            return aluEntity;
        }

        //Mapear modelo entidad a modelo de dominio.
        private AluModel MapAluModel(Alumno aluEntity)
        {//Mapear un solo objeto.
            var aluModel = new AluModel()
            {
                Id = aluEntity.Id,
                FirstName = aluEntity.FirstName,
                LastName = aluEntity.LastName,
                DNI = aluEntity.DNI,
                FechaNacimiento = aluEntity.FechaNacimiento,
                email = aluEntity.email,
                carrera = aluEntity.carrera
            };
            return aluModel;
        }
        private IEnumerable<AluModel> MapAluModel(IEnumerable<Alumno> alumnoEntities)
        {//Mapear colección de objetos.
            var alumnoModels = new List<AluModel>();
            foreach (var alumno in alumnoEntities)
            {
                alumnoModels.Add(MapAluModel(alumno));
            }
            return alumnoModels;
        }

        #endregion

    }
}
