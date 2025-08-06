using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradePlace
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private List<Field> DatosCampos
        {
            get
            {
                return ViewState["DatosCampos"] as List<Field> ?? new List<Field>();
            }
            set
            {
                ViewState["DatosCampos"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                GetCommunity();
                GetDocument();
                divComunidad.Visible = false;
                divProveedor.Visible = false;
                divDocumento.Visible = true;
                divCampoPlantilla.Visible = false;
                divGrupoComunidad.Visible = true;
                divCopiaComunidad.Visible = false;
                txtProveedor.Enabled = false;
                btnReset.Visible = false;
                btnGuardar.Visible = false;
                btnBuscarProveedor.Enabled = false;
            }
            if (ddlCopiaComunidad.SelectedItem == null)
            {
                btnCopiarConf.Attributes["disabled"] = "true";
                btnCopiarConf.Attributes["style"] = "background-color: #ccc; color: #666; cursor: not-allowed;";
            }
        }

        protected void GetCommunity()
        {
            DBConect dbConect = new DBConect();
            List<Community> lCommunity = dbConect.GetCommunity();

            ddlCommunity.DataSource = lCommunity;
            ddlCommunity.DataTextField = "Name";
            ddlCommunity.DataValueField = "Id";
            ddlCommunity.DataBind();
            ddlCommunity.Items.Insert(0, new ListItem("Seleccione Comunidad", ""));
        }

        protected void GetCopyCommunity(string seleccion)
        {
            DBConect dbConect = new DBConect();
            List<Community> lCommunity = dbConect.GetCommunity();

            lCommunity.RemoveAll(c => c.Id == seleccion);
            ddlCopiaComunidad.DataSource = lCommunity;
            ddlCopiaComunidad.DataTextField = "Name";
            ddlCopiaComunidad.DataValueField = "Id";
            ddlCopiaComunidad.DataBind();
            ddlCopiaComunidad.Items.Insert(0, new ListItem("Seleccione Comunidad", ""));
        }

        protected void GetDocument()
        {
            DBConect dbConect = new DBConect();
            List<Document> lDocument = dbConect.GetDocument();

            ddlDocumento.DataSource = lDocument;
            ddlDocumento.DataTextField = "Name";
            ddlDocumento.DataValueField = "Id";
            ddlDocumento.DataBind();
            ddlDocumento.Items.Insert(0, new ListItem("Seleccione Documento", ""));
        }

        protected void btn_Agregar(object sender, EventArgs e)
        {
            
            List<Field> lField = new List<Field>();
            string sType = string.Empty;
            string sDataType = string.Empty;
            foreach (GridViewRow fila in GridParametros.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    // Obtener controles por ID desde la fila
                    TextBox txtIdField = (TextBox)fila.FindControl("txtIdField");
                    TextBox txtNombreCampo = (TextBox)fila.FindControl("txtNombreCampo");
                    TextBox txtNombreTabla = (TextBox)fila.FindControl("txtNombreTabla");
                    DropDownList ddlTipo = (DropDownList)fila.FindControl("ddlTipo");
                    DropDownList ddlTipoDato = (DropDownList)fila.FindControl("ddlTipoDato");
                    TextBox txtLongitud = (TextBox)fila.FindControl("txtLongitud");
                    TextBox txtDecimal = (TextBox)fila.FindControl("txtDecimal");
                    CheckBox chkObligatorio = (CheckBox)fila.FindControl("chkObligatorio");
                    TextBox txtOrden = (TextBox)fila.FindControl("txtOrden");
                    DropDownList ddlTipoExtraccion = (DropDownList)fila.FindControl("ddlTipoExtraccion");

                    // Obtener valores
                    string idField = txtIdField?.Text ?? "";
                    string nombrecampo = txtNombreCampo?.Text ?? "";
                    string nombretabla = txtNombreTabla?.Text ?? "";
                    string tipo = ddlTipo?.SelectedValue ?? "";
                    string tipodato = ddlTipoDato?.SelectedValue ?? "";
                    string longitud = txtLongitud?.Text ?? "";
                    string dec = txtDecimal?.Text ?? "";
                    bool obligatorio = chkObligatorio?.Checked ?? false;
                    string orden = txtOrden?.Text ?? "";
                    string extractor = ddlTipoExtraccion?.SelectedValue ?? "";

                    Field oCampo = new Field();
                    oCampo.IdField = idField;
                    oCampo.FieldName = nombrecampo;
                    oCampo.TableName = nombretabla;
                    oCampo.Type = tipo;
                    oCampo.DataType = tipodato;
                    oCampo.Length = longitud;
                    oCampo.Dec = dec;
                    oCampo.Required = Convert.ToBoolean(obligatorio);
                    oCampo.Order = orden;
                    oCampo.Extractor = extractor;

                    lField.Add(oCampo);

                }
            }

            var lista = lField;
            string sConfiguracion = ddlConfiguracion.SelectedItem.Value;
            if (sConfiguracion.Equals("GEN"))
            {
                lista.Add(new Field {IdField = (lista.Count + 1).ToString(), FieldName = "", TableName = "", Type = "Encabezado", DataType = "Número (INT)", Length = "0", Dec = "0", Required = false, Order = (lista.Count + 1).ToString() });
                DatosCampos = lista;
            }

            else
            {
                List<Template> lTemplate = new List<Template>();
                lTemplate = ((List<Template>) Session["lTemplate"]);
                foreach (Template TP in lTemplate)
                {
                    ddlTipoExtraccion.SelectedValue = TP.Extractor;
                    if (TP.Id.Equals(ddlCampoPlantilla.SelectedItem.Value))
                    {
                        bool bExits = false;
                        foreach (Field fl in lista)
                        {
                            if (TP.Id.Trim() == fl.IdField.Trim())
                            {
                                bExits = true;
                                break;
                            }                            
                        }

                        if (!bExits)
                        {
                            sType = TP.Type.ToUpper().Trim();
                            if (sType.Equals("E"))
                            {
                                sType = "E";
                            }
                            else
                            {
                                sType = "D";
                            }

                            sDataType = TP.DataType.ToUpper().Trim();

                            string sRequired = TP.Required.ToUpper().Trim();
                            bool bRequired = false;
                            if (sRequired.Equals("Y"))
                            {
                                bRequired = true;
                            }

                            lista.Add(new Field { IdField = TP.Id, FieldName = TP.Name, TableName = TP.TableName, Type = sType, DataType = sDataType, Length = TP.Length, Dec = TP.LengthDecimal, Required = bRequired, Order = TP.Order });
                            DatosCampos = lista;
                            break;
                        }
                        
                    }
                }
                  
            }

            GridParametros.DataSource = DatosCampos;
            GridParametros.DataBind();

            btnReset.Visible = true;
            btnGuardar.Visible = true;
        }

        protected void GridParametros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtNombreCampo = (TextBox)e.Row.FindControl("txtNombreCampo");
                DropDownList ddlTipo = (DropDownList)e.Row.FindControl("ddlTipo");
                DropDownList ddlTipoDato = (DropDownList)e.Row.FindControl("ddlTipoDato");

                Field campo = (Field)e.Row.DataItem;

                if (ddlTipo != null && campo != null)
                    ddlTipo.SelectedValue = campo.Type;

                if (ddlTipoDato != null && campo != null)
                    ddlTipoDato.SelectedValue = campo.DataType;

                string seleccion = ddlConfiguracion.SelectedItem.Value;
                if (seleccion.Equals("COM") || seleccion.Equals("PRO"))
                {
                    txtNombreCampo.Enabled = false;
                    ddlTipo.Enabled = false;
                }
                else
                {
                    txtNombreCampo.Enabled = true;
                    ddlTipo.Enabled = true;
                }
                
            }
        }

        protected void GridParametros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow fila = GridParametros.Rows[index];

            if (e.CommandName == "Guardar")
            {
                int iCommunityId = 0;
                int iSupplierId = 0;
                int iDocumentId = Convert.ToInt32(ddlDocumento.SelectedItem.Value);
                string seleccion = ddlConfiguracion.SelectedItem.Value;
                string sExtractionType = ddlTipoExtraccion.SelectedItem.Value;

                if (seleccion.Equals("COM"))
                {
                    iCommunityId = Convert.ToInt32(ddlCommunity.SelectedItem.Value);
                }
                else if (seleccion.Equals("PRO"))
                {
                    iCommunityId = Convert.ToInt32(ddlCommunity.SelectedItem.Value);
                    iSupplierId = Convert.ToInt32(Session["idProveedor"]);
                }

                string sFieldName = ((TextBox)fila.FindControl("txtNombreCampo")).Text;
                string sFieldTableName = ((TextBox)fila.FindControl("txtNombreTabla")).Text;
                string sFieldType = ((DropDownList)fila.FindControl("ddlTipo")).SelectedValue;
                string sDataType = ((DropDownList)fila.FindControl("ddlTipoDato")).SelectedValue;
                int iLength = Convert.ToInt32(((TextBox)fila.FindControl("txtLongitud")).Text);
                string sDecimalLength = ((TextBox)fila.FindControl("txtDecimal")).Text;
                int iFieldOrder = Convert.ToInt32(((TextBox)fila.FindControl("txtOrden")).Text);
                bool bIsRequired = ((CheckBox)fila.FindControl("chkObligatorio")).Checked;

                //Guardar Valores
                DBConect dbConect = new DBConect();
                dbConect.SaveStructureExtractor(sExtractionType, sFieldName, sFieldTableName, sFieldType, sDataType, iLength, sDecimalLength, iFieldOrder, bIsRequired, iDocumentId, iCommunityId, iSupplierId);

                string script = "Swal.fire({ icon: 'success', title: '¡Guardado!', text: 'Los datos fueron guardados exitosamente.' });";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaGuardado", script, true);

            }
            else if (e.CommandName == "Eliminar")
            {
                var lista = DatosCampos;
                if (index >= 0 && index < lista.Count)
                {
                    string sExtractionType = ddlTipoExtraccion.SelectedItem.Value;
                    TextBox txtIdField = (TextBox)fila.FindControl("txtIdField");
                    string idField = txtIdField?.Text ?? "";


                    DBConect dbConect = new DBConect();
                    dbConect.DeleteTemplate(sExtractionType, idField);

                    lista.RemoveAt(index);
                    DatosCampos = lista;
                    GridParametros.DataSource = lista;
                    GridParametros.DataBind();

                    if (GridParametros.Rows.Count == 0)
                    {
                        btnReset.Visible = false;
                        btnGuardar.Visible = false;
                    }

                    string script = "Swal.fire({ icon: 'success', title: '¡Eliminado!', text: 'Los datos fueron eliminados exitosamente.' });";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaGuardado", script, true);
                }
            }
        }
        protected void ddlConfiguracion_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_Reset(sender, e);
            string seleccion = ddlConfiguracion.SelectedItem.Value;
            if (seleccion.Equals("COM"))
            {
                divComunidad.Visible = true;
                divProveedor.Visible = false;
                divGrupoComunidad.Visible = true;
                divDocumento.Visible = true;
                divCampoPlantilla.Visible = true;
                ddlTipoExtraccion.Enabled = false;
                divCopiaComunidad.Visible = true;
            }
            else if (seleccion.Equals("PRO"))
            {
                divComunidad.Visible = true;
                divProveedor.Visible = true;
                divDocumento.Visible = true;
                divCampoPlantilla.Visible = true;
                divGrupoComunidad.Visible = true;
                divCopiaComunidad.Visible = true;
                ddlTipoExtraccion.Enabled = false;
            }
            else 
            {
                divComunidad.Visible = false;
                divProveedor.Visible = false;
                divDocumento.Visible = true;
                divCampoPlantilla.Visible = false;
                divGrupoComunidad.Visible = true;
                divCopiaComunidad.Visible = false;
                ddlTipoExtraccion.Enabled = true;
            }
        }

        protected void ddlCommunity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccion = ddlCommunity.SelectedItem.Value;
            if (seleccion.Trim() != "")
            {
                txtProveedor.Enabled = true;
                btnBuscarProveedor.Enabled = true;
                GetCopyCommunity(seleccion);
            }
            else
            {
                txtProveedor.Text = "";
                txtProveedor.Enabled = false;
                btnBuscarProveedor.Enabled = false;
            }
        }

        protected void ddlCopiaCommunity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCopiaComunidad.SelectedItem != null)
            {
                string seleccion = ddlCopiaComunidad.SelectedItem.Value;
                if (seleccion.Trim() != "")
                {
                    ddlTipoExtraccion.Enabled = true;

                    btnAgregar.Attributes["disabled"] = "true";
                    btnAgregar.Attributes["style"] = "background-color: #ccc; color: #666; cursor: not-allowed;";

                    btnCopiarConf.Attributes.Remove("disabled");
                    btnCopiarConf.Attributes["style"] = "";
                    btnCopiarConf.CssClass = "boton-personalizado";
                }
                else
                {
                    ddlTipoExtraccion.Enabled = false;

                    btnAgregar.Attributes.Remove("disabled");
                    btnAgregar.Attributes["style"] = "";
                    btnAgregar.CssClass = "boton-personalizado";

                    btnCopiarConf.Attributes["disabled"] = "true";
                    btnCopiarConf.Attributes["style"] = "background-color: #ccc; color: #666; cursor: not-allowed;";
                }
            }
        }

        protected void ddlDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccion = ddlDocumento.SelectedItem.Value;

            if (seleccion.Trim() != "")
            {
                DBConect dbConect = new DBConect();
                List<Template> lTemplate = dbConect.GetTemplate(Convert.ToInt32(seleccion));
                Session["lTemplate"] = lTemplate;
                ddlCampoPlantilla.DataSource = lTemplate;
                ddlCampoPlantilla.DataTextField = "Name";
                ddlCampoPlantilla.DataValueField = "Id";
                ddlCampoPlantilla.DataBind();
                ddlCampoPlantilla.Items.Insert(0, new ListItem("Seleccione Campo", ""));
            }
            else
            {
                ddlCampoPlantilla.Items.Clear();
                ddlCampoPlantilla.DataBind();
                ddlCampoPlantilla.Items.Insert(0, new ListItem("Seleccione Campo", ""));
            }
        }

        protected void btn_Reset(object sender, EventArgs e)
        {
            DatosCampos = null;

            GridParametros.DataSource = DatosCampos;
            GridParametros.DataBind();

            btnReset.Visible = false;
            btnGuardar.Visible = false;
        }

        protected void btn_BuscarProveedor(object sender, EventArgs e)
        {
            DBConect dbConect = new DBConect();
            List<Supplier> lSupplier = dbConect.GetSupplier(Convert.ToInt32(ddlCommunity.SelectedItem.Value), txtProveedor.Text.Trim());

            if (lSupplier.Count == 1)
            {
                Session["idProveedor"] = lSupplier[0].Id;
                txtProveedor.Text = lSupplier[0].Name;
            }
            else
            {
                gvProveedores.DataSource = lSupplier;
                gvProveedores.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", @"var myModal = new bootstrap.Modal(document.getElementById('modalProveedores'));myModal.show();", true);
            }
        }
        protected void gvProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                string[] valores = e.CommandArgument.ToString().Split('|');
                string idProveedor = valores[0];
                string nombreProveedor = valores[1];

                Session["idProveedor"] = idProveedor;
                txtProveedor.Text = nombreProveedor;

                // Cierra el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", @"
                setTimeout(function() {
                    var modalElement = document.getElementById('modalProveedores');
                    var modal = bootstrap.Modal.getInstance(modalElement);
                    if (!modal) {
                        modal = new bootstrap.Modal(modalElement);
                    }
                    modal.hide();

                    // Limpia el backdrop y la clase modal-open
                    document.body.classList.remove('modal-open');
                    var backdrops = document.querySelectorAll('.modal-backdrop');
                    backdrops.forEach(function(b) { b.remove(); });
                }, 200);", true);

            }
        }
        protected void btn_Guardar(object sender, EventArgs e)
        {
            foreach (GridViewRow fila in GridParametros.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    int iCommunityId = 0;
                    int iSupplierId = 0;
                    int iDocumentId = Convert.ToInt32(ddlDocumento.SelectedItem.Value);
                    string seleccion = ddlConfiguracion.SelectedItem.Value;

                    if(seleccion.Equals("COM"))
                    {
                        iCommunityId = Convert.ToInt32(ddlCommunity.SelectedItem.Value);
                    }
                    else if (seleccion.Equals("PRO"))
                    {
                        iCommunityId = Convert.ToInt32(ddlCommunity.SelectedItem.Value);
                        iSupplierId = Convert.ToInt32(Session["idProveedor"]);
                    }

                    string sExtractionType = ddlTipoExtraccion.SelectedItem.Value;
                    string sFieldName = ((TextBox)fila.FindControl("txtNombreCampo")).Text;
                    string sFieldTableName = ((TextBox)fila.FindControl("txtNombreTabla")).Text;
                    string sFieldType = ((DropDownList)fila.FindControl("ddlTipo")).SelectedValue;
                    string sDataType = ((DropDownList)fila.FindControl("ddlTipoDato")).SelectedValue;
                    int iLength = Convert.ToInt32(((TextBox)fila.FindControl("txtLongitud")).Text);
                    string sDecimalLength = ((TextBox)fila.FindControl("txtDecimal")).Text;
                    int iFieldOrder = Convert.ToInt32(((TextBox)fila.FindControl("txtOrden")).Text);
                    bool bIsRequired = ((CheckBox)fila.FindControl("chkObligatorio")).Checked;

                    //Guardar Valores
                    DBConect dbConect = new DBConect();
                    dbConect.SaveStructureExtractor(sExtractionType, sFieldName, sFieldTableName, sFieldType, sDataType, iLength, sDecimalLength, iFieldOrder, bIsRequired, iDocumentId, iCommunityId, iSupplierId);
                }
            }
            string script = "Swal.fire({ icon: 'success', title: '¡Guardado!', text: 'Los datos fueron guardados exitosamente.' });";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaGuardado", script, true);
        }

        protected void btn_CopiarConf(object sender, EventArgs e)
        {
            List<Field> lField = new List<Field>();
            string sType = string.Empty;
            string sDataType = string.Empty;
            string sMensaje = string.Empty;

            DBConect dbConect = new DBConect();
            dbConect.CopyCommunity(Convert.ToInt32(ddlDocumento.SelectedValue), Convert.ToInt32(ddlCommunity.SelectedValue), Convert.ToInt32(ddlCopiaComunidad.SelectedValue), ddlTipoExtraccion.SelectedValue, out sMensaje);

            
            if (!string.IsNullOrEmpty(sMensaje))
            {
                string script = $"Swal.fire({{ icon: 'error', title: '¡Error!', text: '{sMensaje}' }});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaGuardado", script, true);
            }
            else
            {
                List<Template> lTemplate = dbConect.GetCopyCommunity(Convert.ToInt32(ddlDocumento.SelectedValue), Convert.ToInt32(ddlCopiaComunidad.SelectedValue), ddlTipoExtraccion.SelectedValue);
                foreach (Template TP in lTemplate)
                {
                    sType = TP.Type.ToUpper().Trim();
                    if (sType.Equals("E"))
                    {
                        sType = "E";
                    }
                    else
                    {
                        sType = "D";
                    }

                    sDataType = TP.DataType.ToUpper().Trim();

                    string sRequired = TP.Required.ToUpper().Trim();
                    bool bRequired = false;
                    if (sRequired.Equals("Y"))
                    {
                        bRequired = true;
                    }

                    lField.Add(new Field { IdField = TP.Id, FieldName = TP.Name, TableName = TP.TableName, Type = sType, DataType = sDataType, Length = TP.Length, Dec = TP.LengthDecimal, Required = bRequired, Order = TP.Order });
                }

                DatosCampos = lField;
                GridParametros.DataSource = DatosCampos;
                GridParametros.DataBind();

                btnReset.Visible = true;
                btnGuardar.Visible = true;

                string script = "Swal.fire({ icon: 'success', title: '¡Copiado!', text: 'Los datos fueron copiados exitosamente.' });";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaGuardado", script, true);
            }
        }
    }
}