using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankingDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ADODB.Connection ConnOBJ = new ADODB.Connection();
        ADODB.Recordset RecSet = new ADODB.Recordset();
        ADODB.Recordset RecSetAcc = new ADODB.Recordset();
        int customerID,streetNo,accountNo,balance,branchNo;
        private void Form1_Load(object sender, EventArgs e)
        {
            ConnOBJ.Provider = "Microsoft.jet.oledb.4.0";
            ConnOBJ.ConnectionString = "C:\\Users\\jlwan\\source\\repos\\C#\\BankingDB\\Database\\BankingDB.mdb";
            ConnOBJ.Open();
            RecSet.Open("Select * FROM Customer order by CustomerID ASC",ConnOBJ,ADODB.CursorTypeEnum.adOpenDynamic,ADODB.LockTypeEnum.adLockOptimistic);
            RecSet.Update();
            RecSetAcc.Open("Select * FROM Account order by CustomerID ASC", ConnOBJ, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic);
            RecSetAcc.Update();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Control X in Customer.Controls)
            {
                if (X is TextBox &  string.IsNullOrEmpty(X.Text))
                {
                    MessageBox.Show("please fill in all fields");
                    return;
                }
            }
            bool success;
            success = int.TryParse(textBox1.Text, out customerID) & int.TryParse(textBox4.Text, out streetNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }
            string criteria = "CustomerID=" + textBox1.Text;
            RecSet.Find(criteria);
            
            if (RecSet.EOF == false)
            {
                MessageBox.Show("the CustomerID has been in the Database,if you would want to modify the record, please select the Save Modify button");
                return;                
            }
            RecSet.AddNew();
            SaveInDB();
            RecSet.Update();                
            MessageBox.Show("the record has been added into the Database");
        }

        private void Movefirst()
        {
            bool success;
            success = int.TryParse(textBox1.Text, out customerID) & int.TryParse(textBox4.Text, out streetNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (Control X in Customer.Controls)
            {
                if (X is TextBox &  string.IsNullOrEmpty(X.Text))
                {
                    MessageBox.Show("please fill in all fields");
                    return;
                }
            }
            bool success;
            success = int.TryParse(textBox1.Text, out customerID) & int.TryParse(textBox4.Text, out streetNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }            
            string criteria = "CustomerID = " + textBox1.Text;
            RecSet.Find(criteria);
            if (RecSet.EOF)
            {
                MessageBox.Show("cannot delete, for there isn't the CustomerID in the Database");
                return;
            }
            var result = MessageBox.Show("are you sure to delete the record from the database, once deleted, all accounts associated with the CustomerID will be deleted","delete CustomerID",MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;                    
            }
            RecSet.Delete();
            RecSet.Update();                         
            Clearform.PerformClick();
            ClearAccount.PerformClick();
            MessageBox.Show("the CustomerID has been deleted from the Database");            
        }

        private void SearchCustomerID_Click(object sender, EventArgs e)
        {
            bool success;
            success = int.TryParse(textBox1.Text, out customerID);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }            
            string criteria = "CustomerID = " + textBox1.Text;
            RecSet.Find(criteria);
            if (RecSet.EOF)
            {              
                MessageBox.Show("there isn't CustomerID in the Database");
                return;
            }
            Populateform();
            if(RecSetAcc.BOF)
            {
                return;
            }
            RecSetAcc.MoveFirst();
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                MessageBox.Show("there isn't any account associated with the Customer in the Database");
                ClearAccount.PerformClick();
                return;
            }
            AccPopulateform();
        }
        private void Populateform()
        {
            textBox1.Text = RecSet.Fields["CustomerID"].Value.ToString();
            textBox2.Text = RecSet.Fields["FirstName"].Value;
            textBox3.Text = RecSet.Fields["LastName"].Value;
            textBox4.Text = RecSet.Fields["StreetNo"].Value.ToString();
            textBox5.Text = RecSet.Fields["StreetName"].Value;
            textBox6.Text = RecSet.Fields["City"].Value;
            textBox7.Text = RecSet.Fields["Province"].Value;
            textBox8.Text = RecSet.Fields["PostalCode"].Value;
            textBox9.Text = RecSet.Fields["Country"].Value;
        }

        private void AccPopulateform()
        {
            textBox10.Text = RecSetAcc.Fields["AccountNo"].Value.ToString();
            textBox11.Text = RecSetAcc.Fields["AccountType"].Value;
            textBox12.Text = RecSetAcc.Fields["Balance"].Value.ToString();
            textBox13.Text = RecSetAcc.Fields["CustomerID"].Value.ToString();
            textBox14.Text = RecSetAcc.Fields["BranchNo"].Value.ToString();
        }

        private void Clearform_Click(object sender, EventArgs e)
        {
            foreach( Control X in Customer .Controls )
            {
                if(X is TextBox)
                {
                    X.Text = "";
                }
            }            
        }
        

        private void SearchFirstName_Click(object sender, EventArgs e)
        {
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }
            string criteria = $"FirstName = '{textBox2.Text}'" ;
            RecSet.Find(criteria);
            if (RecSet.EOF)
            {
                MessageBox.Show("there isn't the FirstName in the Database");
                return;
            }            
            Populateform();
            if(RecSetAcc.BOF)
            {                
                return;
            }
            RecSetAcc.MoveFirst();
            criteria = "CustomerID = " + RecSet.Fields["CustomerID"].Value.ToString();
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                MessageBox.Show("there isn't any account associated with the FirstName in the Database");
                ClearAccount.PerformClick();
                return;
            }
            AccPopulateform();
        }

        private void SaveModify_Click(object sender, EventArgs e)
        {            
            foreach (Control X in Customer .Controls )
            {
                if (X is TextBox &  string.IsNullOrEmpty(X.Text))
                {
                    MessageBox.Show("please fill in all fields");
                    return;
                }
            }
            bool success;
            success = int.TryParse(textBox1.Text, out customerID) & int.TryParse(textBox4.Text, out streetNo);            
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if(textBox1.Text != RecSet.Fields["CustomerID"].Value.ToString())
            {
                MessageBox.Show("the CustomerID cannot be modified, please select add CustomerID button");
                textBox1.Text = RecSet.Fields["CustomerID"].Value.ToString();
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }            
            string criteria = "CustomerID=" + textBox1.Text;
            RecSet.Find(criteria);
            if (RecSet.EOF)
            {                
                MessageBox.Show(" there isn't the record in the Database");
                return;
            }            
            SaveInDB();
            RecSet.Update();                                    
            MessageBox.Show("the record has been modified in the database");                   
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            bool success;
            success = int.TryParse(textBox1.Text, out customerID);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }
            string criteria = "CustomerID = " + textBox1.Text;
            RecSet.Find(criteria);
            RecSet.MovePrevious();
            if (RecSet.BOF == false)
            {                
                Populateform();
            }
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            criteria = "CustomerID = " + textBox1.Text;
            RecSetAcc.Find(criteria);           
            if (RecSetAcc.EOF )
            {
                ClearAccount.PerformClick();                
                return;
            }
            AccPopulateform();
        }

        private void AddAccount_Click(object sender, EventArgs e)
        {
            foreach (Control X in Account.Controls)
            {
                if (X is TextBox &  string.IsNullOrEmpty(X.Text))
                {
                    MessageBox.Show("please fill in all fields");
                    return;
                }
            }
            bool success;
            success = int.TryParse(textBox10.Text, out accountNo) & int.TryParse(textBox12.Text, out balance) & int.TryParse(textBox13.Text,out customerID) & int.TryParse(textBox14.Text,out branchNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            
            if(RecSet.BOF == false)
            {
                RecSet.MoveFirst();
            }
            string criteria = "CustomerID = " + textBox13.Text;
            RecSet.Find(criteria);
            if(RecSet.EOF)
            {
                MessageBox.Show("cannot find the Customer, please add CustomerID at first then add this account");
                return ;
            }            
            Populateform();
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            criteria = "AccountNo=" + textBox10.Text;
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF == false)
            {
                MessageBox.Show("the Account Number has been in the Database,if you would want to modify the account, please select the Save Modify button");
                Clearform.PerformClick();
                return;
            }
            RecSetAcc.AddNew();
            accSaveInDB();
            RecSetAcc.Update(); 
            RecSetAcc.Close();
            RecSetAcc.Open("Select * FROM Account order by CustomerID ASC", ConnOBJ, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic);        
            MessageBox.Show("the record has been added into the Database");            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bool success;
            success = int.TryParse(textBox1.Text, out customerID);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSet.BOF ==false)
            {
                RecSet.MoveFirst();
            }
            string criteria = "CustomerID = " + textBox1.Text;
            RecSet.Find(criteria);
            RecSet.MoveNext();
            if (RecSet.EOF == false)
            {
                Populateform();
            }
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            criteria = "CustomerID = " + textBox1.Text;
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                ClearAccount.PerformClick();
                return;
            }
            AccPopulateform();
        }

        private void ClearAccount_Click(object sender, EventArgs e)
        {
            foreach (Control X in Account.Controls)
            {
                if (X is TextBox)
                {
                    X.Text = "";
                }
            }           
        }

        private void SearchAccountNumber_Click(object sender, EventArgs e)
        {
            bool success;
            success = int.TryParse(textBox10.Text, out accountNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSetAcc.BOF)
            {
                return;
            }
            RecSetAcc.MoveFirst();
            string criteria = "AccountNo = " + textBox10.Text;
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                MessageBox.Show("there isn't the Account Number in the Database");
                return;
            }
            AccPopulateform();
            RecSet.MoveFirst();
            criteria = "CustomerID = " + RecSetAcc.Fields["CustomerID"].Value.ToString();
            RecSet.Find(criteria);
            if (RecSet.EOF)
            {
                MessageBox.Show("there should have a Customer owning the account, please ask for technical support");
                return;
            }
            Populateform();            
        }

        private void AccountModify_Click(object sender, EventArgs e)
        {
            foreach (Control X in Account.Controls)
            {
                if (X is TextBox &  string.IsNullOrEmpty(X.Text))
                {
                    MessageBox.Show("please fill in all fields");
                    return;
                }
            }
            bool success;
            success = int.TryParse(textBox10.Text, out accountNo) & int.TryParse(textBox12.Text, out balance) & int.TryParse(textBox13.Text, out customerID) & int.TryParse(textBox14.Text, out branchNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if(textBox10.Text != RecSetAcc.Fields["AccountNo"].Value.ToString())
            {
                MessageBox.Show("the Account Number cannot be modified, please select Add Account button to add a new account.");
                textBox10.Text = RecSetAcc.Fields["AccountNo"].Value.ToString();
                return;
            }
            if(textBox13.Text != RecSetAcc.Fields["CustomerID"].Value.ToString())
            {
                MessageBox.Show("the CustomerID cannot be modified, please select Add CustomerID button to add a new CustomerID.");
                textBox13.Text = RecSetAcc.Fields["CustomerID"].Value.ToString();
                return;
            }
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            string criteria = "AccountNo = " + textBox10.Text;
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                MessageBox.Show("there isn't the Account Number in the Database");
                return;
            }
            if(textBox1.Text != textBox13.Text)
            {
                MessageBox.Show(" the CustomerID associated with this account doesn't match, cannot modify the record.");
                textBox1.Text = RecSet.Fields["CustomerID"].Value.ToString();
                return;
            }
            accSaveInDB();                
            RecSetAcc.Update();               
            MessageBox.Show("the account record has been modified");
           
        }

        private void DeleteAccount_Click(object sender, EventArgs e)
        {
            foreach (Control X in Account.Controls)
            {
                if (X is TextBox &  string.IsNullOrEmpty(X.Text))
                {
                    MessageBox.Show("please fill in all fields");
                    return;
                }
            }
            bool success;
            success = int.TryParse(textBox10.Text, out accountNo) & int.TryParse(textBox12.Text, out balance) & int.TryParse(textBox13.Text, out customerID) & int.TryParse(textBox14.Text, out branchNo);
            if (success == false)
            {
                MessageBox.Show("wrong input");
                return;
            }
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            string criteria = "AccountNo = " + textBox10.Text;
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                MessageBox.Show("there isn't the Account Number in the Database");
                return;
            }
            var result = MessageBox.Show("are you sure to delete the account from the database", "delete CustomerID", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            RecSetAcc.Delete();
            RecSetAcc.Update();            
            ClearAccount.PerformClick();
            MessageBox.Show("the Account has been deleted from the Database");
        }

        private void PreviousAccount_Click(object sender, EventArgs e)
        {
            bool success;
            success = int.TryParse(textBox10.Text, out accountNo); 
            if (success == false)
            {
                MessageBox.Show("wrong inpupt");
                return;
            }
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            string criteria = "AccountNo = " + textBox10.Text;
            RecSetAcc.Find(criteria);
            RecSetAcc.MovePrevious();           
            if (RecSetAcc.BOF)
            {
                return;
            }
            AccPopulateform();
            RecSet.MoveFirst();
            criteria = "CustomerID=" + RecSetAcc.Fields["CustomerID"].Value.ToString();
            RecSet.Find(criteria);
            Populateform();
        }

        private void NextAccount_Click(object sender, EventArgs e)
        {
            bool success;
            success = int.TryParse(textBox10.Text, out accountNo);
            if (success == false)
            {
                MessageBox.Show("wrong inpupt");
                return;
            }
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            string criteria = "AccountNo = " + textBox10.Text;
            RecSetAcc.Find(criteria);
            RecSetAcc.MoveNext();
            if (RecSetAcc.EOF)
            {
                return;
            }
            AccPopulateform();
            RecSet.MoveFirst();
            criteria = "CustomerID=" + RecSetAcc.Fields["CustomerID"].Value.ToString();
            RecSet.Find(criteria);
            Populateform();
        }

        private void FirstAccount_Click(object sender, EventArgs e)
        {            
            if (RecSetAcc.BOF)
            {
                return;                
            }
            RecSetAcc.MoveFirst();            
            AccPopulateform();
            RecSet.MoveFirst();
            string criteria = "CustomerID =" + RecSetAcc.Fields["CustomerID"].Value.ToString();
            RecSet.Find(criteria);
            Populateform();
        }

        private void LastAccount_Click(object sender, EventArgs e)
        {
            if (RecSetAcc.BOF)
            {
                return;
            }
            RecSetAcc.MoveLast();
            AccPopulateform();
            RecSet.MoveFirst();
            string criteria = "CustomerID =" + RecSetAcc.Fields["CustomerID"].Value.ToString();
            RecSet.Find(criteria);
            Populateform();
        }

        private void First_Click(object sender, EventArgs e)
        {           
            if (RecSet.BOF )
            {
                return;
            }
            RecSet.MoveFirst();
            Populateform();
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            string criteria = "CustomerID = " + RecSet.Fields["CustomerID"].Value.ToString();
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                ClearAccount.PerformClick();
                return;
            }
            AccPopulateform();
        }

        private void Last_Click(object sender, EventArgs e)
        {           
            if (RecSet.EOF)
            {
                return;               
            }
            RecSet.MoveLast();
            Populateform();
            if (RecSetAcc.BOF ==false)
            {
                RecSetAcc.MoveFirst();
            }
            string criteria = "CustomerID = " + RecSet.Fields["CustomerID"].Value.ToString();
            RecSetAcc.Find(criteria);
            if (RecSetAcc.EOF)
            {
                ClearAccount.PerformClick();
                return;
            }
            AccPopulateform();
        }

        private void SaveInDB()
        {
            
            RecSet.Fields["CustomerID"].Value = customerID;
            RecSet.Fields["FirstName"].Value = textBox2.Text;
            RecSet.Fields["LastName"].Value = textBox3.Text;
            RecSet.Fields["StreetNo"].Value = streetNo;
            RecSet.Fields["StreetName"].Value = textBox5.Text;            
            RecSet.Fields["City"].Value = textBox6.Text;
            RecSet.Fields["Province"].Value = textBox7.Text;
            RecSet.Fields["PostalCode"].Value = textBox8.Text;
            RecSet.Fields["Country"].Value = textBox9.Text;
                      
        }
        private void accSaveInDB()
        {
                        
            RecSetAcc.Fields["AccountNo"].Value = accountNo;
            RecSetAcc.Fields["AccountType"].Value = textBox11.Text;
            RecSetAcc.Fields["Balance"].Value = balance;
            RecSetAcc.Fields["CustomerID"].Value = customerID;
            RecSetAcc.Fields["BranchNo"].Value = branchNo;           
            
        }
    }
}
