<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Inicio.aspx.vb" Inherits="WebApplication1.Inicio" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="page-container">
        <section class="page-header">
            <h1>Panel principal</h1>
            <p>
                Desde esta pantalla podrá acceder a las opciones principales del sistema.
            </p>
        </section>

        <section class="dashboard-grid">
            <article class="dashboard-card">
                <h2>Clientes</h2>
                <p>Permite consultar, registrar, editar y eliminar información de clientes.</p>
                <a class="btn btn-primary" href="<%= ResolveUrl("~/Clientes.aspx") %>">
                    Ir a clientes
                </a>
            </article>

            <article class="dashboard-card">
                <h2>Bitácora</h2>
                <p>Permite consultar las acciones realizadas sobre los registros de clientes.</p>
                <a class="btn btn-secondary" href="<%= ResolveUrl("~/Bitacoras.aspx") %>">
                    Ver bitácora
                </a>
            </article>
        </section>
    </main>
</asp:Content>
