using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;

namespace WebApplication1
{
    public partial class userlogin : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(strcon);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }
                SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id='" + txtMemberID.Text.Trim() + "' AND password='" + txtPassword.Text.Trim() + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    var dtRecords = new DataTable();
                    dtRecords.Load(dr);

                    Response.Write("<script>alert('Welcome " + dtRecords.Rows[0].Field<string>("full_name") + "');</script>");
                    Session["username"] = dtRecords.Rows[0].Field<string>("member_id");
                    Session["fullname"] = dtRecords.Rows[0].Field<string>("full_name");
                    Session["role"] = "user";
                    Session["status"] = dtRecords.Rows[0].Field<string>("account_status");

                    Response.Redirect("homepage.aspx", false);
                }
                else
                {
                    Response.Write("<script>alert('Invalid credentials');</script>");
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
    }
}