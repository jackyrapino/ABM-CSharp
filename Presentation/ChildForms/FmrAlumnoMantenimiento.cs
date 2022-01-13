using Common;
using Domain;
using Presentation.Helpers;
using Presentation.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.ChildForms
{
    public partial class FmrAlumnoMantenimiento : BaseForms.BaseFixedForm
    {

        private AluModel aluModel;//Obtiene o establece el modelo de dominio de usuario.
        private bool aluModify; //Obtiene o establece un usuario será editado.
        private int aluId;//Obtiene o establece el id del usuario a editar.
       
        public FmrAlumnoMantenimiento()
        {
            InitializeComponent();
            lblCaption.Text = "Agregar nuevo usuario";
            aluModel = new AluModel();//Inicializar modelo de dominio de usuario.
            aluModify = false;//Establecer userModify en falso.
            
        }

        public FmrAlumnoMantenimiento(AluModel _aluModel, bool isUserProfile)
        {// editar un usuario o actualizar el perfil de usuario.

            InitializeComponent();
            this.TitleBarColor = Color.MediumSeaGreen;
            btnSave.BackColor = Color.MediumSeaGreen;
        

            aluModel = _aluModel;
            aluModify  = true; 
            FillFields();                  
            if (isUserProfile) 
            {
                lblCaption.Text = "Actualizar mi perfil de usuario";
              
            }
            else 
                lblCaption.Text = "Modificar usuario";
        }

        private void FillFields()
        {//Cargar los datos del modelo  en los campos del formulario.
            aluId = aluModel.Id;
            txtFirstName.Text = aluModel.FirstName;
            txtLastName.Text = aluModel.LastName;
            txtEmail.Text = aluModel.email;
            txtDni.Text = aluModel.DNI;
            txtFechaNacimiento.Text = aluModel.FechaNacimiento;
            txtCarrera.Text = aluModel.carrera;
           
        }
        private void FillAluModel()
        {//LLenar modelo

            aluModel.Id = aluId;
            aluModel.FirstName = txtFirstName.Text;
            aluModel.LastName = txtLastName.Text;
            aluModel.email = txtEmail.Text;
            aluModel.DNI = txtDni.Text;
            aluModel.FechaNacimiento = txtFechaNacimiento.Text;
            aluModel.carrera = txtCarrera.Text;
            
        
        }
        private void Save()
        {//Guardar cambios.
            int result = -1;
            try
            {
                FillAluModel();//Obtener modelo de vista.
                //var validateData = new DataValidation(aluModel);//Validar campos del objeto.
                //var validatePassword = txtFechaNacimiento.Text == FmrAlumnoMantenimiento.Text;//Validar contraseñas.

                //if (validateData.Result == true && validatePassword == true)//Si el objeto y contraseña es válido.
                //{
                    //EDITAR USUARIO
                    if (aluModify == true)
                    {
                        result = aluModel.ModifyAlumno();//Invocar metodo ModifyUser del modelo de usuario.
                        if (result > 0)
                        {
                            MessageBox.Show("Usuario actualizado con éxito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;//Establecer Ok como resultado de dialogo del formulario.
                            this.Close();//Cerrar formulario
                        }
                        else
                        {
                            MessageBox.Show("No se realizó la operación, intente nuevamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    //AGREGAR USUARIO
                    else
                    {
                        result = aluModel.CreateAlu(); //Invocar metodo CreateUser del modelo de usuario.

                        if (result > 0)
                        {
                            MessageBox.Show("Usuario agregado con éxito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK; //Establecer Ok como resultado de dialogo del formulario.
                            this.Close();//Cerrar formulario
                        }
                        else
                        {
                            MessageBox.Show("No se realizó la operación, intente nuevamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                //}
                //else //Si el objeto o contraseña NO es válido, mostrar mensaje segun el caso.
                //{
                //    if (validateData.Result == false)
                //        MessageBox.Show(validateData.ErrorMessage, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    else
                //        MessageBox.Show("Las contraseñas no coinciden", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }
            catch (Exception ex)
            {
                var message = ExceptionManager.GetMessage(ex);//Obtener mensaje de excepción.
                MessageBox.Show(message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);//Mostrar mensaje.
            }
        }

        private void FmrAlumnoMantenimiento_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDeletePhoto_Click(object sender, EventArgs e)
        {

        }

        private void btnAddPhoto_Click(object sender, EventArgs e)
        {

        }

        private void txtCarrera_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
