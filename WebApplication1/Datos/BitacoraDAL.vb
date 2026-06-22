Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class BitacoraDAL

    Public Sub Registrar(bitacora As Bitacora)
        Const consulta As String =
            "INSERT INTO dbo.Bitacoras " &
            "(Accion, IdCliente, IdUsuario, NombreUsuario, FechaHora, Detalle, IpOrigen) " &
            "VALUES " &
            "(@Accion, @IdCliente, @IdUsuario, @NombreUsuario, SYSDATETIME(), @Detalle, @IpOrigen);"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text

                comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 20).Value = bitacora.Accion
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = bitacora.IdCliente
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = bitacora.IdUsuario
                comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 50).Value = bitacora.NombreUsuario
                comando.Parameters.Add("@Detalle", SqlDbType.NVarChar, 500).Value = ObtenerValorNullable(bitacora.Detalle)
                comando.Parameters.Add("@IpOrigen", SqlDbType.NVarChar, 45).Value = ObtenerValorNullable(bitacora.IpOrigen)

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function Listar(fechaDesde As DateTime?,
                           fechaHasta As DateTime?,
                           accion As String,
                           usuario As String) As List(Of Bitacora)

        Dim consulta As New StringBuilder()

        consulta.AppendLine("SELECT TOP (200)")
        consulta.AppendLine("    B.IdBitacora,")
        consulta.AppendLine("    B.Accion,")
        consulta.AppendLine("    B.IdCliente,")
        consulta.AppendLine("    LTRIM(RTRIM(ISNULL(C.Nombre, '') + ' ' + ISNULL(C.Apellido, ''))) AS Cliente,")
        consulta.AppendLine("    B.IdUsuario,")
        consulta.AppendLine("    B.NombreUsuario,")
        consulta.AppendLine("    B.FechaHora,")
        consulta.AppendLine("    B.Detalle,")
        consulta.AppendLine("    B.IpOrigen")
        consulta.AppendLine("FROM dbo.Bitacoras B")
        consulta.AppendLine("INNER JOIN dbo.Clientes C ON C.IdCliente = B.IdCliente")
        consulta.AppendLine("WHERE 1 = 1")

        Dim bitacoras As New List(Of Bitacora)()

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand()
                comando.Connection = conexion
                comando.CommandType = CommandType.Text

                If fechaDesde.HasValue Then
                    consulta.AppendLine("AND B.FechaHora >= @FechaDesde")
                    comando.Parameters.Add("@FechaDesde", SqlDbType.DateTime2).Value = fechaDesde.Value.Date
                End If

                If fechaHasta.HasValue Then
                    consulta.AppendLine("AND B.FechaHora < @FechaHasta")
                    comando.Parameters.Add("@FechaHasta", SqlDbType.DateTime2).Value = fechaHasta.Value.Date.AddDays(1)
                End If

                If Not String.IsNullOrWhiteSpace(accion) Then
                    consulta.AppendLine("AND B.Accion = @Accion")
                    comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 20).Value = accion.Trim()
                End If

                If Not String.IsNullOrWhiteSpace(usuario) Then
                    consulta.AppendLine("AND B.NombreUsuario LIKE @Usuario")
                    comando.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = "%" & usuario.Trim() & "%"
                End If

                consulta.AppendLine("ORDER BY B.FechaHora DESC, B.IdBitacora DESC;")

                comando.CommandText = consulta.ToString()

                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        bitacoras.Add(MapearBitacora(lector))
                    End While
                End Using
            End Using
        End Using

        Return bitacoras
    End Function

    ' Mapea un registro del SqlDataReader a un objeto Bitacora
    Private Shared Function MapearBitacora(lector As SqlDataReader) As Bitacora
        Return New Bitacora With {
            .IdBitacora = Convert.ToInt32(lector("IdBitacora")),
            .Accion = Convert.ToString(lector("Accion")),
            .IdCliente = Convert.ToInt32(lector("IdCliente")),
            .Cliente = Convert.ToString(lector("Cliente")),
            .IdUsuario = Convert.ToInt32(lector("IdUsuario")),
            .NombreUsuario = Convert.ToString(lector("NombreUsuario")),
            .FechaHora = Convert.ToDateTime(lector("FechaHora")),
            .Detalle = If(lector("Detalle") Is DBNull.Value, String.Empty, Convert.ToString(lector("Detalle"))),
            .IpOrigen = If(lector("IpOrigen") Is DBNull.Value, String.Empty, Convert.ToString(lector("IpOrigen")))
        }
    End Function

    ' Convierte una cadena vacía o nula a DBNull.Value para parámetros SQL, o devuelve la cadena limpia
    Private Shared Function ObtenerValorNullable(valor As String) As Object
        If String.IsNullOrWhiteSpace(valor) Then
            Return DBNull.Value
        End If

        Return valor.Trim()
    End Function

End Class