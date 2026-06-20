<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="WebApplication1.Login" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="page-container">
        <section class="login-wrapper">
            <div class="login-card">
                <h1>Inicio de sesión</h1>
                <p class="text-muted">
                    Ingrese sus credenciales para acceder al sistema de gestión de clientes.
                </p>

                <div class="form-group">
                    <label for="txtUsuario">Usuario</label>
                    <asp:TextBox 
                        ID="txtUsuario" 
                        runat="server" 
                        CssClass="form-control" 
                        MaxLength="50" 
                        autocomplete="username" />
                </div>

                <div class="form-group">
                    <label for="txtContrasena">Contraseña</label>
                    <asp:TextBox 
                        ID="txtContrasena" 
                        runat="server" 
                        CssClass="form-control" 
                        TextMode="Password" 
                        MaxLength="100" 
                        autocomplete="current-password" />
                </div>

                <asp:Label 
                    ID="lblMensaje" 
                    runat="server" 
                    CssClass="validation-message" 
                    EnableViewState="false" />

                <div class="form-actions">
                    <asp:Button 
                        ID="btnIngresar" 
                        runat="server" 
                        Text="Ingresar" 
                        CssClass="btn btn-primary" />
                </div>
            </div>
        </section>
    </main>
</asp:Content>
