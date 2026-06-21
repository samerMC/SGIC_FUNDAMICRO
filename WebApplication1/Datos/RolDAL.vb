Imports System.Data
Imports System.Data.SqlClient

Public Class RolDAL

    Public Function ListarActivos() As List(Of Rol)
        Const consulta As String =
            "SELECT IdRol, NombreRol, Estado " &
            "FROM dbo.Roles " &
            "WHERE Estado = 1 " &
            "ORDER BY NombreRol;"

        Dim roles As New List(Of Rol)()

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text

                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        roles.Add(New Rol With {
                            .IdRol = Convert.ToInt32(lector("IdRol")),
                            .NombreRol = Convert.ToString(lector("NombreRol")),
                            .Estado = Convert.ToBoolean(lector("Estado"))
                        })
                    End While
                End Using
            End Using
        End Using

        Return roles
    End Function

End Class