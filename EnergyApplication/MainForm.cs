﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnergyApplication
{
    public partial class MainForm : Form
    {
        private bool childFormOpen = false;
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void apartmentRegisterButton_Click(object sender, EventArgs e)
        {
            apartmentRegisterForm myApartmentRegisterForm = new apartmentRegisterForm();
            myApartmentRegisterForm.FormClosed += ChildForm_FormClosed;
            myApartmentRegisterForm.Show();
            this.Enabled = false;
            childFormOpen = true;
        }

      
        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            this.Enabled = true;
            childFormOpen = false;

        }
    }
}
