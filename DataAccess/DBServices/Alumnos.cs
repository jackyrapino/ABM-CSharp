using Common;
using DataAccess.DBServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace DataAccess.DBServices
{
    public class Alumnos : ConnectionToSql
    {



        public int CreateAlu(Alumno alumno)
        {//Insertar nuevo usuario.
            int result = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    //command.Connection = connection;
                    //command.CommandText = @"INSERT INTO Alumnos (FirstName,LastName,DNI,FechaNacimiento,email,carrera)
                    //                            VALUES ('Alberto', 'Perez','15456598', '10/10/2000','ap@a.com', 'Cientifico');";

                    command.Connection = connection;

                    command.CommandText = @"insert into Alumnos 
                                         values (@FirstName,@LastName, @DNI, @FechaNacimiento,@email,@carrera)";
                    command.Parameters.AddWithValue("@FirstName", alumno.FirstName);
                    command.Parameters.AddWithValue("@LastName", alumno.LastName);
                    command.Parameters.AddWithValue("@DNI", alumno.DNI);
                    command.Parameters.AddWithValue("@FechaNacimiento", alumno.FechaNacimiento);
                    command.Parameters.AddWithValue("@email", alumno.email);
                    command.Parameters.AddWithValue("@carrera", alumno.carrera);


                  
                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery(); //Ejecutar el comando de texto y asignar el resultado al campo result.
                }
            }
            return result;//retornar el numero de filas afectadas de la transaccion. 
        }
        public int ModifyAlu(Alumno alumno)
        {//Actualizar alumno.
            int result = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                   

                    command.CommandText = @"update Alumnos 
                                            set 
                                            FirstName=@FirstName,
                                            LastName=@LastName,
                                            DNI=@DNI,   
                                            FechaNacimiento=@FechaNacimiento,
                                            email=@email,
                                            carrera=@carrera  
	                                        where id=@id";
                    command.Parameters.AddWithValue("@id", alumno.Id);
                    command.Parameters.AddWithValue("@FirstName", alumno.FirstName);
                    command.Parameters.AddWithValue("@LastName", alumno.LastName);
                    command.Parameters.AddWithValue("@DNI", alumno.DNI);
                    command.Parameters.AddWithValue("@FechaNacimiento", alumno.FechaNacimiento);
                    command.Parameters.AddWithValue("@email", alumno.email);
                    command.Parameters.AddWithValue("@carrera", alumno.carrera);


                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }
        public int RemoveAlu(int id)
        {//Eliminar alumno.
            int result = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"delete from Alumnos where id=@Id ";
                    command.Parameters.AddWithValue("@Id", id);

                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }
        public Alumno GetAluById(int id)
        {//Obtener usuario por id.
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select *from Alumnos where id=@Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var aluObj = new Alumno
                        {
                            Id = (int)reader[0],
                            FirstName = reader[1].ToString(),
                            LastName = reader[2].ToString(),
                            DNI = reader[3].ToString(),
                            FechaNacimiento  = reader[4].ToString(),
                            email = reader[5].ToString(),
                            carrera = reader[6].ToString(),
                         
                        };
                        return aluObj; //Retornar resultado (objeto).
                    }
                    else
                        return null;//Retornar NULL si no hay resultado.
                }
            }
        }
        
        
        public IEnumerable<Alumno> GetAllAlu()
        {//Listar alumno.
            var aluList = new List<Alumno>();//Crear lista generica de alumno.

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select *from Alumnos ";
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())//Agregar los resultados en la lista mientras el lector siga leyendo las filas.
                        {
                            var aluObj = new Alumno
                            {
                                Id = (int)reader[0],
                                FirstName = reader[1].ToString(),
                                LastName = reader[2].ToString(),
                                DNI = reader[3].ToString(),
                                FechaNacimiento = reader[4].ToString(),
                                email = reader[5].ToString(),
                                carrera = reader[6].ToString(),
                               
                            };
                            aluList.Add(aluObj);
                        }
                    }
                }
            }
            return aluList; //Retornar lista.
        }
    
    }
}
