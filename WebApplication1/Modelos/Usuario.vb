Public Class Usuario

    Public Property IdUsuario As Integer

    Public Property IdRol As Integer

    Public Property NombreRol As String

    Public Property NombreUsuario As String

    Public Property NombreCompleto As String

    Public Property Correo As String

    Public Property PasswordHash As String

    Public Property PasswordSalt As String

    Public Property Estado As Boolean

    Public Property FechaCreacion As DateTime

    Public Property FechaActualizacion As DateTime?

    Public Property FechaUltimoAcceso As DateTime?

    Public ReadOnly Property EstadoTexto As String
        Get
            Return If(Estado, "Activo", "Inactivo")
        End Get
    End Property

    Public ReadOnly Property AccionEstadoTexto As String
        Get
            Return If(Estado, "Desactivar", "Activar")
        End Get
    End Property

End Class