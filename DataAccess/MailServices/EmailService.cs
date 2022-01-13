using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MailServices
{
    public class EmailService
    {
     

        #region -> Definición de Campos

        private SmtpClient smtpClient;//Obtiene o establece el cliente SMTP
        //Parámetros del correo servidor remitente:
        private readonly string Address = "jackymarquez@hotmail.com";//Establece la dirección del correo servidor
        private readonly string Password = "@302002";//Establece la contraseña del correo servidor
        private readonly string DisplayName = "Jacqueline Marquez";//Establece el nombre para mostrar cuando se envia un mensaje de correo.
        private readonly string Host = "smtp.live.com";//Establece el nombre o la dirección IP del host para las transacciones SMTP.
        private readonly int Port = 465;//Establece el puerto utilizado para las transacciones SMTP.
        private readonly bool SSL = true;//Establece si el cliente SMTP utiliza Secure Sockets Layer (SSL) para cifrar la conexión.

        #endregion

        #region -> Constructor

        public EmailService()
        {
            smtpClient = new SmtpClient();//Inicializar cliente SMTP.
            smtpClient.Credentials = new NetworkCredential(Address, Password);//Establecer las credenciales (Usuario y contraseña).
            smtpClient.Host = Host;//Establecer el host.
            smtpClient.Port = Port;//Establecer el puerto.
            smtpClient.EnableSsl = SSL;//Establecir el cifrado SSL.
        }
        #endregion

        #region -> Métodos

        public void Send(string recipient, string subject, string body)
        {//Este método es responsable de enviar un mensaje de correo a un solo destinatario.

            var mailMessage = new MailMessage();//Inicializar el objeto mensaje de correo.
            var mailSender = new MailAddress(Address, DisplayName);//Inicializar el objeto dirección de correo electrónico remitente.

            try
            {
                mailMessage.From = mailSender;//Establecer la dirección de correo remitente.
                mailMessage.To.Add(recipient);//Establecer y agregar la dirección de correo destinatario.
                mailMessage.Subject = subject;//Establecer el asunto del mensaje de correo.
                mailMessage.Body = body;//Establecer el contenido del mensaje de correo.
                mailMessage.Priority = MailPriority.Normal;//Establecer la prioridad del mensaje de correo.

                smtpClient.Send(mailMessage);//Enviar el mensaje de correo mediante el cliente SMTP(Protocolo simple de transferencia de correo)
            }
            catch (SmtpException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
        public void Send(List<string> recipients, string subject, string body)
        {//Este método es responsable de enviar un mensaje de correo a varios destinatarios.

            var mailMessage = new MailMessage();//Inicializar el objeto mensaje de correo.
            var mailSender = new MailAddress(Address, DisplayName);//Inicializar el objeto dirección de correo electrónico remitente.

            try
            {
                mailMessage.From = mailSender;//Establecer la dirección de correo remitente.
                foreach (var recipientMail in recipients)//Recorrer la lista de destinatarios para obtener cada dirección de correo.
                {
                    mailMessage.To.Add(recipientMail);//Agregar la dirección de correo destinatario a la colección de direcciones de correo.
                }
                mailMessage.Subject = subject;//Establecer el asunto del mensaje de correo.
                mailMessage.Body = body;//Establecer el contenido del mensaje de correo.
                mailMessage.Priority = MailPriority.Normal;//Establecer la prioridad del mensaje de correo.

                smtpClient.Send(mailMessage);//Enviar el mensaje de correo mediante el cliente SMTP(Protocolo simple de transferencia de correo)
            }
            catch (SmtpException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
        #endregion
    }
}
