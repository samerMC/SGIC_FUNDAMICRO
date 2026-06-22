Public Class Login
    Inherits System.Web.UI.Page

    Private ReadOnly _usuarioDAL As New UsuarioDAL()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MensajeHelper.Limpiar(lblMensaje)
            txtUsuario.Focus()

            If SesionHelper.ExisteSesionActiva() Then
                Response.Redirect("~/Inicio.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
    End Sub

    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        MensajeHelper.Limpiar(lblMensaje)

        Dim nombreUsuario As String = txtUsuario.Text.Trim()
        Dim contrasena As String = txtContrasena.Text

        If Not ValidacionHelper.TextoObligatorio(nombreUsuario) OrElse Not ValidacionHelper.TextoObligatorio(contrasena) Then
            MensajeHelper.Advertencia(lblMensaje, "Debe ingresar usuario y contraseña.")
            Return
        End If

        Try
            Dim usuario As Usuario = _usuarioDAL.ObtenerPorNombreUsuario(nombreUsuario)

            If Not CredencialesValidas(usuario, contrasena) Then
                MensajeHelper.ErrorSistema(lblMensaje, "Usuario o contraseña incorrectos.")
                Return
            End If

            _usuarioDAL.ActualizarUltimoAcceso(usuario.IdUsuario)
            SesionHelper.CrearSesion(usuario)

            Response.Redirect("~/Inicio.aspx", False)
            Context.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            MensajeHelper.ErrorSistema(lblMensaje, "No fue posible iniciar sesión. Intente nuevamente.")

            ' No se muestra el detalle técnico del error por seguridad.
        End Try
    End Sub

    Private Function CredencialesValidas(usuario As Usuario, contrasena As String) As Boolean
        If usuario Is Nothing Then
            Return False
        End If

        If Not usuario.Estado Then
            Return False
        End If

        Return PasswordHelper.ValidarContrasena(contrasena, usuario.PasswordSalt, usuario.PasswordHash)
    End Function

End Class