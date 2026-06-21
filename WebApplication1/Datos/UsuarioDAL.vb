Imports System.Data
Imports System.Data.SqlClient

Public Class UsuarioDAL

    Public Function ObtenerPorNombreUsuario(nombreUsuario As String) As Usuario
        If String.IsNullOrWhiteSpace(nombreUsuario) Then
            Return Nothing
        End If

        Const consulta As String = "SELECT U.IdUsuario, U.IdRol, R.NombreRol, U.NombreUsuario, U.NombreCompleto, U.Correo, " &
            "U.PasswordHash, U.PasswordSalt, U.Estado, U.FechaCreacion, U.FechaUltimoAcceso " &
            "FROM dbo.Usuarios U " &
            "INNER JOIN dbo.Roles R ON R.IdRol = U.IdRol " &
            "WHERE U.NombreUsuario = @NombreUsuario;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 50).Value = nombreUsuario.Trim()

                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    If Not lector.Read() Then
                        Return Nothing
                    End If

                    Dim usuario As New Usuario With {
                        .IdUsuario = Convert.ToInt32(lector("IdUsuario")),
                        .NombreUsuario = Convert.ToString(lector("NombreUsuario")),
                        .NombreCompleto = Convert.ToString(lector("NombreCompleto")),
                        .PasswordHash = Convert.ToString(lector("PasswordHash")),
                        .PasswordSalt = Convert.ToString(lector("PasswordSalt")),
                        .IdRol = Convert.ToInt32(lector("IdRol")),
                        .NombreRol = Convert.ToString(lector("NombreRol")),
                        .Correo = If(lector("Correo") Is DBNull.Value, String.Empty, Convert.ToString(lector("Correo"))),
                        .Estado = Convert.ToBoolean(lector("Estado")),
                        .FechaCreacion = Convert.ToDateTime(lector("FechaCreacion"))
                    }

                    If lector("FechaUltimoAcceso") IsNot DBNull.Value Then
                        usuario.FechaUltimoAcceso = Convert.ToDateTime(lector("FechaUltimoAcceso"))
                    End If

                    Return usuario
                End Using
            End Using
        End Using
    End Function

    Public Sub ActualizarUltimoAcceso(idUsuario As Integer)
        Const consulta As String =
            "UPDATE dbo.Usuarios " &
            "SET FechaUltimoAcceso = SYSDATETIME() " &
            "WHERE IdUsuario = @IdUsuario;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

End Class