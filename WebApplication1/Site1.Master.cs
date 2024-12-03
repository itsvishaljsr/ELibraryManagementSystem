using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["role"] == null)
                {
                    lnkBtnUserLogin.Visible = true; // user login link button
                    lnkBtnSignUp.Visible = true; // sign up link button

                    lnkBtnLogout.Visible = false; // logout link button
                    lnkBtnGreet.Visible = false; // hello user link button


                    lnkBtnAdminLogin.Visible = true; // admin login link button
                    lnkBtnAuthorManagement.Visible = false; // author management link button
                    lnkBtnPublisherManagement.Visible = false; // publisher management link button
                    lnkBtnBookInventory.Visible = false; // book inventory link button
                    lnkBtnBookIssuing.Visible = false; // book issuing link button
                    lnkBtnMemberManagement.Visible = false; // member management link button

                }
                else if (Session["role"].Equals("user"))
                {
                    lnkBtnUserLogin.Visible = false; // user login link button
                    lnkBtnSignUp.Visible = false; // sign up link button

                    lnkBtnLogout.Visible = true; // logout link button
                    lnkBtnGreet.Visible = true; // hello user link button
                    lnkBtnGreet.Text = "Hello " + Session["fullname"].ToString();


                    lnkBtnAdminLogin.Visible = true; // admin login link button
                    lnkBtnAuthorManagement.Visible = false; // author management link button
                    lnkBtnPublisherManagement.Visible = false; // publisher management link button
                    lnkBtnBookInventory.Visible = false; // book inventory link button
                    lnkBtnBookIssuing.Visible = false; // book issuing link button
                }
                else if (Session["role"].Equals("admin"))
                {
                    lnkBtnUserLogin.Visible = false; // user login link button
                    lnkBtnSignUp.Visible = false; // sign up link button

                    lnkBtnLogout.Visible = true; // logout link button
                    lnkBtnGreet.Visible = true; // hello user link button
                    lnkBtnGreet.Text = "Hello " + Session["fullname"].ToString();


                    lnkBtnAdminLogin.Visible = false; // admin login link button
                    lnkBtnAuthorManagement.Visible = true; // author management link button
                    lnkBtnPublisherManagement.Visible = true; // publisher management link button
                    lnkBtnBookInventory.Visible = true; // book inventory link button
                    lnkBtnBookIssuing.Visible = true; // book issuing link button
                    lnkBtnMemberManagement.Visible = true; // member management link button
                    lnkBtnGreet.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void lnkBtnAdminLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminlogin.aspx");
        }

        protected void lnkBtnAuthorManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminauthormanagement.aspx");
        }

        protected void lnkBtnPublisherManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminpublishermanagement.aspx");
        }


        protected void lnkBtnBookIssuing_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminbookissuing.aspx");
        }

        protected void lnkBtnMemberManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminmembermanagement.aspx");
        }

        protected void lnkBtnBookInventory_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminbookinventory.aspx");
        }

        protected void lnkBtnUserLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlogin.aspx");
        }

        protected void lnkBtnSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("usersignup.aspx");
        }

        protected void lnkBtnViewBooks_Click(object sender, EventArgs e)
        {
            Response.Redirect("viewbooks.aspx");
        }

        protected void lnkBtnLogout_Click(object sender, EventArgs e)
        {
            Session["username"] = "";
            Session["fullname"] = "";
            Session["role"] = null;
            Session["status"] = "";

            lnkBtnUserLogin.Visible = true; // user login link button
            lnkBtnSignUp.Visible = true; // sign up link button

            lnkBtnLogout.Visible = false; // logout link button
            lnkBtnGreet.Visible = false; // hello user link button


            lnkBtnAdminLogin.Visible = true; // admin login link button
            lnkBtnAuthorManagement.Visible = false; // author management link button
            lnkBtnPublisherManagement.Visible = false; // publisher management link button
            lnkBtnBookInventory.Visible = false; // book inventory link button
            lnkBtnBookIssuing.Visible = false; // book issuing link button
            lnkBtnMemberManagement.Visible = false; // member management link button

            Response.Redirect("homepage.aspx");
        }

        protected void lnkBtnGreet_Click(object sender, EventArgs e)
        {
            Response.Redirect("userprofile.aspx");
        }
    }
}