Imports System.Web.UI.WebControls

Public NotInheritable Class MensajeHelper

    Private Sub New()
    End Sub

    Public Shared Sub Exito(etiqueta As Label, mensaje As String)
        Mostrar(etiqueta, mensaje, "app-message app-message-success")
    End Sub

    Public Shared Sub ErrorSistema(etiqueta As Label, mensaje As String)
        Mostrar(etiqueta, mensaje, "app-message app-message-error")
    End Sub

    Public Shared Sub Advertencia(etiqueta As Label, mensaje As String)
        Mostrar(etiqueta, mensaje, "app-message app-message-warning")
    End Sub

    Public Shared Sub Limpiar(etiqueta As Label)
        If etiqueta Is Nothing Then
            Return
        End If

        etiqueta.Text = String.Empty
        etiqueta.CssClass = "app-message"
    End Sub

    Private Shared Sub Mostrar(etiqueta As Label, mensaje As String, claseCss As String)
        If etiqueta Is Nothing Then
            Return
        End If

        etiqueta.Text = mensaje
        etiqueta.CssClass = claseCss
    End Sub

End Class