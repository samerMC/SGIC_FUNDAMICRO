Imports System.Web.UI.WebControls

Public Class Clientes
    Inherits System.Web.UI.Page

    Private ReadOnly _clienteDAL As New ClienteDAL()
    Private ReadOnly _bitacoraDAL As New BitacoraDAL()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not SesionHelper.ValidarAcceso() Then
            Return
        End If

        If Not IsPostBack Then
            CargarClientes()
            PrepararNuevoCliente()
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        LimpiarMensaje()

        If Not DatosValidos() Then
            Return
        End If

        Dim idCliente As Integer = ObtenerIdClienteFormulario()
        Dim esNuevo As Boolean = idCliente = 0

        Try
            If _clienteDAL.ExisteDui(txtDui.Text, idCliente) Then
                MostrarMensaje("Ya existe un cliente activo con el DUI ingresado.", True)
                Return
            End If

            Dim cliente As Cliente = CrearClienteDesdeFormulario(idCliente)

            If esNuevo Then
                Dim idClienteCreado As Integer = _clienteDAL.Insertar(cliente, SesionHelper.ObtenerIdUsuario())

                RegistrarBitacora(
                    "AGREGAR",
                    idClienteCreado,
                    String.Format("Se agregó el cliente {0}.", cliente.NombreCompleto)
                )

                MostrarMensaje("Cliente creado correctamente.", False)
            Else
                _clienteDAL.Actualizar(cliente, SesionHelper.ObtenerIdUsuario())

                RegistrarBitacora(
                    "EDITAR",
                    cliente.IdCliente,
                    String.Format("Se actualizó la información del cliente {0}.", cliente.NombreCompleto)
                )

                MostrarMensaje("Cliente actualizado correctamente.", False)
            End If

            CargarClientes()
            PrepararNuevoCliente()

        Catch ex As Exception
            MostrarMensaje("No fue posible guardar el cliente. Intente nuevamente.", True)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarMensaje()
        PrepararNuevoCliente()
    End Sub

    ' Este evento se dispara cuando se hace clic en los botones de editar o eliminar dentro del GridView. Se identifica el cliente seleccionado a través del CommandArgument, que contiene el IdCliente.
    Protected Sub gvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Dim idCliente As Integer

        If Not Integer.TryParse(e.CommandArgument.ToString(), idCliente) Then
            MostrarMensaje("No fue posible identificar el cliente seleccionado.", True)
            Return
        End If

        Select Case e.CommandName
            Case "EditarCliente"
                CargarClienteParaEdicion(idCliente)

            Case "EliminarCliente"
                EliminarCliente(idCliente)
        End Select
    End Sub

    Private Sub CargarClientes()
        gvClientes.DataSource = _clienteDAL.ListarActivos()
        gvClientes.DataBind()
    End Sub

    Private Sub CargarClienteParaEdicion(idCliente As Integer)
        Dim cliente As Cliente = _clienteDAL.ObtenerPorId(idCliente)

        If cliente Is Nothing OrElse Not cliente.Estado Then
            MostrarMensaje("El cliente seleccionado no existe o ya fue eliminado.", True)
            Return
        End If

        hfIdCliente.Value = cliente.IdCliente.ToString()
        txtNombre.Text = cliente.Nombre
        txtApellido.Text = cliente.Apellido
        txtDui.Text = cliente.Dui
        txtNit.Text = cliente.Nit
        txtTelefono.Text = cliente.Telefono
        txtCorreo.Text = cliente.Correo
        txtDireccion.Text = cliente.Direccion

        MostrarMensaje("Cliente cargado para edición.", False)
    End Sub

    Private Sub EliminarCliente(idCliente As Integer)
        Dim cliente As Cliente = _clienteDAL.ObtenerPorId(idCliente)

        If cliente Is Nothing OrElse Not cliente.Estado Then
            MostrarMensaje("El cliente seleccionado no existe o ya fue eliminado.", True)
            Return
        End If

        Try
            _clienteDAL.EliminarLogico(idCliente, SesionHelper.ObtenerIdUsuario())

            RegistrarBitacora(
                "ELIMINAR",
                cliente.IdCliente,
                String.Format("Se eliminó el cliente {0}.", cliente.NombreCompleto)
            )

            CargarClientes()
            PrepararNuevoCliente()

            MostrarMensaje("Cliente eliminado correctamente.", False)

        Catch ex As Exception
            MostrarMensaje("No fue posible eliminar el cliente. Intente nuevamente.", True)
        End Try
    End Sub

    Private Function DatosValidos() As Boolean
        If Not ValidacionHelper.TextoObligatorio(txtNombre.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar el nombre del cliente.")
            Return False
        End If

        If Not ValidacionHelper.TextoObligatorio(txtApellido.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar el apellido del cliente.")
            Return False
        End If

        If Not ValidacionHelper.LongitudMaxima(txtNombre.Text, 100) Then
            MensajeHelper.Advertencia(lblMensaje, "El nombre no debe superar los 100 caracteres.")
            Return False
        End If

        If Not ValidacionHelper.LongitudMaxima(txtApellido.Text, 100) Then
            MensajeHelper.Advertencia(lblMensaje, "El apellido no debe superar los 100 caracteres.")
            Return False
        End If

        If Not ValidacionHelper.DuiValido(txtDui.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "El DUI debe tener el formato 00000000-0.")
            Return False
        End If

        If Not ValidacionHelper.TelefonoValido(txtTelefono.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "El teléfono contiene caracteres no válidos.")
            Return False
        End If

        If Not ValidacionHelper.CorreoValido(txtCorreo.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar un correo electrónico válido.")
            Return False
        End If

        If Not ValidacionHelper.LongitudMaxima(txtDireccion.Text, 250) Then
            MensajeHelper.Advertencia(lblMensaje, "La dirección no debe superar los 250 caracteres.")
            Return False
        End If

        Return True
    End Function

    Private Function CrearClienteDesdeFormulario(idCliente As Integer) As Cliente
        Return New Cliente With {
            .IdCliente = idCliente,
            .Nombre = txtNombre.Text.Trim(),
            .Apellido = txtApellido.Text.Trim(),
            .Dui = txtDui.Text.Trim(),
            .Nit = txtNit.Text.Trim(),
            .Telefono = txtTelefono.Text.Trim(),
            .Correo = txtCorreo.Text.Trim(),
            .Direccion = txtDireccion.Text.Trim(),
            .Estado = True
        }
    End Function

    Private Function ObtenerIdClienteFormulario() As Integer
        Dim idCliente As Integer

        If Integer.TryParse(hfIdCliente.Value, idCliente) Then
            Return idCliente
        End If

        Return 0
    End Function

    ' Este método limpia los campos del formulario para preparar la creación de un nuevo cliente después de guardar o cancelar una edición.
    Private Sub PrepararNuevoCliente()
        hfIdCliente.Value = String.Empty
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtDui.Text = String.Empty
        txtNit.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtCorreo.Text = String.Empty
        txtDireccion.Text = String.Empty
    End Sub

    Private Sub MostrarMensaje(mensaje As String, esError As Boolean)
        If esError Then
            MensajeHelper.ErrorSistema(lblMensaje, mensaje)
        Else
            MensajeHelper.Exito(lblMensaje, mensaje)
        End If
    End Sub

    Private Sub LimpiarMensaje()
        MensajeHelper.Limpiar(lblMensaje)
    End Sub

    Private Sub RegistrarBitacora(accion As String, idCliente As Integer, detalle As String)
        Dim bitacora As New Bitacora With {
            .Accion = accion,
            .IdCliente = idCliente,
            .IdUsuario = SesionHelper.ObtenerIdUsuario(),
            .NombreUsuario = SesionHelper.ObtenerNombreUsuario(),
            .Detalle = detalle,
            .IpOrigen = ObtenerIpOrigen()
        }

        _bitacoraDAL.Registrar(bitacora)
    End Sub

    Private Function ObtenerIpOrigen() As String
        Dim ipProxy As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")

        If Not String.IsNullOrWhiteSpace(ipProxy) Then
            Return ipProxy.Split(","c)(0).Trim()
        End If

        Return Request.UserHostAddress
    End Function

    Protected Sub btnConfirmarEliminar_Click(sender As Object, e As EventArgs) Handles btnConfirmarEliminar.Click
        Dim idCliente As Integer

        If Not Integer.TryParse(hfIdClienteEliminar.Value, idCliente) Then
            MostrarMensaje("No fue posible identificar el cliente seleccionado.", True)
            Return
        End If

        EliminarCliente(idCliente)
    End Sub

End Class