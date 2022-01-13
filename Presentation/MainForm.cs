using Common;
using Domain;
using Presentation.ChildForms;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class MainForm : BaseForms.BaseMainForm
    {
        /// <summary>
        /// Esta clase hereda de clase BaseMainForm.
        /// </summary>

        #region -> Definición de Campos

        private DragControl dragControl;//Permite arrastrar el formulario.       
        private List<Form> listChildForms; //Obtiene o establece los formularios secundarios abiertos en el panel escritorio del formualario.
        private Form activeChildForm;//Obtiene o establece el formulario secundario mostrado actualmente.
        #endregion

        #region -> Constructores

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            dragControl = new DragControl(panelTitleBar, this);
            listChildForms = new List<Form>();
            linkProfile.Visible = false;
        }
        public MainForm(UserModel userModel)
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            dragControl = new DragControl(panelTitleBar, this);
            listChildForms = new List<Form>();
            LoadUserData(userModel);
        }
        #endregion

        #region -> Definición de Métodos

        public void LoadUserData(UserModel userModel)
        {
            //Cargar los datos del usuario conectado en el menú lateral.
            lblName.Text = userModel.FirstName;
            lblLastName.Text = userModel.LastName;
            lblPosition.Text = userModel.Position;
            if (userModel.Photo != null)
                pictureBoxPhoto.Image = Utils.ItemConverter.BinaryToImage(userModel.Photo);
            else pictureBoxPhoto.Image = Properties.Resources.DefaultUserProfile;
        }

        private void ManagePermissions()
        {//Administrar los permisos de usuario, esto es simplemente una demostración,
            //Puedes eliminarlo si no lo necesitas.
            switch (ActiveUser.Position)
            {
                case Positions.Administrador:
                    btnAlumnos.Enabled = false;
                    break;
                case Positions.Bedel:
                    btnUsers.Enabled = false;
                    break;
                case Positions.Profesor:
                    btnUsers.Enabled = false;
                    btnAlumnos.Enabled = true;  
                    break;
            }
        }
        private void Security()
        {//Puede hacer lo mismo en cualquier formulario que te parezca necesario.
            var userModel = new UserModel();
            if (userModel.ValidateActiveUser() == false)//Si el usuario no se ha autenticado, Cerrar la aplicacion.
            {
                MessageBox.Show("Error Fatal, se detectó que está intentando acceder al sistema sin credenciales, por favor inicie sesion e indetifiquese");
                Application.Exit();
            }
            //Opcional, muchas veces en las aplicaciones de escritorio no es necesario.
        }

        private void OpenChildForm<ChildForm>(object senderMenuButton) where ChildForm : Form, new()
        {
            Button menuButton = (Button)senderMenuButton;//Boton de donde se abre el formulario, puedes enviar un valor nulo, si no estas tratando de abrir un formulario desde un control diferente de los botones del menu lateral.
            Form form = listChildForms.OfType<ChildForm>().FirstOrDefault();//Buscar si el formulario ya está instanciado o se ha mostrado anteriormente.

            if (activeChildForm != null && form == activeChildForm)//Si el formulario es igual al formulario  actual activo, retornar ( no hacer nada).
                return;

            if (form == null)//Si el formulario no existe, entonces crear la instancia y mostrarla en el panel escritorio.
            {

                form = new ChildForm();// Instanciar formulario.   
                form.FormBorderStyle = FormBorderStyle.None;//Quitar el borde.
                form.TopLevel = false;//Indicar que el formulario no es de nivel superior  
                form.Dock = DockStyle.Fill; //Establecer el estilo de muelle en lleno (Rellenar el panel escritorio)          
                listChildForms.Add(form);//Agregar formulario secundario a la lista de formularios.

                if (menuButton != null)//Si el boton de menu es diferente a nulo:
                {
                    ActivateButton(menuButton);//Activar/Resaltar el boton.
                    form.FormClosed += (s, e) =>
                    {//Cuando del formulario se cierre, desactivar el boton.
                        DeactivateButton(menuButton);
                    };
                }
                btnChildFormClose.Visible = true;//Mostrar el boton de cerrar formulario secundario.
            }
            CleanDesktop();//Eliminar el formulario secundario actual del panel escritorio
            panelDesktop.Controls.Add(form);//agregar formulario secundario al panel del escritorio
            panelDesktop.Tag = form;// Almacenar el formulario
            form.Show();//Mostrar el formulario 
            form.BringToFront();// Traer al frente
            form.Focus();//Enfocar el formulario
            lblCaption.Text = form.Text;//Establecer el titulo del formulario.
            activeChildForm = form;//Establecer como formulario activo.
        }

        private void CloseChildForm()
        {//Cerrar formulario secundario activo.

            if (activeChildForm != null)
            {
                listChildForms.Remove(activeChildForm);//Eliminar de la lista de formularios.
                panelDesktop.Controls.Remove(activeChildForm);//Eliminar del panel escritorio.
                activeChildForm.Close();//Cerrar el formulario.
                RefreshDesktop();//Actualizar el escritorio (Mostrar el formulario anterior si es el caso, caso contrario restablecer el formulario principal)
            }
        }
        private void CleanDesktop()
        {//Limpiar el escritorio.

            if (activeChildForm != null)
            {
                activeChildForm.Hide();
                panelDesktop.Controls.Remove(activeChildForm);
            }
            /* Este método oculta y elimina el formulario secundario activo del panel escritorio,
            * por lo que solo habrá un formulario secundario abierto dentro del panel del escritorio,
            * ya que agregar un formulario nuevo elimina el formulario anterior y agrega el nuevo
            * formulario (Revise el método OpenChildForm).
            * Los formularios secundarios inactivos se almacenan en una lista genérica.

            * Creé estos métodos para deshacerme de las dudas ya que muchos piensan (incluido yo mismo) que
            * tener 20 o 50 formularios agregados dentro del panel escritorio afecta el rendimiento,
            * bueno, no me di cuenta de eso. De hecho, es posible agregar 10 mil controles dinámicamente
            * en una formulario mostrada y no hay límite si se agrega desde el constructor del formulario, 
            * para experimentar, agregué 100 mil etiquetas y 10 mil paneles con color aunque tardó más
            * de 10 minutos en mostrar (pc : 8 ram, cpu SixCore 3,50 GHz)
             y no hay ningún problema de rendimiento (consumió 20mb ram) y desplazarse por el formulario funciona bien.

            * Por lo tanto, si estos métodos parecen muy tediosos o difíciles de entender, puede utilizar 
            * los métodos para abrir un formulario secundario dentro del panel de Proyectos 
            * anteriores (tutoriales) que se muestran en YouTube, donde los formularios secundarios
            * se almacenan dentro del Panel escritorio, se superponen uno tras otro y se muestra uno (form.bringtofront ();).

            * Sin embargo, todavía no me parece apropiado agregar tantos formularios dentro de un panel 
            * considerando que un formulario predeterminado es de nivel superior y no me gusta la idea
            * de tener tantos controles en un panel (controles de formulario secundario ).

            * Como resultado, preferí almacenar los formularios secundarios en una lista genérica y 
            * agregar y mostrar solo un formulario en el panel escritorio :) */
        }
        private void RefreshDesktop()
        {//Este método es responsable  de actualizar el formulario principal con los parametros adecuados,
            //ya sea restablecer los valores pretederminados o mostrar un formulario secundario anterior si es el caso.
            var childForm = listChildForms.LastOrDefault();//Verificar y obtener el último formulario secundario (anterior) en la lista de formularios
            if (childForm != null)//si hay un formulario secundario instanciado en la lista, agregarlo de nuevo al panel escritorio.
            {
                activeChildForm = childForm;
                panelDesktop.Controls.Add(childForm);
                panelDesktop.Tag = childForm;
                childForm.Show();
                childForm.BringToFront();
                lblCaption.Text = childForm.Text;
            }
            else //Si no hay ningún resultado, no hay ninguna instancia en la lista de formularios secundarios.
            {
                ResetDefaults();//Restablecer el formulario principal a los valores predeterminados
            }
        }
        private void ResetDefaults()
        {
            activeChildForm = null;
            lblCaption.Text = "   Home";
            btnChildFormClose.Visible = false;
        }

        private void ActivateButton(Button menuButton)
        {
            menuButton.ForeColor = Color.FromArgb(0, 100, 182);
            //menuButton.BackColor = panelMenuHeader.BackColor;
        }
        private void DeactivateButton(Button menuButton)
        {
            menuButton.ForeColor = Color.DarkGray;
            //menuButton.BackColor = panelSideMenu.BackColor;
        }


        #endregion

        #region -> Metodos de evento

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetDefaults();
            Security();
            ManagePermissions();
        }

        #region - Cerrar sesión, Cerrar aplicación, minimizar y maximizar.

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de cerrar sesión?", "Mensaje",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();//Cierra el formulario
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.CloseApp();//Cierra la aplicación.
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            this.MaximizeRestore();
            if (this.WindowState == FormWindowState.Maximized)
                btnMaximize.Image = Properties.Resources.btnRestore;
            else btnMaximize.Image = Properties.Resources.btnMaximize;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.Minimize();
        }
        private void btnChildFormClose_Click(object sender, EventArgs e)
        {
            CloseChildForm();
        }
        #endregion

        #region - Convertir foto de perfil a circular

        private void pictureBoxPhoto_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                var rectangle = new Rectangle(0, 0, pictureBoxPhoto.Width - 1, pictureBoxPhoto.Height - 1);
                graphicsPath.AddEllipse(rectangle);
                pictureBoxPhoto.Region = new Region(graphicsPath);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var pen = new Pen(new SolidBrush(pictureBoxPhoto.BackColor), 1);
                e.Graphics.DrawEllipse(pen, rectangle);
            }
        }
        #endregion

        #region - Contraer o Expandir menú lateral

        private void btnSideMenu_Click(object sender, EventArgs e)
        {
            if (panelSideMenu.Width > 100)
            {
                panelSideMenu.Width = 52;
                foreach (Control control in panelMenuHeader.Controls)
                {
                    if (control != btnSideMenu)
                        control.Visible = false;
                }
            }
            else
            {
                panelSideMenu.Width = 230;
                foreach (Control control in panelMenuHeader.Controls)
                {
                    control.Visible = true;
                }
            }
        }
        #endregion

        #region - Abrir formularios secundarios

        private void linkProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenChildForm<FormUserProfile>(null);//Si el control no es boton del menu lateral enviar NULL como parámetro.
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            OpenChildForm<FormUsers>(sender);//Si el control es un boton del menu lateral enviar el objeto remitente.
        }

        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            OpenChildForm<FrmAlumnos>(sender);
        }
     
        #endregion
        #endregion
    }
}
