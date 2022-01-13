using Domain;
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
    public partial class FrmAlumnos : Form
    {

        private AluModel aluModel = new AluModel();
        private List<AluModel> aluList;
        public FrmAlumnos()
        {
            InitializeComponent();
            ListAlumnos();
        }

        private void ListAlumnos()
        {//LLenar la cuadricula de datos con la lista de alumnos.
            aluList = aluModel.GetAllAlu().ToList();//Obtener todos los alumnos
            dataGridView1.DataSource = aluList;//Establecer la fuente de datos.
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount <= 0)
            {
                MessageBox.Show("No hay datos para seleccionar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedCells.Count > 1)
            {
                var AlumnoDM = aluModel.GetAluById((int)dataGridView1.CurrentRow.Cells[0].Value);//Obtener ID del alumno y buscar usando el método GetUser(id).
                if (AlumnoDM != null)
                {
                    var AluForm = new FmrAlumnoMantenimiento(AlumnoDM, false);// Instanciar formulario de mantenimiento y enviar parametros necesarios (modelo de usuario - NO es edicion de datos es de perfil)
                    DialogResult result = AluForm.ShowDialog();//Mostrar formulario como ventana de dialogo y guardar resultado.
                    if (result == System.Windows.Forms.DialogResult.OK)//Si el resultado de dialogo es OK, actualizar vista de datos.
                    {
                        ListAlumnos();
                    }
                }
                else MessageBox.Show("No se ha encontrado usuario", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
                MessageBox.Show("Por favor seleccione una fila", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            var AluForm = new FmrAlumnoMantenimiento();//Instanciar formulario de mantenimiento sin parametros.
            DialogResult result = AluForm.ShowDialog();//Mostrar formulario como ventana de dialogo y guardar resultado.
            if (result == System.Windows.Forms.DialogResult.OK)//Si el resultado de dialogo es OK, actualizar vista de datos.
            {
                ListAlumnos();
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //Eliminar usuario.
            if (dataGridView1.RowCount <= 0)
            {
                MessageBox.Show("No hay datos para seleccionar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1.SelectedCells.Count > 1)
            {
                var result = aluModel.RemoveUser((int)dataGridView1.CurrentRow.Cells[0].Value);//Obtener ID del usuario e invocar el metodo eliminar usuario del modelo.

                if (result > 0)
                {
                    MessageBox.Show("Usuario eliminado con éxito");
                    ListAlumnos();
                }
                else MessageBox.Show("No se ha realizado operación, intente nuevamente");
            }
            else
                MessageBox.Show("Por favor seleccione una fila", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
