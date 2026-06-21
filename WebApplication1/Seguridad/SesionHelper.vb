Imports System.Web

Public NotInheritable Class SesionHelper

    Private Const ClaveIdUsuario As String = "IdUsuario"
    Private Const ClaveNombreUsuario As String = "NombreUsuario"
    Private Const ClaveNombreCompleto As String = "NombreCompleto"
    Private Const ClaveIdRol As String = "IdRol"
    Private Const ClaveNombreRol As String = "NombreRol"

    Private Sub New()
        ' Evita crear instancias de esta clase.
    End Sub

    Public Shared Sub CrearSesion(usuario As Usuario)
        If usuario Is Nothing Then
            Throw New ArgumentNullException(NameOf(usuario))
        End If

        Dim contexto As HttpContext = HttpContext.Current

        contexto.Session(ClaveIdUsuario) = usuario.IdUsuario
        contexto.Session(ClaveNombreUsuario) = usuario.NombreUsuario
        contexto.Session(ClaveNombreCompleto) = usuario.NombreCompleto
        contexto.Session(ClaveIdRol) = usuario.IdRol
        contexto.Session(ClaveNombreRol) = usuario.NombreRol
    End Sub

    Public Shared Function ExisteSesionActiva() As Boolean
        Dim contexto As HttpContext = HttpContext.Current

        If contexto Is Nothing OrElse contexto.Session Is Nothing Then
            Return False
        End If

        Return contexto.Session(ClaveIdUsuario) IsNot Nothing
    End Function

    Public Shared Function ValidarAcceso() As Boolean
        If ExisteSesionActiva() Then
            Return True
        End If

        Dim contexto As HttpContext = HttpContext.Current

        contexto.Response.Redirect("~/Login.aspx", False)
        contexto.ApplicationInstance.CompleteRequest()

        Return False
    End Function

    Public Shared Function ObtenerIdUsuario() As Integer
        Dim contexto As HttpContext = HttpContext.Current

        If contexto Is Nothing OrElse contexto.Session Is Nothing OrElse contexto.Session(ClaveIdUsuario) Is Nothing Then
            Return 0
        End If

        Dim idUsuario As Integer

        If Integer.TryParse(contexto.Session(ClaveIdUsuario).ToString(), idUsuario) Then
            Return idUsuario
        End If

        Return 0
    End Function

    Public Shared Function ObtenerNombreUsuario() As String
        Return ObtenerValorSesion(ClaveNombreUsuario)
    End Function

    Public Shared Function ObtenerNombreCompleto() As String
        Return ObtenerValorSesion(ClaveNombreCompleto)
    End Function

    Public Shared Sub CerrarSesion()
        Dim contexto As HttpContext = HttpContext.Current

        If contexto Is Nothing OrElse contexto.Session Is Nothing Then
            Return
        End If

        contexto.Session.Clear()
        contexto.Session.Abandon()

        If contexto.Request.Cookies("ASP.NET_SessionId") IsNot Nothing Then
            contexto.Response.Cookies("ASP.NET_SessionId").Expires = DateTime.Now.AddDays(-1)
        End If
    End Sub

    Private Shared Function ObtenerValorSesion(clave As String) As String
        Dim contexto As HttpContext = HttpContext.Current

        If contexto Is Nothing OrElse contexto.Session Is Nothing OrElse contexto.Session(clave) Is Nothing Then
            Return String.Empty
        End If

        Return contexto.Session(clave).ToString()
    End Function

    Public Shared Function ObtenerIdRol() As Integer
        Dim contexto As HttpContext = HttpContext.Current

        If contexto Is Nothing OrElse contexto.Session Is Nothing OrElse contexto.Session(ClaveIdRol) Is Nothing Then
            Return 0
        End If

        Dim idRol As Integer

        If Integer.TryParse(contexto.Session(ClaveIdRol).ToString(), idRol) Then
            Return idRol
        End If

        Return 0
    End Function

    Public Shared Function ObtenerNombreRol() As String
        Return ObtenerValorSesion(ClaveNombreRol)
    End Function

    Public Shared Function UsuarioEsAdministrador() As Boolean
        Return String.Equals(ObtenerNombreRol(), "Administrador", StringComparison.OrdinalIgnoreCase)
    End Function

    Public Shared Function ValidarAccesoAdministrador() As Boolean
        If Not ValidarAcceso() Then
            Return False
        End If

        If UsuarioEsAdministrador() Then
            Return True
        End If

        Dim contexto As HttpContext = HttpContext.Current

        contexto.Response.Redirect("~/Inicio.aspx", False)
        contexto.ApplicationInstance.CompleteRequest()

        Return False
    End Function

End Class