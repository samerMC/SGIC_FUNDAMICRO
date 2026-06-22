Public Class Cliente

    Public Property IdCliente As Integer

    Public Property Nombre As String

    Public Property Apellido As String

    Public Property Dui As String

    Public Property Nit As String

    Public Property Telefono As String

    Public Property Correo As String

    Public Property Direccion As String

    Public Property Estado As Boolean

    Public Property FechaCreacion As DateTime

    Public Property FechaActualizacion As DateTime?

    Public Property IdUsuarioCreacion As Integer?

    Public Property IdUsuarioActualizacion As Integer?

    Public ReadOnly Property NombreCompleto As String
        Get
            Return String.Format("{0} {1}", Nombre, Apellido).Trim()
        End Get
    End Property

    Public ReadOnly Property EstadoTexto As String
        Get
            Return If(Estado, "Activo", "Eliminado")
        End Get
    End Property

End Class