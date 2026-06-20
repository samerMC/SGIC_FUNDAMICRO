<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Bitacoras.aspx.vb" Inherits="WebApplication1.Bitacoras" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="page-container">
        <section class="page-header">
            <h1>Bitácora de acciones</h1>
            <p>
                Consulte el historial de acciones realizadas sobre los registros de clientes.
            </p>
        </section>

        <section class="table-panel">
            <asp:GridView 
                ID="gvBitacora" 
                runat="server" 
                CssClass="data-table" 
                AutoGenerateColumns="False" 
                EmptyDataText="No hay acciones registradas.">

                <Columns>
                    <asp:BoundField DataField="IdBitacora" HeaderText="ID" />
                    <asp:BoundField DataField="Accion" HeaderText="Acción" />
                    <asp:BoundField DataField="IdCliente" HeaderText="ID Cliente" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="FechaHora" HeaderText="Fecha y hora" />
                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                </Columns>
            </asp:GridView>
        </section>
    </main>
</asp:Content>
