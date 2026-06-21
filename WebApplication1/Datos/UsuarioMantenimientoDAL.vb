Imports System.Data
Imports System.Data.SqlClient

Public Class UsuarioMantenimientoDAL

    Public Function Listar() As List(Of Usuario)
        Const consulta As String =
            "SELECT U.IdUsuario, U.IdRol, R.NombreRol, U.NombreUsuario, U.NombreCompleto, U.Correo, " &
            "U.Estado, U.FechaCreacion, U.FechaActualizacion, U.FechaUltimoAcceso " &
            "FROM dbo.Usuarios U " &
            "INNER JOIN dbo.Roles R ON R.IdRol = U.IdRol " &
            "ORDER BY U.IdUsuario DESC;"

        Dim usuarios As New List(Of Usuario)()

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        usuarios.Add(MapearUsuarioListado(lector))
                    End While
                End Using
            End Using
        End Using

        Return usuarios
    End Function

    Public Function ObtenerPorId(idUsuario As Integer) As Usuario
        Const consulta As String =
            "SELECT U.IdUsuario, U.IdRol, R.NombreRol, U.NombreUsuario, U.NombreCompleto, U.Correo, " &
            "U.Estado, U.FechaCreacion, U.FechaActualizacion, U.FechaUltimoAcceso " &
            "FROM dbo.Usuarios U " &
            "INNER JOIN dbo.Roles R ON R.IdRol = U.IdRol " &
            "WHERE U.IdUsuario = @IdUsuario;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario

                conexion.Open()

                Using lector As SqlDataReader = comando.ExecuteReader()
                    If lector.Read() Then
                        Return MapearUsuarioListado(lector)
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function

    Public Function ExisteNombreUsuario(nombreUsuario As String, idUsuarioExcluir As Integer) As Boolean
        Const consulta As String =
            "SELECT COUNT(1) " &
            "FROM dbo.Usuarios " &
            "WHERE NombreUsuario = @NombreUsuario " &
            "AND IdUsuario <> @IdUsuarioExcluir;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 50).Value = nombreUsuario.Trim()
                comando.Parameters.Add("@IdUsuarioExcluir", SqlDbType.Int).Value = idUsuarioExcluir

                conexion.Open()
                Return Convert.ToInt32(comando.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Public Function ExisteCorreo(correo As String, idUsuarioExcluir As Integer) As Boolean
        If String.IsNullOrWhiteSpace(correo) Then
            Return False
        End If

        Const consulta As String =
            "SELECT COUNT(1) " &
            "FROM dbo.Usuarios " &
            "WHERE Correo = @Correo " &
            "AND IdUsuario <> @IdUsuarioExcluir;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = correo.Trim()
                comando.Parameters.Add("@IdUsuarioExcluir", SqlDbType.Int).Value = idUsuarioExcluir

                conexion.Open()
                Return Convert.ToInt32(comando.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Public Sub Insertar(usuario As Usuario, idUsuarioSesion As Integer)
        Const consulta As String =
            "INSERT INTO dbo.Usuarios " &
            "(IdRol, NombreUsuario, NombreCompleto, Correo, PasswordHash, PasswordSalt, Estado, FechaCreacion, IdUsuarioCreacion) " &
            "VALUES " &
            "(@IdRol, @NombreUsuario, @NombreCompleto, @Correo, @PasswordHash, @PasswordSalt, @Estado, SYSDATETIME(), @IdUsuarioCreacion);"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                AgregarParametrosUsuario(comando, usuario)
                comando.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = idUsuarioSesion

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Actualizar(usuario As Usuario, actualizarPassword As Boolean, idUsuarioSesion As Integer)
        Const consulta As String =
            "UPDATE dbo.Usuarios SET " &
            "IdRol = @IdRol, " &
            "NombreUsuario = @NombreUsuario, " &
            "NombreCompleto = @NombreCompleto, " &
            "Correo = @Correo, " &
            "Estado = @Estado, " &
            "PasswordHash = CASE WHEN @ActualizarPassword = 1 THEN @PasswordHash ELSE PasswordHash END, " &
            "PasswordSalt = CASE WHEN @ActualizarPassword = 1 THEN @PasswordSalt ELSE PasswordSalt END, " &
            "FechaActualizacion = SYSDATETIME(), " &
            "IdUsuarioActualizacion = @IdUsuarioActualizacion " &
            "WHERE IdUsuario = @IdUsuario;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                AgregarParametrosUsuario(comando, usuario)
                comando.Parameters.Add("@ActualizarPassword", SqlDbType.Bit).Value = actualizarPassword
                comando.Parameters.Add("@IdUsuarioActualizacion", SqlDbType.Int).Value = idUsuarioSesion
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = usuario.IdUsuario

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub CambiarEstado(idUsuario As Integer, nuevoEstado As Boolean, idUsuarioSesion As Integer)
        Const consulta As String =
            "UPDATE dbo.Usuarios SET " &
            "Estado = @Estado, " &
            "FechaActualizacion = SYSDATETIME(), " &
            "IdUsuarioActualizacion = @IdUsuarioActualizacion " &
            "WHERE IdUsuario = @IdUsuario;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                comando.Parameters.Add("@Estado", SqlDbType.Bit).Value = nuevoEstado
                comando.Parameters.Add("@IdUsuarioActualizacion", SqlDbType.Int).Value = idUsuarioSesion
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario

                conexion.Open()
                comando.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ContarAdministradoresActivos() As Integer
        Const consulta As String =
            "SELECT COUNT(1) " &
            "FROM dbo.Usuarios U " &
            "INNER JOIN dbo.Roles R ON R.IdRol = U.IdRol " &
            "WHERE R.NombreRol = 'Administrador' " &
            "AND U.Estado = 1;"

        Using conexion As SqlConnection = ConexionBD.ObtenerConexion()
            Using comando As New SqlCommand(consulta, conexion)
                conexion.Open()
                Return Convert.ToInt32(comando.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Shared Sub AgregarParametrosUsuario(comando As SqlCommand, usuario As Usuario)
        comando.Parameters.Add("@IdRol", SqlDbType.Int).Value = usuario.IdRol
        comando.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 50).Value = usuario.NombreUsuario.Trim()
        comando.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 150).Value = usuario.NombreCompleto.Trim()

        If String.IsNullOrWhiteSpace(usuario.Correo) Then
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = DBNull.Value
        Else
            comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = usuario.Correo.Trim()
        End If

        comando.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 64).Value = usuario.PasswordHash
        comando.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 100).Value = usuario.PasswordSalt
        comando.Parameters.Add("@Estado", SqlDbType.Bit).Value = usuario.Estado
    End Sub

    Private Shared Function MapearUsuarioListado(lector As SqlDataReader) As Usuario
        Dim usuario As New Usuario With {
            .IdUsuario = Convert.ToInt32(lector("IdUsuario")),
            .IdRol = Convert.ToInt32(lector("IdRol")),
            .NombreRol = Convert.ToString(lector("NombreRol")),
            .NombreUsuario = Convert.ToString(lector("NombreUsuario")),
            .NombreCompleto = Convert.ToString(lector("NombreCompleto")),
            .Correo = If(lector("Correo") Is DBNull.Value, String.Empty, Convert.ToString(lector("Correo"))),
            .Estado = Convert.ToBoolean(lector("Estado")),
            .FechaCreacion = Convert.ToDateTime(lector("FechaCreacion"))
        }

        If lector("FechaActualizacion") IsNot DBNull.Value Then
            usuario.FechaActualizacion = Convert.ToDateTime(lector("FechaActualizacion"))
        End If

        If lector("FechaUltimoAcceso") IsNot DBNull.Value Then
            usuario.FechaUltimoAcceso = Convert.ToDateTime(lector("FechaUltimoAcceso"))
        End If

        Return usuario
    End Function

End Class