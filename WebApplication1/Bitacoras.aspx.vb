Public Class Bitacoras
    Inherits System.Web.UI.Page

    Private ReadOnly _bitacoraDAL As New BitacoraDAL()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not SesionHelper.ValidarAcceso() Then
            Return
        End If

        If Not IsPostBack Then
            CargarBitacoras()
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        LimpiarMensaje()

        If Not FiltrosValidos() Then
            Return
        End If

        CargarBitacoras()
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        LimpiarMensaje()
        LimpiarFiltros()
        CargarBitacoras()
    End Sub

    Private Sub CargarBitacoras()
        Try
            Dim fechaDesde As DateTime? = ObtenerFecha(txtFechaDesde.Text)
            Dim fechaHasta As DateTime? = ObtenerFecha(txtFechaHasta.Text)
            Dim accion As String = ddlAccion.SelectedValue
            Dim usuario As String = txtUsuarioFiltro.Text.Trim()

            gvBitacoras.DataSource = _bitacoraDAL.Listar(fechaDesde, fechaHasta, accion, usuario)
            gvBitacoras.DataBind()

        Catch ex As Exception
            MostrarMensaje("No fue posible cargar la bitácora. Intente nuevamente.")
        End Try
    End Sub

    Private Function FiltrosValidos() As Boolean
        Dim fechaDesde As DateTime? = ObtenerFecha(txtFechaDesde.Text)
        Dim fechaHasta As DateTime? = ObtenerFecha(txtFechaHasta.Text)

        If Not String.IsNullOrWhiteSpace(txtFechaDesde.Text) AndAlso Not fechaDesde.HasValue Then
            MostrarMensaje("La fecha desde no tiene un formato válido.")
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(txtFechaHasta.Text) AndAlso Not fechaHasta.HasValue Then
            MostrarMensaje("La fecha hasta no tiene un formato válido.")
            Return False
        End If

        If fechaDesde.HasValue AndAlso fechaHasta.HasValue AndAlso fechaDesde.Value.Date > fechaHasta.Value.Date Then
            MostrarMensaje("La fecha desde no puede ser mayor que la fecha hasta.")
            Return False
        End If

        Return True
    End Function

    Private Function ObtenerFecha(valor As String) As DateTime?
        If String.IsNullOrWhiteSpace(valor) Then
            Return Nothing
        End If

        Dim fecha As DateTime

        If DateTime.TryParse(valor, fecha) Then
            Return fecha
        End If

        Return Nothing
    End Function

    Private Sub LimpiarFiltros()
        txtFechaDesde.Text = String.Empty
        txtFechaHasta.Text = String.Empty
        ddlAccion.SelectedValue = String.Empty
        txtUsuarioFiltro.Text = String.Empty
    End Sub

    Private Sub MostrarMensaje(mensaje As String)
        lblMensaje.Text = mensaje
    End Sub

    Private Sub LimpiarMensaje()
        lblMensaje.Text = String.Empty
    End Sub

End Class