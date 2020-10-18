using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkingWithDatabase.Data;
using WorkingWithDatabase.Model;

namespace WorkingWithDatabase
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetInput();
            Save();
            Clear();
            PopulateGrid();
            MessageBox.Show("Saved Successfully");
        }

        private void Save()
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (model.Id == 0)
                {
                    ctx.Customers.Add(model);
                }
                else
                {
                    ctx.Entry(model).State = EntityState.Modified;
                }
                ctx.SaveChanges();
            }
        }

        private void GetInput()
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.PhoneNumber = txtPhoneNumber.Text.Trim();
            model.Address = txtAddress.Text.Trim();
        }

        private void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtPhoneNumber.Text = txtAddress.Text = "";
        }

        private void PopulateGrid()
        {
            dgvCustomers.AutoGenerateColumns = false;
            using (var ctx = new ApplicationDbContext())
            {
                dgvCustomers.DataSource = ctx.Customers.ToList<Customer>();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateGrid();
        }

        private void dgvCustomers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["Id"].Value);

                using (var ctx = new ApplicationDbContext())
                {
                    model = ctx.Customers.Where(x => x.Id == model.Id).FirstOrDefault();

                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtPhoneNumber.Text = model.PhoneNumber;
                    txtAddress.Text = model.Address;
            btnDelete.Enabled = true;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetInput();
            Save();
            PopulateGrid();
            MessageBox.Show("Updated");
            Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            PopulateGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sur you want to delete this entry?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Delete();
                PopulateGrid();
                Clear();
                MessageBox.Show("Users has been deleted.");
            }
        }

        private void Delete()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entry = ctx.Entry(model);
                if (entry.State == EntityState.Detached)
                    ctx.Customers.Attach(model);
                ctx.Customers.Remove(model);
                ctx.SaveChanges();
            }
        }
    }
}
