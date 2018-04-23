// Automata
//
// Copyright(c) 2018 Guillermo A. Rodríguez
//
// Licencia MIT(https://es.wikipedia.org/wiki/Licencia_MIT)
//
// Se concede permiso, libre de cargos, a cualquier persona que obtenga una copia
// de este software y de los archivos de documentación asociados (el "Software"), 
// para utilizar el Software sin restricción, incluyendo sin limitación los derechos
// a usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar, y/o vender copias
// del Software, y a permitir a las personas a las que se les proporcione el Software a hacer
// lo mismo, sujeto a las siguientes condiciones:
//
// El aviso de copyright anterior y este aviso de permiso se incluirán en todas
// las copias o partes sustanciales del Software.
//
// EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, 
// INCLUYENDO PERO NO LIMITADA A GARANTÍAS DE COMERCIALIZACIÓN, IDONEIDAD PARA UN PROPÓSITO PARTICULAR
// Y NO INFRACCIÓN.EN NINGÚN CASO LOS AUTORES O PROPIETARIOS DE LOS DERECHOS DE AUTOR SERÁN RESPONSABLES
// DE NINGUNA RECLAMACIÓN, DAÑOS U OTRAS RESPONSABILIDADES, YA SEA EN UNA ACCIÓN DE CONTRATO, AGRAVIO
// O CUALQUIER OTRO MOTIVO, DERIVADAS DE, FUERA DE O EN CONEXIÓN CON EL SOFTWARE O SU USO U OTRO TIPO
// DE ACCIONES EN EL SOFTWARE.


using GenericParsing;
using System;
using System.Data;
using System.IO;

namespace Automata
{
    class Tabla
    {
        private const bool TieneEncabezadoPredeterminado = true;

        private GenericParserAdapter m_adaptadorAnalizadorGenerico;
        protected DataTable m_dtTablaDatos;
        private bool m_blTieneEncabezado;

        public DataColumnCollection Columnas
        {
            get
            {
                if (this.m_dtTablaDatos == null)
                    return null;

                return this.m_dtTablaDatos.Columns;
            }
        }

        public DataRowCollection Filas
        {
            get
            {
                if (this.m_dtTablaDatos == null)
                    return null;

                return this.m_dtTablaDatos.Rows;
            }
        }

        public bool TieneEncabezado
        {
            get
            {
                return this.m_blTieneEncabezado;
            }
            set
            {
                this.m_blTieneEncabezado = value;
            }
        }

        public Tabla()
        {
            this._InicializarVariables();
        }

        public Tabla(TextReader txtLector)
        {
            this.EstablecerOrigenDatos(txtLector);
        }

        public Tabla(string strRutaArchivo)
        {
            this.EstablecerOrigenDatos(strRutaArchivo);
        }

        public void EstablecerOrigenDatos(TextReader txtLector)
        {
            if (txtLector == null)
                throw new ArgumentNullException("txtLector", "El TextReader no puede ser nulo.");

            if (this.m_adaptadorAnalizadorGenerico != null)
            {
                this.m_adaptadorAnalizadorGenerico = null;
                this.m_adaptadorAnalizadorGenerico.Dispose();
            }

            if (this.m_dtTablaDatos != null)
            {
                this.m_dtTablaDatos = null;
                this.m_dtTablaDatos.Dispose();
            }

            this.m_adaptadorAnalizadorGenerico = new GenericParserAdapter(txtLector);

            this.m_dtTablaDatos = this.m_adaptadorAnalizadorGenerico.GetDataTable();

            this.m_blTieneEncabezado = true;
        }

        public void EstablecerOrigenDatos(string strRutaArchivo)
        {
            if (!File.Exists(strRutaArchivo))
                throw new FileNotFoundException("El archivo no pudo ser localizado...");

            if (this.m_adaptadorAnalizadorGenerico != null)
            {
                this.m_adaptadorAnalizadorGenerico = null;
                this.m_adaptadorAnalizadorGenerico.Dispose();
            }

            if (this.m_dtTablaDatos != null)
            {
                this.m_dtTablaDatos = null;
                this.m_dtTablaDatos.Dispose();
            }

            this.m_adaptadorAnalizadorGenerico = new GenericParserAdapter(strRutaArchivo);

            this.m_dtTablaDatos = this.m_adaptadorAnalizadorGenerico.GetDataTable();

            this.m_blTieneEncabezado = true;
        }

        public DataTable ObtenerTablaDatos()
        {
            return this.m_dtTablaDatos;
        }

        private void _InicializarVariables()
        {
            this.m_adaptadorAnalizadorGenerico = null;
            this.m_dtTablaDatos = null;
            this.m_blTieneEncabezado = Tabla.TieneEncabezadoPredeterminado;
        }
        
    }
}
