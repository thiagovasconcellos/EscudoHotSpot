using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscudoHotSpot
{
    public partial class Form1 : MaterialForm
    {
        Hotspot h = null;
        Process NewProcess = new Process();
        public Form1()
        {
            h = new Hotspot();
            InitializeComponent();
            isUserAdmin();
        }

        public bool isUserAdmin()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                isAdmin = false;
                throw;
            }
            return isAdmin;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            h.Create(txtNameSsid.Text, txtPass.Text);
            h.Start();
            txtMessage.Text = h.Message;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            h.Stop();
        }
        
    }
}
