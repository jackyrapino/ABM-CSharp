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
    public partial class FormVerifyUser : BaseForms.BaseFixedForm
    {
       

        private string userPass;

        public FormVerifyUser(string _userPass)
        {
            InitializeComponent();
            lblMessage.Text = "";
            userPass = _userPass;
        }
        private void VerifyConectedUser()
        {
            if (txtUserPass.Text == userPass)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                lblMessage.Text = "Su contraseña es incorrecto, vuelva a intentarlo";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            VerifyConectedUser();
        }

        private void txtUserPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                VerifyConectedUser();
        }
    }
}
