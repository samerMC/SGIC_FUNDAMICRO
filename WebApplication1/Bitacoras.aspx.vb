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
            Dim fechaDesde As DateTime? = Nothing
            Dim fechaHasta As DateTime? = Nothing

            ValidacionHelper.TryObtenerFechaOpcional(txtFechaDesde.Text, fechaDesde)
            ValidacionHelper.TryObtenerFechaOpcional(txtFechaHasta.Text, fechaHasta)

            Dim accion As String = ddlAccion.SelectedValue
            Dim usuario As String = txtUsuarioFiltro.Text.Trim()

            gvBitacoras.DataSource = _bitacoraDAL.Listar(fechaDesde, fechaHasta, accion, usuario)
            gvBitacoras.DataBind()

        Catch ex As Exception
            MensajeHelper.ErrorSistema(lblMensaje, "No fue posible cargar la bitácora. Intente nuevamente.")
        End Try
    End Sub

    ' Este método valida que las fechas ingresadas sean correctas y que la fecha desde no sea mayor que la fecha hasta.
    Private Function FiltrosValidos() As Boolean
        Dim fechaDesde As DateTime? = Nothing
        Dim fechaHasta As DateTime? = Nothing

        If Not ValidacionHelper.TryObtenerFechaOpcional(txtFechaDesde.Text, fechaDesde) Then
            MensajeHelper.Advertencia(lblMensaje, "La fecha desde no tiene un formato válido.")
            Return False
        End If

        If Not ValidacionHelper.TryObtenerFechaOpcional(txtFechaHasta.Text, fechaHasta) Then
            MensajeHelper.Advertencia(lblMensaje, "La fecha hasta no tiene un formato válido.")
            Return False
        End If

        If fechaDesde.HasValue AndAlso fechaHasta.HasValue AndAlso fechaDesde.Value.Date > fechaHasta.Value.Date Then
            MensajeHelper.Advertencia(lblMensaje, "La fecha desde no puede ser mayor que la fecha hasta.")
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFiltros()
        txtFechaDesde.Text = String.Empty
        txtFechaHasta.Text = String.Empty
        ddlAccion.SelectedValue = String.Empty
        txtUsuarioFiltro.Text = String.Empty
    End Sub

    Private Sub MostrarMensaje(mensaje As String)
        MensajeHelper.ErrorSistema(lblMensaje, mensaje)
    End Sub

    Private Sub LimpiarMensaje()
        MensajeHelper.Limpiar(lblMensaje)
    End Sub

End Class