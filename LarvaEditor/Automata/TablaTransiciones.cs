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


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata
{
    class TablaTransiciones : Tabla
    {
        private bool m_blTablaSimbolosConjugados;
        private EstadoTransicion m_estadoTransicionActual;

        private int m_intIndiceEstadoActual;
        private int m_intindiceSimboloActual;

        private string m_strEstadoActual;
        private string m_strSimboloActual;


        public bool TablaSimbolosConjugados
        {
            get { return m_blTablaSimbolosConjugados; }
        }

        public EstadoTransicion EstadoTransicionActual
        {
            get { return m_estadoTransicionActual; }
        }

        public int CantidadSimbolos
        {
            get
            {
                if (this.m_dtTablaDatos == null)
                    throw new InvalidOperationException("No se ha establecido datos a la tabla...");

                return this.m_dtTablaDatos.Columns.Count - 3;
            }
        }

        public int CantidadEstados
        {
            get
            {
                if (this.m_dtTablaDatos == null)
                    throw new InvalidOperationException("No se ha establecido datos a la tabla...");

                return this.m_dtTablaDatos.Rows.Count - 3;
            }
        }

        public TablaTransiciones() : base()
        {
            this.m_estadoTransicionActual = EstadoTransicion.SinDatos;
            this.m_intIndiceEstadoActual = 0;
            this.m_intindiceSimboloActual = 0;
            this.m_strEstadoActual = null;
            this.m_strSimboloActual = null;
        }

        public TablaTransiciones(TextReader txtLector) : base()
        {
            this.EstablecerOrigenDatos(txtLector);

            if (this.m_dtTablaDatos != null)
            {
                this.m_estadoTransicionActual = EstadoTransicion.Listo;
                this.m_blTablaSimbolosConjugados = this._TablaDeSimbolosConjugados();
            }
        }

        public TablaTransiciones(string strRutaArchivo) : base()
        {
            this.EstablecerOrigenDatos(strRutaArchivo);

            if (this.m_dtTablaDatos != null)
            {
                this.m_estadoTransicionActual = EstadoTransicion.Listo;
                this.m_blTablaSimbolosConjugados = this._TablaDeSimbolosConjugados();
            }
        }

        public bool EstablecerCadena(string strCadena)
        {
            if (this.m_estadoTransicionActual == EstadoTransicion.SinDatos)
                throw new InvalidOperationException("No se ha establecido datos a la tabla...");
            if (this.m_estadoTransicionActual == EstadoTransicion.Analizando)
                throw new InvalidOperationException("La tabla de transiciones ya empezo su recorrido, espere que termine...");

            if (this.m_estadoTransicionActual == EstadoTransicion.Terminado)
                this.m_estadoTransicionActual = EstadoTransicion.Listo;

            if (this.m_blTablaSimbolosConjugados)
                return _AnalizarSimboloConjugado(strCadena);
            else
                return _AnalizarSimbolo(strCadena);
        }

        public string ObtenerToken()
        {
            if (this.m_estadoTransicionActual == EstadoTransicion.SinDatos)
                throw new InvalidOperationException("No se ha establecido datos a la tabla...");
            if (this.m_estadoTransicionActual == EstadoTransicion.Listo)
                throw new InvalidOperationException("No se ha ingresado una cadena que analizar...");
            if (this.m_estadoTransicionActual == EstadoTransicion.Analizando)
                throw new InvalidOperationException("La tabla de transiciones ya empezo su recorrido, espere que termine...");

            if (this.m_strEstadoActual == "Error")
                return this.m_strEstadoActual;
            
            return this.m_dtTablaDatos.Rows[this.m_intIndiceEstadoActual][CantidadSimbolos + 2].ToString();
        }

        private bool _TablaDeSimbolosConjugados()
        {
            if (this.m_estadoTransicionActual == EstadoTransicion.SinDatos)
                throw new InvalidOperationException("No se ha establecido datos a la tabla...");

            for (int i = 1; i <= CantidadSimbolos; i++)
                if (this.m_dtTablaDatos.Columns[i].ColumnName.Length > 1)
                    return true;
            
            return false;
        }

        private int _ObtenerIndiceEstado(string strEstado)
        {
            int intIndiceEstado = 0;

            for (int i = 0; i < CantidadEstados; i++)
            {
                if (this.m_dtTablaDatos.Rows[i][0].ToString() == strEstado)
                    break;

                intIndiceEstado++;
            }

            if (intIndiceEstado == CantidadEstados)
                return 0;

            return intIndiceEstado;
        }

        private bool _AnalizarSimbolo(string strCadena)
        {
            this.m_intIndiceEstadoActual = 0;
            char[] caCaracteres = strCadena.ToArray();
            this.m_estadoTransicionActual = EstadoTransicion.Analizando;

            for (int c = 0; c < caCaracteres.Length; c++)
            {
                for (this.m_intindiceSimboloActual = 1; this.m_intindiceSimboloActual <= CantidadSimbolos; this.m_intindiceSimboloActual++)
                {
                    if (this.m_dtTablaDatos.Columns[this.m_intindiceSimboloActual + 1].ColumnName == caCaracteres[c].ToString())
                    {
                        this.m_strEstadoActual = this.m_dtTablaDatos.Rows[this.m_intIndiceEstadoActual][this.m_intindiceSimboloActual].ToString();

                        if (this.m_strEstadoActual.ToLower() == "error")
                        {
                            this.m_estadoTransicionActual = EstadoTransicion.Terminado;
                            return false;
                        }

                        this.m_intIndiceEstadoActual = _ObtenerIndiceEstado(this.m_strEstadoActual);
                        break;
                    }
                }
            }

            this.m_estadoTransicionActual = EstadoTransicion.Terminado;
            return true;
        }

        private bool _AnalizarSimboloConjugado(string strCadena)
        {
            this.m_intIndiceEstadoActual = 0;
            this.m_estadoTransicionActual = EstadoTransicion.Analizando;

            for (this.m_intindiceSimboloActual = 1; this.m_intindiceSimboloActual <= CantidadSimbolos; this.m_intindiceSimboloActual++)
            {
                if (this.m_dtTablaDatos.Columns[this.m_intindiceSimboloActual + 1].ColumnName == strCadena)
                {
                    this.m_strEstadoActual = this.m_dtTablaDatos.Rows[this.m_intIndiceEstadoActual][this.m_intindiceSimboloActual].ToString();

                    if (this.m_strEstadoActual.ToLower() == "error")
                    {
                        this.m_estadoTransicionActual = EstadoTransicion.Terminado;
                        return false;
                    }

                    this.m_intIndiceEstadoActual = _ObtenerIndiceEstado(this.m_strEstadoActual);
                    break;
                }
            }

            this.m_estadoTransicionActual = EstadoTransicion.Terminado;
            return true;
        }
    }
}
