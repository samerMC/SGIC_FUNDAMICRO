Public Class Usuarios
    Inherits System.Web.UI.Page

    Private ReadOnly _usuarioDAL As New UsuarioMantenimientoDAL()
    Private ReadOnly _rolDAL As New RolDAL()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not SesionHelper.ValidarAccesoAdministrador() Then
            Return
        End If

        If Not IsPostBack Then
            CargarRoles()
            CargarUsuarios()
            PrepararNuevoUsuario()
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        LimpiarMensaje()

        If Not DatosValidos() Then
            Return
        End If

        Dim idUsuario As Integer = ObtenerIdUsuarioFormulario()
        Dim esNuevo As Boolean = idUsuario = 0
        Dim cambiarPassword As Boolean = Not String.IsNullOrWhiteSpace(txtContrasena.Text)

        Try
            If _usuarioDAL.ExisteNombreUsuario(txtNombreUsuario.Text, idUsuario) Then
                MostrarMensaje("Ya existe un usuario con ese nombre.")
                Return
            End If

            If _usuarioDAL.ExisteCorreo(txtCorreo.Text, idUsuario) Then
                MostrarMensaje("Ya existe un usuario con ese correo.")
                Return
            End If

            Dim usuario As Usuario = CrearUsuarioDesdeFormulario(idUsuario)

            If esNuevo OrElse cambiarPassword Then
                usuario.PasswordSalt = PasswordHelper.GenerarSalt()
                usuario.PasswordHash = PasswordHelper.GenerarHash(txtContrasena.Text, usuario.PasswordSalt)
            End If

            If esNuevo Then
                _usuarioDAL.Insertar(usuario, SesionHelper.ObtenerIdUsuario())
                MostrarMensaje("Usuario creado correctamente.")
            Else
                _usuarioDAL.Actualizar(usuario, cambiarPassword, SesionHelper.ObtenerIdUsuario())
                MostrarMensaje("Usuario actualizado correctamente.")
            End If

            CargarUsuarios()
            PrepararNuevoUsuario()

        Catch ex As Exception
            MostrarMensaje("No fue posible guardar el usuario. Intente nuevamente.")
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarMensaje()
        PrepararNuevoUsuario()
    End Sub

    Protected Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        Dim idUsuario As Integer

        If Not Integer.TryParse(e.CommandArgument.ToString(), idUsuario) Then
            MostrarMensaje("No fue posible identificar el usuario seleccionado.")
            Return
        End If

        Select Case e.CommandName
            Case "EditarUsuario"
                CargarUsuarioParaEdicion(idUsuario)

            Case "CambiarEstado"
                CambiarEstadoUsuario(idUsuario)
        End Select
    End Sub

    Private Sub CargarRoles()
        ddlRol.DataSource = _rolDAL.ListarActivos()
        ddlRol.DataTextField = "NombreRol"
        ddlRol.DataValueField = "IdRol"
        ddlRol.DataBind()
    End Sub

    Private Sub CargarUsuarios()
        gvUsuarios.DataSource = _usuarioDAL.Listar()
        gvUsuarios.DataBind()
    End Sub

    Private Sub CargarUsuarioParaEdicion(idUsuario As Integer)
        Dim usuario As Usuario = _usuarioDAL.ObtenerPorId(idUsuario)

        If usuario Is Nothing Then
            MostrarMensaje("El usuario seleccionado no existe.")
            Return
        End If

        hfIdUsuario.Value = usuario.IdUsuario.ToString()
        txtNombreUsuario.Text = usuario.NombreUsuario
        txtNombreCompleto.Text = usuario.NombreCompleto
        txtCorreo.Text = usuario.Correo
        ddlRol.SelectedValue = usuario.IdRol.ToString()
        chkEstado.Checked = usuario.Estado
        txtContrasena.Text = String.Empty
        txtConfirmarContrasena.Text = String.Empty
    End Sub

    Private Sub CambiarEstadoUsuario(idUsuario As Integer)
        Dim usuario As Usuario = _usuarioDAL.ObtenerPorId(idUsuario)

        If usuario Is Nothing Then
            MostrarMensaje("El usuario seleccionado no existe.")
            Return
        End If

        If usuario.IdUsuario = SesionHelper.ObtenerIdUsuario() Then
            MostrarMensaje("No puede cambiar el estado del usuario con sesión activa.")
            Return
        End If

        If usuario.Estado AndAlso usuario.NombreRol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) Then
            If _usuarioDAL.ContarAdministradoresActivos() <= 1 Then
                MostrarMensaje("No puede desactivar el último administrador activo.")
                Return
            End If
        End If

        Try
            _usuarioDAL.CambiarEstado(usuario.IdUsuario, Not usuario.Estado, SesionHelper.ObtenerIdUsuario())
            CargarUsuarios()
            MostrarMensaje("Estado del usuario actualizado correctamente.")

        Catch ex As Exception
            MostrarMensaje("No fue posible cambiar el estado del usuario.")
        End Try
    End Sub

    Private Function DatosValidos() As Boolean
        Dim idUsuario As Integer = ObtenerIdUsuarioFormulario()
        Dim esNuevo As Boolean = idUsuario = 0

        If String.IsNullOrWhiteSpace(txtNombreUsuario.Text) Then
            MostrarMensaje("Debe ingresar el nombre de usuario.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtNombreCompleto.Text) Then
            MostrarMensaje("Debe ingresar el nombre completo.")
            Return False
        End If

        If ddlRol.SelectedValue = String.Empty Then
            MostrarMensaje("Debe seleccionar un rol.")
            Return False
        End If

        If esNuevo AndAlso String.IsNullOrWhiteSpace(txtContrasena.Text) Then
            MostrarMensaje("Debe ingresar una contraseña para el nuevo usuario.")
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(txtContrasena.Text) Then
            If txtContrasena.Text.Length < 8 Then
                MostrarMensaje("La contraseña debe tener al menos 8 caracteres.")
                Return False
            End If

            If txtContrasena.Text <> txtConfirmarContrasena.Text Then
                MostrarMensaje("La contraseña y la confirmación no coinciden.")
                Return False
            End If
        End If

        Return True
    End Function

    Private Function CrearUsuarioDesdeFormulario(idUsuario As Integer) As Usuario
        Return New Usuario With {
            .IdUsuario = idUsuario,
            .IdRol = Convert.ToInt32(ddlRol.SelectedValue),
            .NombreUsuario = txtNombreUsuario.Text.Trim(),
            .NombreCompleto = txtNombreCompleto.Text.Trim(),
            .Correo = txtCorreo.Text.Trim(),
            .Estado = chkEstado.Checked
        }
    End Function

    Private Function ObtenerIdUsuarioFormulario() As Integer
        Dim idUsuario As Integer

        If Integer.TryParse(hfIdUsuario.Value, idUsuario) Then
            Return idUsuario
        End If

        Return 0
    End Function

    Private Sub PrepararNuevoUsuario()
        hfIdUsuario.Value = String.Empty
        txtNombreUsuario.Text = String.Empty
        txtNombreCompleto.Text = String.Empty
        txtCorreo.Text = String.Empty
        txtContrasena.Text = String.Empty
        txtConfirmarContrasena.Text = String.Empty
        chkEstado.Checked = True

        If ddlRol.Items.Count > 0 Then
            ddlRol.SelectedIndex = 0
        End If
    End Sub

    Private Sub MostrarMensaje(mensaje As String)
        lblMensaje.Text = mensaje
    End Sub

    Private Sub LimpiarMensaje()
        lblMensaje.Text = String.Empty
    End Sub

End Class