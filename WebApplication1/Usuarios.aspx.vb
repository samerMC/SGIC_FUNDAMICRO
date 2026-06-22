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
                MostrarExito("Usuario creado correctamente.")
            Else
                _usuarioDAL.Actualizar(usuario, cambiarPassword, SesionHelper.ObtenerIdUsuario())
                MostrarExito("Usuario actualizado correctamente.")
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

    ' Este evento maneja tanto la edición como el cambio de estado de los usuarios. Se identifica la acción a través del CommandName y se obtiene el IdUsuario desde CommandArgument.
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
            MostrarExito("Estado del usuario actualizado correctamente.")

        Catch ex As Exception
            MostrarMensaje("No fue posible cambiar el estado del usuario.")
        End Try
    End Sub

    ' Este método valida que los datos ingresados en el formulario sean correctos antes de intentar guardarlos en la base de datos.
    ' También maneja validaciones específicas para la contraseña solo si se está creando un nuevo usuario o si se ha ingresado una nueva contraseña para un usuario existente.
    Private Function DatosValidos() As Boolean
        Dim idUsuario As Integer = ObtenerIdUsuarioFormulario()
        Dim esNuevo As Boolean = idUsuario = 0
        Dim mensajeContrasena As String = String.Empty

        If Not ValidacionHelper.TextoObligatorio(txtNombreUsuario.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar el nombre de usuario.")
            Return False
        End If

        If Not ValidacionHelper.NombreUsuarioValido(txtNombreUsuario.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "El usuario debe tener entre 3 y 50 caracteres. Use solo letras, números, punto, guion o guion bajo.")
            Return False
        End If

        If Not ValidacionHelper.TextoObligatorio(txtNombreCompleto.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar el nombre completo.")
            Return False
        End If

        If Not ValidacionHelper.LongitudMaxima(txtNombreCompleto.Text, 150) Then
            MensajeHelper.Advertencia(lblMensaje, "El nombre completo no debe superar los 150 caracteres.")
            Return False
        End If

        If Not ValidacionHelper.CorreoValido(txtCorreo.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar un correo electrónico válido.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(ddlRol.SelectedValue) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe seleccionar un rol.")
            Return False
        End If

        If esNuevo AndAlso String.IsNullOrWhiteSpace(txtContrasena.Text) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar una contraseña para el nuevo usuario.")
            Return False
        End If

        If Not String.IsNullOrWhiteSpace(txtContrasena.Text) Then
            If Not ValidacionHelper.ContrasenaSegura(txtContrasena.Text, mensajeContrasena) Then
                MensajeHelper.Advertencia(lblMensaje, mensajeContrasena)
                Return False
            End If

            If txtContrasena.Text <> txtConfirmarContrasena.Text Then
                MensajeHelper.Advertencia(lblMensaje, "La contraseña y la confirmación no coinciden.")
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
        MensajeHelper.ErrorSistema(lblMensaje, mensaje)
    End Sub

    Private Sub MostrarExito(mensaje As String)
        MensajeHelper.Exito(lblMensaje, mensaje)
    End Sub

    Private Sub LimpiarMensaje()
        MensajeHelper.Limpiar(lblMensaje)
    End Sub

    Protected Sub btnConfirmarCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnConfirmarCambiarEstado.Click
        Dim idUsuario As Integer

        If Not Integer.TryParse(hfIdUsuarioCambiarEstado.Value, idUsuario) Then
            MostrarMensaje("No fue posible identificar el usuario seleccionado.")
            Return
        End If

        CambiarEstadoUsuario(idUsuario)
    End Sub

End Class