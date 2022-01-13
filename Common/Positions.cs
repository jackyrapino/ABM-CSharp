using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Positions
    {
       

        public const string Bedel = "Bedel";
        public const string Profesor = "Profesor";
        public const string Administrador = "Administrador";
        

        public static IEnumerable<string> GetPositions()
        {//Método para listar los cargos.
            var positions = new List<string>();
            positions.Add(Bedel);
            positions.Add(Profesor);
            positions.Add(Administrador);
            positions.Sort();
            return positions;
        }
    }
}
