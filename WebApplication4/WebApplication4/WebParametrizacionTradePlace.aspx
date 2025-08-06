<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebParametrizacionTradePlace.aspx.cs" Inherits="TradePlace.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />    
    <link href="css/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <script src="js/bootstrap.bundle.min.js"></script> 
    <script src="js/sweetalert2@11.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container-fluid mt-5">
            <div class="row justify-content-center">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="mb-3 d-flex justify-content-between align-items-end flex-wrap gap-3">
                                <div id="divConfiguracion" runat="server" class="div-configuracion ancho-grid">
                                    <div class="filtro">
                                        <label for="ddlConfiguracion" class="form-label">Tipo Configuración</label>
                                        <asp:DropDownList ID="ddlConfiguracion" runat="server" CssClass="form-select"
                                            Style="width:100%;" AutoPostBack="true" OnSelectedIndexChanged="ddlConfiguracion_SelectedIndexChanged">
                                            <asp:ListItem Text="Plantilla Genérica" value="GEN" />
                                            <asp:ListItem Text="Para una Comunidad" value="COM"/>
                                            <asp:ListItem Text="Para un Proveedor" value="PRO" />
                                        </asp:DropDownList>
                                    </div>

                                    <div id="divComunidad" runat="server" class="filtro">
                                        <label for="ddlComunidad" class="form-label">Comunidad</label>
                                        <asp:DropDownList ID="ddlCommunity" runat="server" CssClass="form-select" Style="min-width:200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlCommunity_SelectedIndexChanged" />
                                    </div>

                                    <div id="divProveedor" runat="server" class="filtro">
                                        <label for="txtProveedor" class="form-label">Proveedor</label>
                                        <div style="display: flex; align-items: center; ">
                                            <asp:TextBox ID="txtProveedor" runat="server" CssClass="form-control" Style="min-width:200px;" Placeholder="Buscar proveedor..." />
                                            <asp:LinkButton ID="btnBuscarProveedor" runat="server" OnClick="btn_BuscarProveedor" CssClass="boton-personalizado" Style="margin-left: 10px;" CausesValidation="false">
                                                <i class="fas fa-search"></i> Buscar
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div id="divGrupoComunidad" runat="server" class="grupo-comunidad">
                                        <div id="divDocumento" runat="server" class="filtro">
                                            <label for="ddlDocumento" class="form-label">Documento</label>
                                            <asp:DropDownList ID="ddlDocumento" runat="server" CssClass="form-select" Style="min-width:200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlDocumento_SelectedIndexChanged" />
                                        </div>

                                        <div id="divCampoPlantilla" runat="server" class="filtro">
                                            <label for="ddlCampoPlantilla" class="form-label">Campo Plantilla</label>
                                            <asp:DropDownList ID="ddlCampoPlantilla" runat="server" CssClass="form-select" Style="min-width:200px;" />
                                        </div>
                                    </div>

                                    <div id="divCopiaComunidad" runat="server" class="filtro">
                                        <label for="ddlCopiaComunidad" class="form-label">Copia desde la Comunidad</label>
                                        <asp:DropDownList ID="ddlCopiaComunidad" runat="server" CssClass="form-select" Style="min-width:200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlCopiaCommunity_SelectedIndexChanged" />
                                    </div>

                                </div>
                                
                            </div>

                            <!-- Modal para proveedores -->
                            <div id="modalProveedores" runat="server" ClientIDMode="Static" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalProveedoresLabel" aria-hidden="true">
                              <div class="modal-dialog modal-lg" role="document">
                                <div class="modal-content">
                                  <div class="modal-header">
                                    <h5 class="modal-title">Seleccionar Proveedor</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                  </div>
                                  <div class="modal-body">
                                    <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" 
                                                  OnRowCommand="gvProveedores_RowCommand">
                                      <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="Name" HeaderText="Nombre" />
                                        <asp:TemplateField>
                                          <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" runat="server" Text="Seleccionar"
                                                CommandName="Seleccionar" CommandArgument='<%# Eval("Id") + "|" + Eval("Name") %>' />
                                          </ItemTemplate>
                                        </asp:TemplateField>
                                      </Columns>
                                    </asp:GridView>
                                  </div>
                                </div>
                              </div>
                            </div>

                            <div id="TipoExtraccion" runat="server" class="tipo-extraccion">
                                <label for="ddlTipoExtraccion" class="form-label">Tipo de Extracción:</label>
                                <asp:DropDownList ID="ddlTipoExtraccion" runat="server" CssClass="form-select" >
                                    <asp:ListItem Text="EDI" value="EDI" />
                                    <asp:ListItem Text="CSV" value="CSV" />
                                    <asp:ListItem Text="JSON" value="JSON" />
                                    <asp:ListItem Text="Texto Plano (TXT)" value="TXT" />
                                    <asp:ListItem Text="XML" value="XML" />
                                    <asp:ListItem Text="Excel (XLSX)" value="XLSX" />
                                </asp:DropDownList>
                                <i class="fas fa-info-circle texto-informativo"></i>
                                <span class="texto-informativo">Este tipo aplicará a todos los campos de esta configuración</span>
                            </div>
                            
                            <div class="mb-3">
                                <asp:Button ID="btnAgregar" runat="server" OnClientClick="return validarSeleccion();" Text="Agregar Campo" OnClick="btn_Agregar" CssClass="boton-personalizado" ValidationGroup="GuardarFormulario" />
                            
                                <asp:Button ID="btnCopiarConf" runat="server" OnClientClick="return validarSeleccion();" Text="Copiar Configuración" OnClick="btn_CopiarConf" CssClass="boton-personalizado" ValidationGroup="GuardarFormulario" />
                            </div>

                            <asp:GridView ID="GridParametros" runat="server" AutoGenerateColumns="False" OnRowCommand="GridParametros_RowCommand" OnRowDataBound="GridParametros_RowDataBound" CssClass="table custom-gridview ancho-grid">
                                <Columns>

                                <asp:TemplateField HeaderStyle-CssClass="d-none" ItemStyle-CssClass="d-none">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtIdField" runat="server" Text='<%# Bind("IdField") %>' CssClass="form-control" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Nombre Campo" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtNombreCampo" runat="server" Text='<%# Bind("FieldName") %>' CssClass="form-control" />
                                        <asp:RequiredFieldValidator 
                                            ID="rfvNombreCampo" 
                                            runat="server" 
                                            ControlToValidate="txtNombreCampo" 
                                            ErrorMessage="Nombre Campo no puede estar en blanco" 
                                            Display="Dynamic" 
                                            ForeColor="Red"
                                            ValidationGroup="GuardarFormulario" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Nombre en Tabla" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtNombreTabla" runat="server" Text='<%# Bind("TableName") %>' CssClass="form-control" />
                                        <asp:RequiredFieldValidator 
                                            ID="rfvNombreTabla" 
                                            runat="server" 
                                            ControlToValidate="txtNombreTabla" 
                                            ErrorMessage="Nombre Tabla no puede estar en blanco" 
                                            Display="Dynamic" 
                                            ForeColor="Red"
                                            ValidationGroup="GuardarFormulario" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="Encabezado" value="E" />
                                            <asp:ListItem Text="Detalle" value="D" />
                                            <asp:ListItem Text="Totales" value="T" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Tipo Dato" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlTipoDato" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="Texto (VARCHAR)" value="VARCHAR" />
                                            <asp:ListItem Text="Número (INT)" value="INTEGER" />
                                            <asp:ListItem Text="Fecha (DATE)" value="DATETIME" />
                                            <asp:ListItem Text="Decimal (DECIMAL)" value="DECIMAL" />
                                            <asp:ListItem Text="Booleano (BIT)" value="BIT" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Longitud" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLongitud" runat="server" Text='<%# Bind("Length") %>' CssClass="form-control" TextMode="Number" />
                                        <asp:RangeValidator 
                                        ID="rngLongitud" runat="server" 
                                        ControlToValidate="txtLongitud" 
                                        MinimumValue="0" MaximumValue="99999" 
                                        Type="Integer"
                                        ErrorMessage="No se permiten números negativos"
                                        CssClass="text-danger" Display="Dynamic" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Decimales" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDecimal" runat="server" Text='<%# Bind("Dec") %>' CssClass="form-control" TextMode="Number" />
                                        <asp:RangeValidator 
                                        ID="rngDecimal" runat="server" 
                                        ControlToValidate="txtDecimal" 
                                        MinimumValue="0" MaximumValue="99999" 
                                        Type="Integer"
                                        ErrorMessage="No se permiten números negativos"
                                        CssClass="text-danger" Display="Dynamic" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Obligatorio" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <div class="d-flex justify-content-center checkbox-grande">
                                            <asp:CheckBox ID="chkObligatorio" runat="server" Checked='<%# Bind("Required") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Orden" HeaderStyle-Font-Size="Small">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOrden" runat="server" Text='<%# Bind("Order") %>' CssClass="form-control" TextMode="Number" ReadOnly="true" />
                                        <asp:RangeValidator 
                                        ID="rngOrden" runat="server" 
                                        ControlToValidate="txtOrden" 
                                        MinimumValue="1" MaximumValue="99999" 
                                        Type="Integer"
                                        ErrorMessage="Solo se permiten números positivos mayores a cero"
                                        CssClass="text-danger" Display="Dynamic" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-Font-Size="Small" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:LinkButton ID="btnGuardar" runat="server" CommandName="Guardar" CausesValidation="true" ValidationGroup="GuardarFormulario" CommandArgument='<%# Container.DataItemIndex %>' style="text-decoration: none; border: none; background: none;" >
                                                <i class="fas fa-save" style="color: green; font-size: 24px;"></i>
                                            </asp:LinkButton>
                                       
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este registro?');" style="text-decoration: none; border: none; background: none;" >
                                                <i class="fas fa-trash-alt" style="color: red; font-size: 24px;"></i>
                                            </asp:LinkButton>

                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="mb-3 text-end">                            
                            <asp:LinkButton ID="btnReset" runat="server" OnClick="btn_Reset" CssClass="boton-reiniciar">
                                <i class="fas fa-undo"></i> Reiniciar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnGuardar" runat="server" OnClientClick="return validarSeleccion();" ValidationGroup="GuardarFormulario" OnClick="btn_Guardar" CssClass="boton-personalizado">
                                <i class="fas fa-save"></i> Guardar Todo
                            </asp:LinkButton>
                        </div>

                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function validarSeleccion() {
                var ddlComunidad = document.getElementById('<%= ddlCommunity.ClientID %>');
                var ddlDocumento = document.getElementById('<%= ddlDocumento.ClientID %>');
                var ddlCampoPlantilla = document.getElementById('<%= ddlCampoPlantilla.ClientID %>');
                var ddlCopiaComunidad = document.getElementById('<%= ddlCopiaComunidad.ClientID %>');
                var txtProveedor = document.getElementById('<%= txtProveedor.ClientID %>');
                
                if (ddlDocumento.value === "") {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Campo requerido',
                        text: 'Debe seleccionar un documento.',
                        confirmButtonText: 'Entendido'
                    });
                    return false;
                }
                else if (ddlComunidad.value === "") {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Campo requerido',
                        text: 'Debe seleccionar una comunidad.',
                        confirmButtonText: 'Entendido'
                    });
                    return false;
                }
                else if (ddlCampoPlantilla.value === "" && ddlCopiaComunidad.value === "") {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Campo requerido',
                        text: 'Debe seleccionar un campo.',
                        confirmButtonText: 'Entendido'
                    });
                    return false;
                }
                else if (txtProveedor.value === "" && ddlCopiaComunidad.value === "") {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Campo requerido',
                        text: 'Debe indicar un proveedor.',
                        confirmButtonText: 'Entendido'
                    });
                    return false;
                }
                return true;
            }
        </script>
    </form>
</body>
</html>