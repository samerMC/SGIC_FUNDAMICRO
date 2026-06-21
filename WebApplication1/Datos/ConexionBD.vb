Imports System.Configuration
Imports System.Data.SqlClient

Public NotInheritable Class ConexionBD

    Private Sub New()
        ' Evita crear instancias de esta clase.
    End Sub

    Public Shared Function ObtenerConexion() As SqlConnection
        Dim configuracion = ConfigurationManager.ConnectionStrings("ConexionDB")

        If configuracion Is Nothing OrElse String.IsNullOrWhiteSpace(configuracion.ConnectionString) Then
            Throw New ConfigurationErrorsException("No se encontró la cadena de conexión 'ConexionDB' en Web.config.")
        End If

        Return New SqlConnection(configuracion.ConnectionString)
    End Function

End Class