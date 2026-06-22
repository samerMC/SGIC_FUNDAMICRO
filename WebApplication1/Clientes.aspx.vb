Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls

Public Class Clientes
    Inherits System.Web.UI.Page

    Private ReadOnly _clienteDAL As New ClienteDAL()

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
                _clienteDAL.Insertar(cliente, SesionHelper.ObtenerIdUsuario())
                MostrarMensaje("Cliente creado correctamente.", False)
            Else
                _clienteDAL.Actualizar(cliente, SesionHelper.ObtenerIdUsuario())
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

            CargarClientes()
            PrepararNuevoCliente()

            MostrarMensaje("Cliente eliminado correctamente.", False)

        Catch ex As Exception
            MostrarMensaje("No fue posible eliminar el cliente. Intente nuevamente.", True)
        End Try
    End Sub

    Private Function DatosValidos() As Boolean
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MostrarMensaje("Debe ingresar el nombre del cliente.", True)
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtApellido.Text) Then
            MostrarMensaje("Debe ingresar el apellido del cliente.", True)
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(txtDui.Text) AndAlso Not DuiValido(txtDui.Text) Then
            MostrarMensaje("El DUI debe tener el formato 00000000-0.", True)
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(txtCorreo.Text) AndAlso Not CorreoValido(txtCorreo.Text) Then
            MostrarMensaje("Debe ingresar un correo electrónico válido.", True)
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(txtTelefono.Text) AndAlso Not TelefonoValido(txtTelefono.Text) Then
            MostrarMensaje("El teléfono contiene caracteres no válidos.", True)
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

    Private Function DuiValido(dui As String) As Boolean
        Return Regex.IsMatch(dui.Trim(), "^\d{8}-\d$")
    End Function

    Private Function CorreoValido(correo As String) As Boolean
        Return Regex.IsMatch(correo.Trim(), "^[^@\s]+@[^@\s]+\.[^@\s]+$")
    End Function

    Private Function TelefonoValido(telefono As String) As Boolean
        Return Regex.IsMatch(telefono.Trim(), "^[0-9+\-\s]{7,20}$")
    End Function

    Private Sub MostrarMensaje(mensaje As String, esError As Boolean)
        lblMensaje.Text = mensaje
        lblMensaje.CssClass = If(esError, "validation-message", "success-message")
    End Sub

    Private Sub LimpiarMensaje()
        lblMensaje.Text = String.Empty
        lblMensaje.CssClass = "validation-message"
    End Sub

End Class