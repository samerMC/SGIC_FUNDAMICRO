Public Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ConfigurarMenu()
    End Sub

    Private Sub ConfigurarMenu()
        Dim sesionActiva As Boolean = SesionHelper.ExisteSesionActiva()

        phMenuLogin.Visible = Not sesionActiva
        phMenuPrivado.Visible = sesionActiva

        If sesionActiva Then
            lblUsuarioSesion.Text = SesionHelper.ObtenerNombreCompleto()
        Else
            lblUsuarioSesion.Text = String.Empty
        End If
    End Sub

End Class