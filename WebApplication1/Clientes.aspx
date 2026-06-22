<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Clientes.aspx.vb" Inherits="WebApplication1.Clientes" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="page-container">
        <section class="page-header">
            <h1>Mantenimiento de clientes</h1>
            <p>
                Administre la información de los clientes registrados en el sistema.
            </p>
        </section>

        <section class="form-panel">
            <h2>Datos del cliente</h2>

            <asp:HiddenField ID="hfIdCliente" runat="server" />
            <asp:HiddenField ID="hfIdClienteEliminar" runat="server" />

            <div class="form-grid">
                <div class="form-group">
                    <label for="txtNombre">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                </div>

                <div class="form-group">
                    <label for="txtApellido">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="100" />
                </div>

                <div class="form-group">
                    <label for="txtDui">DUI</label>
                    <asp:TextBox ID="txtDui" runat="server" CssClass="form-control" MaxLength="10" />
                </div>

                <div class="form-group">
                    <label for="txtNit">NIT</label>
                    <asp:TextBox ID="txtNit" runat="server" CssClass="form-control" MaxLength="20" />
                </div>

                <div class="form-group">
                    <label for="txtTelefono">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="20" />
                </div>

                <div class="form-group">
                    <label for="txtCorreo">Correo electrónico</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" MaxLength="150" />
                </div>

                <div class="form-group form-group-full">
                    <label for="txtDireccion">Dirección</label>
                    <asp:TextBox
                        ID="txtDireccion"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="250" />
                </div>
            </div>

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="app-message"
                EnableViewState="false" />

            <div class="form-actions">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" CausesValidation="false" />
            </div>
        </section>

        <section class="table-panel">
            <h2>Clientes registrados</h2>

            <asp:GridView
                ID="gvClientes"
                runat="server"
                CssClass="data-table"
                AutoGenerateColumns="False"
                EmptyDataText="No hay clientes registrados.">

                <Columns>
                    <asp:BoundField DataField="IdCliente" HeaderText="ID" />
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Cliente" />
                    <asp:BoundField DataField="Dui" HeaderText="DUI" />
                    <asp:BoundField DataField="Nit" HeaderText="NIT" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Correo" HeaderText="Correo" />
                    <asp:BoundField DataField="EstadoTexto" HeaderText="Estado" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <div class="table-actions">
                                <asp:LinkButton
                                    ID="lnkEditar"
                                    runat="server"
                                    Text="Editar"
                                    CommandName="EditarCliente"
                                    CommandArgument='<%# Eval("IdCliente") %>'
                                    CssClass="action-link action-edit" />

                                <asp:LinkButton
                                    ID="lnkEliminar"
                                    runat="server"
                                    Text="Eliminar"
                                    CommandName="EliminarClienteModal"
                                    CommandArgument='<%# Eval("IdCliente") %>'
                                    CssClass="action-link action-danger"
                                    OnClientClick='<%# String.Format("mostrarModalEliminarCliente({0}); return false;", Eval("IdCliente")) %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </section>
        <asp:Button
            ID="btnConfirmarEliminar"
            runat="server"
            Text="Confirmar eliminación"
            Style="display:none;"
            CausesValidation="false" />
    </main>
    <div id="modalEliminarCliente" class="app-modal-overlay" aria-hidden="true">
        <div class="app-modal" role="dialog" aria-modal="true" aria-labelledby="modalEliminarClienteTitulo">
            <h2 id="modalEliminarClienteTitulo">Confirmar eliminación</h2>

            <p>
                ¿Confirma que desea eliminar este cliente?
            </p>

            <p class="form-hint">
                El registro no se eliminará físicamente; quedará desactivado para conservar trazabilidad.
            </p>

            <div class="app-modal-actions">
                <button type="button" class="btn btn-danger" onclick="confirmarEliminarCliente();">
                    Eliminar
                </button>

                <button type="button" class="btn btn-secondary" onclick="cerrarModalEliminarCliente();">
                    Cancelar
                </button>
            </div>
        </div>
    </div>

    <script>
        function mostrarModalEliminarCliente(idCliente) {
            document.getElementById('<%= hfIdClienteEliminar.ClientID %>').value = idCliente;

            const modal = document.getElementById('modalEliminarCliente');
            modal.classList.add('is-visible');
            modal.setAttribute('aria-hidden', 'false');
        }

        function cerrarModalEliminarCliente() {
            const modal = document.getElementById('modalEliminarCliente');
            modal.classList.remove('is-visible');
            modal.setAttribute('aria-hidden', 'true');

            document.getElementById('<%= hfIdClienteEliminar.ClientID %>').value = '';
        }

        function confirmarEliminarCliente() {
            document.getElementById('<%= btnConfirmarEliminar.ClientID %>').click();
        }
    </script>
</asp:Content>
