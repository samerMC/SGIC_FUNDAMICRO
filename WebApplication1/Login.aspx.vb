Public Class Login
    Inherits System.Web.UI.Page

    Private ReadOnly _usuarioDAL As New UsuarioDAL()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack AndAlso SesionHelper.ExisteSesionActiva() Then
            Response.Redirect("~/Inicio.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End If
    End Sub

    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        LimpiarMensaje()

        Dim nombreUsuario As String = txtUsuario.Text.Trim()
        Dim contrasena As String = txtContrasena.Text

        If String.IsNullOrWhiteSpace(nombreUsuario) OrElse String.IsNullOrWhiteSpace(contrasena) Then
            MostrarMensaje("Debe ingresar usuario y contraseña.")
            Return
        End If

        Try
            Dim usuario As Usuario = _usuarioDAL.ObtenerPorNombreUsuario(nombreUsuario)

            If Not CredencialesValidas(usuario, contrasena) Then
                MostrarMensaje("Usuario o contraseña incorrectos.")
                Return
            End If

            _usuarioDAL.ActualizarUltimoAcceso(usuario.IdUsuario)
            SesionHelper.CrearSesion(usuario)

            Response.Redirect("~/Inicio.aspx", False)
            Context.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            MostrarMensaje("No fue posible iniciar sesión. Intente nuevamente.")
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

    Private Sub MostrarMensaje(mensaje As String)
        lblMensaje.Text = mensaje
    End Sub

    Private Sub LimpiarMensaje()
        lblMensaje.Text = String.Empty
    End Sub

End Class