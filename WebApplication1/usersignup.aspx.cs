using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebApplication1
{
    public partial class usersignup : System.Web.UI.Page
    {

        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Sign Up Button functionality
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {

                Response.Write("<script>alert('Member Already Exist with this Member ID, try other ID');</script>");
            }
            else
            {
                signUpNewMember();
            }
        }

        // user defined method
        bool checkMemberExists()
        {
            SqlConnection con = new SqlConnection(strcon);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * from member_master_tbl where member_id='" + txtUserID.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        void signUpNewMember()
        {
            SqlConnection con = new SqlConnection(strcon);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO member_master_tbl(full_name,dob,contact_no,email,state,city,pincode,full_address,member_id,password,account_status) values(@full_name,@dob,@contact_no,@email,@state,@city,@pincode,@full_address,@member_id,@password,@account_status)", con);
                cmd.Parameters.AddWithValue("@full_name", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", txtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@contact_no", txtContactNo.Text.Trim());
                cmd.Parameters.AddWithValue("@email", txtEmailID.Text.Trim());
                cmd.Parameters.AddWithValue("@state", ddlState.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@city", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@pincode", txtPinCode.Text.Trim());
                cmd.Parameters.AddWithValue("@full_address", txtFullAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@member_id", txtUserID.Text.Trim());
                cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@account_status", "pending");
                int status = cmd.ExecuteNonQuery();
                con.Close();
                if(status != -1)
                {
                    Response.Write("<script>alert('Sign Up Successful. Go to User Login Page');</script>");
                    clearSignupForm();
                }
                else
                {
                    Response.Write("<script>alert('System is facing some proble while signup! try again later.');</script>");
                }
                
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
        }

        private void clearSignupForm()
        {
            txtFullName.Text = string.Empty;
            txtDOB.Text =  string.Empty;
            txtContactNo.Text = string.Empty;
            ddlState.SelectedIndex = 0;
            txtCity.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserID.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            txtFullAddress.Text = string.Empty;

        }
    }
}