Imports System.Text.RegularExpressions

Public NotInheritable Class ValidacionHelper

    Private Sub New()
    End Sub

    Public Shared Function TextoObligatorio(valor As String) As Boolean
        Return Not String.IsNullOrWhiteSpace(valor)
    End Function

    Public Shared Function LongitudMaxima(valor As String, longitudMax As Integer) As Boolean
        If valor Is Nothing Then
            Return True
        End If

        Return valor.Trim().Length <= longitudMax
    End Function

    Public Shared Function NombreUsuarioValido(nombreUsuario As String) As Boolean
        If String.IsNullOrWhiteSpace(nombreUsuario) Then
            Return False
        End If

        ' Permite letras, números, punto, guion y guion bajo.
        Return Regex.IsMatch(nombreUsuario.Trim(), "^[A-Za-z0-9._-]{3,50}$")
    End Function

    Public Shared Function CorreoValido(correo As String) As Boolean
        If String.IsNullOrWhiteSpace(correo) Then
            Return True
        End If

        Return Regex.IsMatch(correo.Trim(), "^[^@\s]+@[^@\s]+\.[^@\s]+$")
    End Function

    Public Shared Function DuiValido(dui As String) As Boolean
        If String.IsNullOrWhiteSpace(dui) Then
            Return True
        End If

        Return Regex.IsMatch(dui.Trim(), "^\d{8}-\d$")
    End Function

    Public Shared Function TelefonoValido(telefono As String) As Boolean
        If String.IsNullOrWhiteSpace(telefono) Then
            Return True
        End If

        Return Regex.IsMatch(telefono.Trim(), "^[0-9+\-\s]{7,20}$")
    End Function

    Public Shared Function ContrasenaSegura(contrasena As String, ByRef mensaje As String) As Boolean
        mensaje = String.Empty

        If String.IsNullOrWhiteSpace(contrasena) Then
            mensaje = "Debe ingresar una contraseña."
            Return False
        End If

        If contrasena.Length < 8 Then
            mensaje = "La contraseña debe tener al menos 8 caracteres."
            Return False
        End If

        If Not Regex.IsMatch(contrasena, "[A-Z]") Then
            mensaje = "La contraseña debe incluir al menos una letra mayúscula."
            Return False
        End If

        If Not Regex.IsMatch(contrasena, "[a-z]") Then
            mensaje = "La contraseña debe incluir al menos una letra minúscula."
            Return False
        End If

        If Not Regex.IsMatch(contrasena, "\d") Then
            mensaje = "La contraseña debe incluir al menos un número."
            Return False
        End If

        Return True
    End Function

    Public Shared Function TryObtenerFechaOpcional(valor As String, ByRef fecha As DateTime?) As Boolean
        fecha = Nothing

        If String.IsNullOrWhiteSpace(valor) Then
            Return True
        End If

        Dim fechaTemporal As DateTime

        If DateTime.TryParse(valor, fechaTemporal) Then
            fecha = fechaTemporal
            Return True
        End If

        Return False
    End Function

End Class