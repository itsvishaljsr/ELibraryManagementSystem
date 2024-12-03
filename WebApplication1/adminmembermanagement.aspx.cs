using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WebApplication1
{
    public partial class adminmembermanagement : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null)
            {
                Response.Redirect("homepage.aspx");
            }
            else if (Session["role"].Equals("user"))
            {
                Response.Redirect("homepage.aspx");
            }
            grdViewMemberDetails.DataBind();
        }

        protected void lnkBtnGetMemberDetails_Click(object sender, EventArgs e)
        {
            getMemberByID();
        }

        protected void lnkBtnActive_Click(object sender, EventArgs e)
        {
            updateMemberStatusByID("active");
        }

        protected void lnkBtnPending_Click(object sender, EventArgs e)
        {
            updateMemberStatusByID("pending");
        }

        protected void lnkDeactive_Click(object sender, EventArgs e)
        {
            updateMemberStatusByID("deactive");
        }

        protected void btnDeleteMember_Click(object sender, EventArgs e)
        {
            deleteMemberByID();
        }

        bool checkIfMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
               
                SqlCommand cmd = new SqlCommand("SELECT * from member_master_tbl where member_id='" + txtMemberID.Text.Trim() + "';", con);
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
        }

        void deleteMemberByID()
        {
            if (checkIfMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("DELETE from member_master_tbl WHERE member_id='" + txtMemberID.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Member Deleted Successfully');</script>");
                    clearForm();
                    grdViewMemberDetails.DataBind();

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }

            }
            else
            {
                Response.Write("<script>alert('Invalid Member ID');</script>");
            }
        }

        void getMemberByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }
                SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id='" + txtMemberID.Text.Trim() + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txtName.Text = dr.GetValue(0).ToString();
                        txtAccountStatus.Text = dr.GetValue(10).ToString();
                        txtDOB.Text = dr.GetValue(1).ToString();
                        txtContactNo.Text = dr.GetValue(2).ToString();
                        txtEmailID.Text = dr.GetValue(3).ToString();
                        txtState.Text = dr.GetValue(4).ToString();
                        txtCity.Text = dr.GetValue(5).ToString();
                        txtPincode.Text = dr.GetValue(6).ToString();
                        txtFullAddress.Text = dr.GetValue(7).ToString();

                    }

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
        }

        void updateMemberStatusByID(string status)
        {
            if (checkIfMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl SET account_status='" + status + "' WHERE member_id='" + txtMemberID.Text.Trim() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    grdViewMemberDetails.DataBind();
                    Response.Write("<script>alert('Member Status Updated');</script>");


                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid Member ID');</script>");
            }

        }

        void clearForm()
        {
            txtAccountStatus.Text = "";
            txtName.Text = "";
            txtAccountStatus.Text = "";
            txtDOB.Text = "";
            txtContactNo.Text = "";
            txtEmailID.Text = "";
            txtState.Text = "";
            txtCity.Text = "";
            txtPincode.Text = "";
            txtFullAddress.Text = "";
        }

    }
}