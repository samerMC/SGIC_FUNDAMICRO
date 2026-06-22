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
                CssClass="validation-message"
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
                            <asp:LinkButton
                                ID="lnkEditar"
                                runat="server"
                                Text="Editar"
                                CommandName="EditarCliente"
                                CommandArgument='<%# Eval("IdCliente") %>' />

                            <asp:LinkButton
                                ID="lnkEliminar"
                                runat="server"
                                Text="Eliminar"
                                CommandName="EliminarCliente"
                                CommandArgument='<%# Eval("IdCliente") %>'
                                CssClass="danger-link"
                                OnClientClick="return confirm('¿Confirma eliminar este cliente?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </section>
    </main>
</asp:Content>
