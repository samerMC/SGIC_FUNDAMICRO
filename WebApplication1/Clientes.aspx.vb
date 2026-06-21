Public Class Clientes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not SesionHelper.ValidarAcceso() Then
            Return
        End If
    End Sub

End Class