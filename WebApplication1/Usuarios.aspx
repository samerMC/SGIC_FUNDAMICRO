<%@ Page Title="Usuarios" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Usuarios.aspx.vb" Inherits="WebApplication1.Usuarios" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="page-container">
        <section class="page-header">
            <h1>Mantenimiento de usuarios</h1>
            <p>Administre los usuarios autorizados para ingresar al sistema.</p>
        </section>

        <section class="form-panel">
            <h2>Datos del usuario</h2>

            <asp:HiddenField ID="hfIdUsuario" runat="server" />

            <div class="form-grid">
                <div class="form-group">
                    <label for="txtNombreUsuario">Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" MaxLength="50" />
                </div>

                <div class="form-group">
                    <label for="txtNombreCompleto">Nombre completo</label>
                    <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="form-control" MaxLength="150" />
                </div>

                <div class="form-group">
                    <label for="txtCorreo">Correo</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" MaxLength="150" />
                </div>

                <div class="form-group">
                    <label for="ddlRol">Rol</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="txtContrasena">Contraseña</label>
                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" />
                </div>

                <div class="form-group">
                    <label for="txtConfirmarContrasena">Confirmar contraseña</label>
                    <asp:TextBox ID="txtConfirmarContrasena" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" />
                </div>

                <div class="form-group">
                    <asp:CheckBox ID="chkEstado" runat="server" Text=" Usuario activo" Checked="true" />
                </div>
            </div>

            <p class="text-muted">
                Al editar un usuario, deje la contraseña vacía si no desea cambiarla.
            </p>

            <asp:Label ID="lblMensaje" runat="server" CssClass="validation-message" EnableViewState="false" />

            <div class="form-actions">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" CausesValidation="false" />
            </div>
        </section>

        <section class="table-panel">
            <h2>Usuarios registrados</h2>

            <asp:GridView
                ID="gvUsuarios"
                runat="server"
                CssClass="data-table"
                AutoGenerateColumns="False"
                EmptyDataText="No hay usuarios registrados.">

                <Columns>
                    <asp:BoundField DataField="IdUsuario" HeaderText="ID" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre completo" />
                    <asp:BoundField DataField="Correo" HeaderText="Correo" />
                    <asp:BoundField DataField="NombreRol" HeaderText="Rol" />
                    <asp:BoundField DataField="EstadoTexto" HeaderText="Estado" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lnkEditar"
                                runat="server"
                                Text="Editar"
                                CommandName="EditarUsuario"
                                CommandArgument='<%# Eval("IdUsuario") %>' />

                            <asp:LinkButton
                                ID="lnkCambiarEstado"
                                runat="server"
                                Text='<%# Eval("AccionEstadoTexto") %>'
                                CommandName="CambiarEstado"
                                CommandArgument='<%# Eval("IdUsuario") %>'
                                CssClass="danger-link"
                                OnClientClick="return confirm('¿Confirma cambiar el estado de este usuario?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </section>
    </main>
</asp:Content>