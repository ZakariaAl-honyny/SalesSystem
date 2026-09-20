using System.Windows.Forms;

namespace SalesSystem.Desktop.GlobalClasses
{
    internal static class clsGlobal
    {
        public static Form? frmMdiMain = null!;

        public static void _OpenForm(Form form)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name == "frmLogin" || openForm.Name == "frmMain")
                    continue;

                if (openForm.Name == form.Name)
                {
                    openForm.WindowState = FormWindowState.Normal;
                    openForm.Activate();
                    return;
                }
            }

            if (frmMdiMain != null)
            {
                form.Icon = frmMdiMain.Icon;
                form.MdiParent = frmMdiMain;
            }
            form.Show();
        }
    }
}
