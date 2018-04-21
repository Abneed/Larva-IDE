// Larva IDE
// Copyright(c) Guillermo A.Rodríguez.
//
// Se concede permiso, de forma gratuita, a cualquier persona que obtenga una copia
// de este software y archivos de documentación asociados (el "Software"), para tratar 
// el Software sin restricciones, incluidos, entre otros, los derechos de 
// uso, copia, modificación, fusión, publicación, distribución, sublicencia y / o venta de copias 
// del Software y para permitir a las personas a quienes se les proporciona el Software que lo hagan, 
// sujeto a las siguientes condiciones:
//
// El aviso de copyright anterior y este aviso de permiso se incluirán en todos
// copias o porciones sustanciales del software.
//
// EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, 
// INCLUIDAS, ENTRE OTRAS, LAS GARANTÍAS DE COMERCIABILIDAD, IDONEIDAD PARA UN PROPÓSITO EN PARTICULAR 
// Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O PROPIETARIOS DE DERECHOS DE AUTOR SERÁN RESPONSABLES 
// DE NINGÚN RECLAMO, DAÑO O DEMÁS RESPONSABILIDADES, YA SEA EN UNA ACCIÓN CONTRACTUAL, EXTRACONTRACTUAL 
// O DE OTRO TIPO, DERIVADOS, FUERA DEL USO DEL SOFTWARE O DEL USO O DE OTROS TRATADOS EN EL SOFTWARE.


using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace LarvaEditor
{
    public partial class frmLarvaTextEditor : Form
    {
        public frmLarvaTextEditor()
        {
            InitializeComponent();
        }

        private void frmLarvaTextEditor_Load(object sender, EventArgs e)
        {
            AgregarPestana();
            ObtenerColeccionFuentes();
            TamanosFuente();
        }

        private int ConteoPestanas = 0;

        #region Propiedades

        private RichTextBox ObtenerDocumentoActual
        {
            get { return (RichTextBox)tabControlContainerTextEditor.SelectedTab.Controls["Cuerpo"]; }
        }

        #endregion

        #region Metodos 

        #region Pestanas 

        private void AgregarPestana()
        {
            RichTextBox Cuerpo = new RichTextBox();

            Cuerpo.Name = "Cuerpo";
            Cuerpo.Dock = DockStyle.Fill;
            Cuerpo.ContextMenuStrip = contextMenuStripTextEditor;

            TabPage NuevaPagina = new TabPage();
            ConteoPestanas++;

            string TextoDocumento = "Documento " + ConteoPestanas;
            NuevaPagina.Name = TextoDocumento;
            NuevaPagina.Text = TextoDocumento;
            NuevaPagina.Controls.Add(Cuerpo);

            tabControlContainerTextEditor.TabPages.Add(NuevaPagina);
        }

        private void EliminarPestana()
        {
            if (tabControlContainerTextEditor.TabPages.Count != 1)
            {
                tabControlContainerTextEditor.TabPages.Remove(tabControlContainerTextEditor.SelectedTab);
            }
            else
            {
                tabControlContainerTextEditor.TabPages.Remove(tabControlContainerTextEditor.SelectedTab);
                AgregarPestana();
            }
        }

        private void EliminarTodasLasPestanas()
        {
            foreach (TabPage Page in tabControlContainerTextEditor.TabPages)
            {
                tabControlContainerTextEditor.TabPages.Remove(Page);
            }

            AgregarPestana();
        }

        private void EliminarTodasLasPestanasMenosEsta()
        {
            foreach (TabPage Page in tabControlContainerTextEditor.TabPages)
            {
                if (Page.Name != tabControlContainerTextEditor.SelectedTab.Name)
                {
                    tabControlContainerTextEditor.TabPages.Remove(Page);
                }
            }
        }

        #endregion Pestanas

        #region GuardaryAbrir 

        private void Guardar()
        {
            saveFileDialogTextEditor.FileName = tabControlContainerTextEditor.SelectedTab.Name;
            saveFileDialogTextEditor.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialogTextEditor.Filter = "Text Files|*.txt";
            saveFileDialogTextEditor.Title = "Guardar";

            if (saveFileDialogTextEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialogTextEditor.FileName.Length > 0)
                {
                    try
                    {
                        ObtenerDocumentoActual.SaveFile(saveFileDialogTextEditor.FileName, RichTextBoxStreamType.RichText);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        private void GuardarComo()
        {
            saveFileDialogTextEditor.FileName = tabControlContainerTextEditor.SelectedTab.Name;
            saveFileDialogTextEditor.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialogTextEditor.Filter = "Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";
            saveFileDialogTextEditor.Title = "Guardar como";

            if (saveFileDialogTextEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialogTextEditor.FileName.Length > 0)
                {
                    try
                    { 
                        ObtenerDocumentoActual.SaveFile(saveFileDialogTextEditor.FileName, RichTextBoxStreamType.PlainText);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        private void Abrir()
        {
            openFileDialogTextEditor.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialogTextEditor.Filter = "Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.* ";

            if (openFileDialogTextEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialogTextEditor.FileName.Length > 0)
                {
                    try
                    {
                        ObtenerDocumentoActual.LoadFile(openFileDialogTextEditor.FileName, RichTextBoxStreamType.PlainText);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }

        }

        #endregion GuardaryAbrir

        #region Texto

        private void Deshacer()
        {
            ObtenerDocumentoActual.Undo();
        }

        private void Rehacer()
        {
            ObtenerDocumentoActual.Redo();
        }

        private void Cortar()
        {
            ObtenerDocumentoActual.Cut();
        }

        private void Copiar()
        {
            ObtenerDocumentoActual.Copy();
        }

        private void Pegar()
        {
            ObtenerDocumentoActual.Paste();
        }

        private void SeleccionarTodo()
        {
            ObtenerDocumentoActual.SelectAll();
        }

        #endregion  Texto

        #region General

        private void ObtenerColeccionFuentes()
        {
            InstalledFontCollection FuenteIns = new InstalledFontCollection();

            foreach (FontFamily item in FuenteIns.Families)
            {
                toolStripComboBoxFontFamily.Items.Add(item.Name);
            }
            toolStripComboBoxFontFamily.SelectedIndex = 2;
        }

        private void TamanosFuente()
        {
            for (int i = 1; i <+ 75; i++)
            {
                toolStripComboBoxFontSize.Items.Add(i);
            }
            toolStripComboBoxFontSize.SelectedIndex = 15;
        }


        #endregion

        #endregion Metodos

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deshacer();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rehacer();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cortar();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copiar();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pegar();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliminarPestana();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliminarTodasLasPestanas();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliminarTodasLasPestanasMenosEsta();
        }

        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            Font FuenteNegrita = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Bold);
            Font FuenteRegular = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (ObtenerDocumentoActual.SelectionFont.Bold)
            {
                ObtenerDocumentoActual.SelectionFont = FuenteRegular;
            }
            else
            {
                ObtenerDocumentoActual.SelectionFont = FuenteNegrita;
            }
        }

        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            Font FuenteItalico = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Italic);
            Font FuenteRegular = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (ObtenerDocumentoActual.SelectionFont.Italic)
            {
                ObtenerDocumentoActual.SelectionFont = FuenteRegular;
            }
            else
            {
                ObtenerDocumentoActual.SelectionFont = FuenteItalico;
            }
        }

        private void toolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            Font FuenteSubrayado = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Underline);
            Font FuenteRegular = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (ObtenerDocumentoActual.SelectionFont.Underline)
            {
                ObtenerDocumentoActual.SelectionFont = FuenteRegular;
            }
            else
            {
                ObtenerDocumentoActual.SelectionFont = FuenteSubrayado;
            }
        }

        private void toolStripButtonStrikeout_Click(object sender, EventArgs e)
        {
            Font FuenteTachada = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Strikeout);
            Font FuenteRegular = new Font(ObtenerDocumentoActual.SelectionFont.FontFamily,
                ObtenerDocumentoActual.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (ObtenerDocumentoActual.SelectionFont.Strikeout)
            {
                ObtenerDocumentoActual.SelectionFont = FuenteRegular;
            }
            else
            {
                ObtenerDocumentoActual.SelectionFont = FuenteTachada;
            }
        }

        private void toolStripButtonUpperCase_Click(object sender, EventArgs e)
        {
            ObtenerDocumentoActual.SelectedText = ObtenerDocumentoActual.SelectedText.ToUpper();
        }

        private void toolStripButtonLowerCase_Click(object sender, EventArgs e)
        {
            ObtenerDocumentoActual.SelectedText = ObtenerDocumentoActual.SelectedText.ToLower();
        }

        private void toolStripButtonFontIncrease_Click(object sender, EventArgs e)
        {
            float NuevoFuenteTamano = ObtenerDocumentoActual.SelectionFont.SizeInPoints + 2;

            Font NuevoTamano = new Font(ObtenerDocumentoActual.SelectionFont.Name, NuevoFuenteTamano,
                ObtenerDocumentoActual.SelectionFont.Style);

            ObtenerDocumentoActual.SelectionFont = NuevoTamano;
        }

        private void toolStripButtonFontDecrease_Click(object sender, EventArgs e)
        {
            float NuevoFuenteTamano = ObtenerDocumentoActual.SelectionFont.SizeInPoints - 2;

            Font NuevoTamano = new Font(ObtenerDocumentoActual.SelectionFont.Name, NuevoFuenteTamano,
                ObtenerDocumentoActual.SelectionFont.Style);

            ObtenerDocumentoActual.SelectionFont = NuevoTamano;
        }

        private void toolStripButtonForeColor_Click(object sender, EventArgs e)
        {
            if (colorDialogTextEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ObtenerDocumentoActual.SelectionColor = colorDialogTextEditor.Color;
            }
        }

        private void HighlightGreen_Click(object sender, EventArgs e)
        {
            ObtenerDocumentoActual.SelectionColor = Color.LightGreen;
        }

        private void HighlightOrange_Click(object sender, EventArgs e)
        {
            ObtenerDocumentoActual.SelectionColor = Color.Orange;
        }

        private void HighlightYellow_Click(object sender, EventArgs e)
        {
            ObtenerDocumentoActual.SelectionColor = Color.LightYellow;
        }

        private void toolStripComboBoxFontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font NewFont = new Font(toolStripComboBoxFontFamily.SelectedItem.ToString(),
                ObtenerDocumentoActual.SelectionFont.Size, ObtenerDocumentoActual.SelectionFont.Style);

            ObtenerDocumentoActual.SelectionFont = NewFont;
        }

        private void toolStripComboBoxFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            float NuevoTamano;

            float.TryParse(toolStripComboBoxFontSize.SelectedItem.ToString(), out NuevoTamano);

            Font NuevaFuente = new Font(ObtenerDocumentoActual.SelectionFont.Name, NuevoTamano,
                ObtenerDocumentoActual.SelectionFont.Style);

            ObtenerDocumentoActual.SelectionFont = NuevaFuente;
        }

        private void timerTextEditor_Tick(object sender, EventArgs e)
        {
            if (ObtenerDocumentoActual.Text.Length > 0)
            { 
                toolStripStatusBottomLabel.Text = ObtenerDocumentoActual.Text.Length.ToString();
            }
        }

        private void nuevoToolStripButton_Click(object sender, EventArgs e)
        {
            AgregarPestana();
        }

        private void eliminarToolStripButton_Click(object sender, EventArgs e)
        {
            EliminarPestana();
        }

        private void abrirToolStripButton_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void guardarToolStripButton_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void cortarToolStripButton_Click(object sender, EventArgs e)
        {
            Cortar();
        }

        private void copiarToolStripButton_Click(object sender, EventArgs e)
        {
            Copiar();
        }

        private void pegarToolStripButton_Click(object sender, EventArgs e)
        {
            Pegar();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AgregarPestana();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void guardarcomoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarComo();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deshacer();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rehacer();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cortar();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copiar();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pegar();
        }

        private void seleccionartodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeleccionarTodo();
        }
    }
}
