Imports System.Data
Imports System.Data.SqlClient

Public Class ClienteDAL

    Public Function ListarActivos() As List(Of Cliente)
        Const consulta As String =
            "SELECT IdCliente, Nombre, Apellido, Dui, Nit, Telefono, Correo, Direccion, Estado, " &
            "FechaCreacion, FechaActualizacion, IdUsuarioCreacion, IdUsuarioActualizacion " &
            "FROM dbo.Clientes " &
            "WHERE Estado = 1 " &
            "ORDER BY IdCliente DESC;"

        Dim clientes As New List(Of Cliente)()

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text

                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        clientes.Add(MapearCliente(lector))
                    End While
                End Using
            End Using
        End Using

        Return clientes
    End Function

    ' Este método se puede usar para obtener un cliente específico, por ejemplo, para editarlo o mostrar sus detalles.
    Public Function ObtenerPorId(idCliente As Integer) As Cliente
        Const consulta As String =
            "SELECT IdCliente, Nombre, Apellido, Dui, Nit, Telefono, Correo, Direccion, Estado, " &
            "FechaCreacion, FechaActualizacion, IdUsuarioCreacion, IdUsuarioActualizacion " &
            "FROM dbo.Clientes " &
            "WHERE IdCliente = @IdCliente;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente

                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    If lector.Read() Then
                        Return MapearCliente(lector)
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    Public Function ExisteDui(dui As String, idClienteExcluir As Integer) As Boolean
        If String.IsNullOrWhiteSpace(dui) Then
            Return False
        End If

        Const consulta As String =
            "SELECT COUNT(1) " &
            "FROM dbo.Clientes " &
            "WHERE Dui = @Dui " &
            "AND Estado = 1 " &
            "AND IdCliente <> @IdClienteExcluir;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                comando.Parameters.Add("@Dui", SqlDbType.NVarChar, 10).Value = dui.Trim()
                comando.Parameters.Add("@IdClienteExcluir", SqlDbType.Int).Value = idClienteExcluir

                conexion.Open()

                Return Convert.ToInt32(comando.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Public Function Insertar(cliente As Cliente, idUsuarioSesion As Integer) As Integer
        Const consulta As String =
            "INSERT INTO dbo.Clientes " &
            "(Nombre, Apellido, Dui, Nit, Telefono, Correo, Direccion, Estado, FechaCreacion, IdUsuarioCreacion) " &
            "OUTPUT INSERTED.IdCliente " &
            "VALUES " &
            "(@Nombre, @Apellido, @Dui, @Nit, @Telefono, @Correo, @Direccion, 1, SYSDATETIME(), @IdUsuarioCreacion);"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                AgregarParametrosCliente(comando, cliente)
                comando.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = idUsuarioSesion

                conexion.Open()

                Return Convert.ToInt32(comando.ExecuteScalar())
            End Using
        End Using
    End Function

    Public Sub Actualizar(cliente As Cliente, idUsuarioSesion As Integer)
        Const consulta As String =
            "UPDATE dbo.Clientes SET " &
            "Nombre = @Nombre, " &
            "Apellido = @Apellido, " &
            "Dui = @Dui, " &
            "Nit = @Nit, " &
            "Telefono = @Telefono, " &
            "Correo = @Correo, " &
            "Direccion = @Direccion, " &
            "FechaActualizacion = SYSDATETIME(), " &
            "IdUsuarioActualizacion = @IdUsuarioActualizacion " &
            "WHERE IdCliente = @IdCliente " &
            "AND Estado = 1;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                AgregarParametrosCliente(comando, cliente)
                comando.Parameters.Add("@IdUsuarioActualizacion", SqlDbType.Int).Value = idUsuarioSesion
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = cliente.IdCliente

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' Eliminación lógica: se marca el cliente como inactivo en lugar de eliminarlo físicamente de la base de datos.
    Public Sub EliminarLogico(idCliente As Integer, idUsuarioSesion As Integer)
        Const consulta As String =
            "UPDATE dbo.Clientes SET " &
            "Estado = 0, " &
            "FechaActualizacion = SYSDATETIME(), " &
            "IdUsuarioActualizacion = @IdUsuarioActualizacion " &
            "WHERE IdCliente = @IdCliente " &
            "AND Estado = 1;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.CommandType = CommandType.Text
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = idCliente
                comando.Parameters.Add("@IdUsuarioActualizacion", SqlDbType.Int).Value = idUsuarioSesion

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' Este método se puede usar para reactivar un cliente que fue eliminado lógicamente.
    Private Shared Sub AgregarParametrosCliente(comando As SqlCommand, cliente As Cliente)
        comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre.Trim()
        comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido.Trim()
        comando.Parameters.Add("@Dui", SqlDbType.NVarChar, 10).Value = ObtenerValorNullable(cliente.Dui)
        comando.Parameters.Add("@Nit", SqlDbType.NVarChar, 20).Value = ObtenerValorNullable(cliente.Nit)
        comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = ObtenerValorNullable(cliente.Telefono)
        comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = ObtenerValorNullable(cliente.Correo)
        comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 250).Value = ObtenerValorNullable(cliente.Direccion)
    End Sub

    ' Este método convierte una cadena vacía o nula en DBNull.Value para que se pueda almacenar correctamente en la base de datos.
    Private Shared Function ObtenerValorNullable(valor As String) As Object
        If String.IsNullOrWhiteSpace(valor) Then
            Return DBNull.Value
        End If

        Return valor.Trim()
    End Function

    ' Mapea un registro del SqlDataReader a un objeto Cliente, manejando posibles valores nulos para campos opcionales.
    Private Shared Function MapearCliente(lector As SqlDataReader) As Cliente
        Dim cliente As New Cliente With {
            .IdCliente = Convert.ToInt32(lector("IdCliente")),
            .Nombre = Convert.ToString(lector("Nombre")),
            .Apellido = Convert.ToString(lector("Apellido")),
            .Dui = If(lector("Dui") Is DBNull.Value, String.Empty, Convert.ToString(lector("Dui"))),
            .Nit = If(lector("Nit") Is DBNull.Value, String.Empty, Convert.ToString(lector("Nit"))),
            .Telefono = If(lector("Telefono") Is DBNull.Value, String.Empty, Convert.ToString(lector("Telefono"))),
            .Correo = If(lector("Correo") Is DBNull.Value, String.Empty, Convert.ToString(lector("Correo"))),
            .Direccion = If(lector("Direccion") Is DBNull.Value, String.Empty, Convert.ToString(lector("Direccion"))),
            .Estado = Convert.ToBoolean(lector("Estado")),
            .FechaCreacion = Convert.ToDateTime(lector("FechaCreacion"))
        }

        If lector("FechaActualizacion") IsNot DBNull.Value Then
            cliente.FechaActualizacion = Convert.ToDateTime(lector("FechaActualizacion"))
        End If

        If lector("IdUsuarioCreacion") IsNot DBNull.Value Then
            cliente.IdUsuarioCreacion = Convert.ToInt32(lector("IdUsuarioCreacion"))
        End If

        If lector("IdUsuarioActualizacion") IsNot DBNull.Value Then
            cliente.IdUsuarioActualizacion = Convert.ToInt32(lector("IdUsuarioActualizacion"))
        End If

        Return cliente
    End Function

End Class