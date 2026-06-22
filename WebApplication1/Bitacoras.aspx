<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Bitacoras.aspx.vb" Inherits="WebApplication1.Bitacoras" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="page-container">
        <section class="page-header">
            <h1>Bitácoras de acciones</h1>
            <p>
                Consulte el historial de acciones realizadas sobre los registros de clientes.
            </p>
        </section>

        <section class="form-panel">
            <h2>Filtros de búsqueda</h2>

            <div class="form-grid">
                <div class="form-group">
                    <label for="txtFechaDesde">Fecha desde</label>
                    <asp:TextBox 
                        ID="txtFechaDesde" 
                        runat="server" 
                        CssClass="form-control" 
                        TextMode="Date" />
                </div>

                <div class="form-group">
                    <label for="txtFechaHasta">Fecha hasta</label>
                    <asp:TextBox 
                        ID="txtFechaHasta" 
                        runat="server" 
                        CssClass="form-control" 
                        TextMode="Date" />
                </div>

                <div class="form-group">
                    <label for="ddlAccion">Acción</label>
                    <asp:DropDownList ID="ddlAccion" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Todas" Value="" />
                        <asp:ListItem Text="Agregar" Value="AGREGAR" />
                        <asp:ListItem Text="Editar" Value="EDITAR" />
                        <asp:ListItem Text="Eliminar" Value="ELIMINAR" />
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="txtUsuarioFiltro">Usuario</label>
                    <asp:TextBox 
                        ID="txtUsuarioFiltro" 
                        runat="server" 
                        CssClass="form-control" 
                        MaxLength="50" />
                </div>
            </div>

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="validation-message"
                EnableViewState="false" />

            <div class="form-actions">
                <asp:Button 
                    ID="btnBuscar" 
                    runat="server" 
                    Text="Buscar" 
                    CssClass="btn btn-primary" />

                <asp:Button 
                    ID="btnLimpiar" 
                    runat="server" 
                    Text="Limpiar" 
                    CssClass="btn btn-secondary" 
                    CausesValidation="false" />
            </div>
        </section>

        <section class="table-panel">
            <h2>Historial registrado</h2>

            <asp:GridView
                ID="gvBitacoras"
                runat="server"
                CssClass="data-table"
                AutoGenerateColumns="False"
                EmptyDataText="No hay acciones registradas.">

                <Columns>
                    <asp:BoundField DataField="IdBitacora" HeaderText="ID" />
                    <asp:BoundField DataField="Accion" HeaderText="Acción" />
                    <asp:BoundField DataField="IdCliente" HeaderText="ID Cliente" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField 
                        DataField="FechaHora" 
                        HeaderText="Fecha y hora" 
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                    <asp:BoundField DataField="IpOrigen" HeaderText="IP" />
                </Columns>
            </asp:GridView>
        </section>
    </main>
</asp:Content>
