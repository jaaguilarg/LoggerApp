using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerApp.Entities
{
    [Serializable]
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class SqlParametros : System.Attribute
    {
        public bool IsParameter { get; set; }
        public SqlParametros(bool IsParameter)
        {
            this.IsParameter = IsParameter;
        }
    }

    [Serializable]
    public class DataBasetAttributes : System.Attribute
    {
        public bool Save { get; set; }

        public string ParameterName { get; set; }

        public string ColumnName { get; set; }
    }

    /// <summary>
    /// Metadatos que permiten identificar si una propiedad se debe leer o 
    /// escribir al momento de utilizar reflexión en el metodo
    /// </summary>
    [Serializable]
    public class Reflexion : System.Attribute
    {
        /// <summary>
        /// Indica si la propiedad debe o no escribirse
        /// </summary>
        public bool Writable { get; set; }

        /// <summary>
        /// Indica si una propiedad debe o no leerse
        /// </summary>
        public bool Readable { get; set; }
    }
}
