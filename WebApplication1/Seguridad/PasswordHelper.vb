Imports System.Security.Cryptography
Imports System.Text

Public NotInheritable Class PasswordHelper

    Private Sub New()
        ' Evita crear instancias de esta clase.
    End Sub

    Public Shared Function GenerarHash(contrasena As String, salt As String) As String
        If contrasena Is Nothing Then
            contrasena = String.Empty
        End If

        If salt Is Nothing Then
            salt = String.Empty
        End If

        Dim textoBase As String = salt & contrasena
        Dim bytesTexto As Byte() = Encoding.UTF8.GetBytes(textoBase)

        Using sha256 As SHA256 = SHA256.Create()
            Dim bytesHash As Byte() = sha256.ComputeHash(bytesTexto)

            Dim resultado As New StringBuilder()

            For Each byteHash As Byte In bytesHash
                resultado.Append(byteHash.ToString("X2"))
            Next

            Return resultado.ToString()
        End Using
    End Function

    Public Shared Function ValidarContrasena(contrasenaIngresada As String, salt As String, hashAlmacenado As String) As Boolean
        If String.IsNullOrWhiteSpace(hashAlmacenado) OrElse salt Is Nothing Then
            Return False
        End If

        Dim hashCalculado As String = GenerarHash(contrasenaIngresada, salt)

        Return CompararTextoSeguro(hashCalculado, hashAlmacenado)
    End Function

    Private Shared Function CompararTextoSeguro(valorA As String, valorB As String) As Boolean
        If valorA Is Nothing OrElse valorB Is Nothing Then
            Return False
        End If

        Dim textoA As String = valorA.ToUpperInvariant()
        Dim textoB As String = valorB.ToUpperInvariant()

        Dim bytesA As Byte() = Encoding.UTF8.GetBytes(textoA)
        Dim bytesB As Byte() = Encoding.UTF8.GetBytes(textoB)

        Dim longitudMaxima As Integer = Math.Max(bytesA.Length, bytesB.Length)
        Dim diferencia As Integer = bytesA.Length Xor bytesB.Length

        For indice As Integer = 0 To longitudMaxima - 1
            Dim byteA As Byte = If(indice < bytesA.Length, bytesA(indice), CByte(0))
            Dim byteB As Byte = If(indice < bytesB.Length, bytesB(indice), CByte(0))

            diferencia = diferencia Or (byteA Xor byteB)
        Next

        Return diferencia = 0
    End Function

    Public Shared Function GenerarSalt() As String
        Dim bytesSalt(31) As Byte

        Using generador As RandomNumberGenerator = RandomNumberGenerator.Create()
            generador.GetBytes(bytesSalt)
        End Using

        Return Convert.ToBase64String(bytesSalt)
    End Function

End Class