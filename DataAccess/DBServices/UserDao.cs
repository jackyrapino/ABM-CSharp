using Common;
using DataAccess.DBServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DBServices
{
    public class UserDao : ConnectionToSql
    {
       
        public User Login(string username, string password)
        {//Validar el usuario y contraseña del usuario para el inicio de sesion.

            using (var connection = GetConnection()) //Obtener la conexion.
            {
                connection.Open(); //Abrir la conexion.
                using (var command = new SqlCommand())//Crear objeto SqlCommand.
                {
                    command.Connection = connection;//Establecer la conexión.
                    //Establecer el comando de texto.
                    command.CommandText = "select *from Users where (userName=@username and password=@pass) or (Email=@username and password=@pass)";
                    //Establecer los parametros.
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@pass", password);
                    command.CommandType = CommandType.Text;//Establecer el tipo de comando.

                    SqlDataReader reader = command.ExecuteReader();//Ejecutar y establecer el lector.
                    if (reader.Read())//Si el lector tiene filas que leer.
                    {
                        var userObj = new User //Crear objeto y asignar los datos del resultado.
                        {
                            Id = (int)reader[0], //Posicion 0
                            Username = reader[1].ToString(),
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Position = reader[5].ToString(),
                            Email = reader[6].ToString(),
    
                            Photo = reader[7] != DBNull.Value ? (byte[])reader[7] : null 
                           
                        };
                        //Asignar los datos del usuario conectado actualmente en la aplicacion.
                        ActiveUser.Id = userObj.Id;                      
                        ActiveUser.Position = userObj.Position;
                        ActiveUser.Username = username;

                        return userObj; //Retornar el objeto usuario (Resultado)
                    }
                    else //Si la consulta no fue exitosa, retornar nulo.
                        return null;
                }
            }
        }
        public bool ValidateActiveUser()
        {//Seguridad de la aplicacion, utiliza este metodo para verificar que el usuario conectado sea valido.
            bool validUser = false;//Obtiene o establece si el usuario conectado es valido (Valor por defecto =falso).
            string activeUserPass = "";//Obtiene o establece la contraseña del usuario conectado.
            if (string.IsNullOrWhiteSpace(ActiveUser.Username) == false) //Ejecutar este fragmento de codigo siempre en cuando que el nombre usuario NO sea nulo o espacios en blanco.
            {
                using (var connection = GetConnection())//Obtener conexion
                {
                    connection.Open();
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        //Obtener la contraseña del usuario conectado.
                        command.CommandText = "select password from Users where id=@id";//Establecer el comando de texto
                        command.Parameters.AddWithValue("@id", ActiveUser.Id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())//Si el lector tiene filas que leer, almacenar el resultado (Contraseña) en el campo activeUserPass.
                                activeUserPass = reader[0].ToString();
                            command.Parameters.Clear();//Limpiar los parametros para la siguiente consulta.
                        }
                        //Validar usuario conectado.
                        command.CommandText = "select *from Users where userName=@username or Email=@username and Password=@pass and Id=@id";
                        command.Parameters.AddWithValue("@username", ActiveUser.Username);
                        command.Parameters.AddWithValue("@pass", activeUserPass);
                        command.Parameters.AddWithValue("@id", ActiveUser.Id);

                        command.CommandType = CommandType.Text;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) //Si el lector tiene filas, establecer validUser en verdadero.
                                validUser = true;
                            else //Caso contrario, establecer validUser en falso.
                                validUser = false;
                        }
                    }
                }
            }
            return validUser; //Retornar el resultado.
        }
        public int CreateUser(User user)
        {//Insertar nuevo usuario.
            int result = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"insert into Users 
	                                        values (@userName,@password, @firstName, @lastName,@position,@email,@photo)";
                    command.Parameters.AddWithValue("@userName", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@position", user.Position);
                    command.Parameters.AddWithValue("@email", user.Email);
                    if (user.Photo != null)//Si la propiedad Foto es diferente a nulo, asignar el valor de la propiedad.
                        command.Parameters.Add(new SqlParameter("@photo", user.Photo) { SqlDbType = SqlDbType.VarBinary });
                    else //Caso contrario asignar un valor nulo de SQL.  
                        command.Parameters.Add(new SqlParameter("@photo", DBNull.Value) { SqlDbType = SqlDbType.VarBinary });
                    
                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery(); //Ejecutar el comando de texto y asignar el resultado al campo result.
                }
            }
            return result;//retornar el numero de filas afectadas de la transaccion. 
        }
        public int ModifyUser(User user)
        {//Actualizar usuario.
            int result = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"update  Users	
	                                        set userName=@userName,password=@password,firstName=@firstName,lastName= @lastName,position= @position,email=@email, profilePicture=@photo  
	                                        where id=@id ";
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@userName", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@position", user.Position);
                    command.Parameters.AddWithValue("@email", user.Email);
                    if (user.Photo != null)
                        command.Parameters.Add(new SqlParameter("@photo", user.Photo) { SqlDbType = SqlDbType.VarBinary });
                    else
                        command.Parameters.Add(new SqlParameter("@photo", DBNull.Value) { SqlDbType = SqlDbType.VarBinary });

                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }
        public int RemoveUser(int id)
        {//Eliminar usuario.
            int result = -1;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"delete from Users where id=@id ";
                    command.Parameters.AddWithValue("@id", id);
                
                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }
        public User GetUserById(int id)
        {//Obtener usuario por id.
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select *from Users where id=@id";
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var userObj = new User
                        {
                            Id = (int)reader[0],
                            Username = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Position = reader[5].ToString(),
                            Email = reader[6].ToString(),
                            Photo = reader[7] != DBNull.Value ? (byte[])reader[7] : null
                        };
                        return userObj; //Retornar resultado (objeto).
                    }
                    else
                        return null;//Retornar NULL si no hay resultado.
                }
            }
        }
        public User GetUserByUsername(string user)
        {//Obtener usuario por nombre de usuario o email.
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select *from Users where username=@user or email=@user";
                    command.Parameters.AddWithValue("@user", user);
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var userObj = new User
                        {
                            Id = (int)reader[0],
                            Username = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Position = reader[5].ToString(),
                            Email = reader[6].ToString(),
                            Photo = reader[7] != DBNull.Value ? (byte[])reader[7] : null
                        };
                        return userObj;
                    }
                    else
                        return null;
                }
            }
        }
        public IEnumerable<User> GetAllUsers()
        {//Listar usuarios.
            var userList = new List<User>();//Crear lista generica de usuarios.

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select *from Users ";
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())//Agregar los resultados en la lista mientras el lector siga leyendo las filas.
                        {
                            var userObj = new User
                            {
                                Id = (int)reader[0],
                                Username = reader[1].ToString(),
                                Password = reader[2].ToString(),
                                FirstName = reader[3].ToString(),
                                LastName = reader[4].ToString(),
                                Position = reader[5].ToString(),
                                Email = reader[6].ToString(),
                                Photo = reader[7] != DBNull.Value ? (byte[])reader[7] : null
                            };
                            userList.Add(userObj);
                        }
                    }
                }
            }
            return userList; //Retornar lista.
        }
    }
}
