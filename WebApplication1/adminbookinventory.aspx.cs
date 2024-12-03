using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class adminbookinventory : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        static string global_filepath;
        static int global_actual_stock, global_current_stock, global_issued_books;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null)
            {
                Response.Redirect("homepage.aspx");
            }
            else if(Session["role"].Equals("user"))
            {
                Response.Redirect("homepage.aspx");
            }
            if (!IsPostBack)
            {
                fillAuthorPublisherValues();
            }

            grdViewBookDetails.DataBind();

        }

        protected void lnkGetBookDetails_Click(object sender, EventArgs e)
        {
            getBookByID();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkIfBookExists())
            {
                Response.Write("<script>alert('Book Already Exists, try some other Book ID');</script>");
            }
            else
            {
                addNewBook();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            updateBookByID();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            deleteBookByID();
        }

        void deleteBookByID()
        {
            if (checkIfBookExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("DELETE from book_master_tbl WHERE book_id='" + txtBookID.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Book Deleted Successfully');</script>");

                    grdViewBookDetails.DataBind();

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

        void updateBookByID()
        {

            if (checkIfBookExists())
            {
                try
                {

                    int actual_stock = Convert.ToInt32(txtActualStock.Text.Trim());
                    int current_stock = Convert.ToInt32(txtCurrentStock.Text.Trim());

                    if (global_actual_stock == actual_stock)
                    {

                    }
                    else
                    {
                        if (actual_stock < global_issued_books)
                        {
                            Response.Write("<script>alert('Actual Stock value cannot be less than the Issued books');</script>");
                            return;


                        }
                        else
                        {
                            current_stock = actual_stock - global_issued_books;
                            txtCurrentStock.Text = "" + current_stock;
                        }
                    }

                    string genres = "";
                    foreach (int i in lbxGenre.GetSelectedIndices())
                    {
                        genres = genres + lbxGenre.Items[i] + ",";
                    }
                    genres = genres.Remove(genres.Length - 1);

                    string filepath = "~/book_inventory/books1";
                    string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (filename == "" || filename == null)
                    {
                        filepath = global_filepath;

                    }
                    else
                    {
                        FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                        filepath = "~/book_inventory/" + filename;
                    }

                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE book_master_tbl set book_name=@book_name, genre=@genre, author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, language=@language, edition=@edition, book_cost=@book_cost, no_of_pages=@no_of_pages, book_description=@book_description, actual_stock=@actual_stock, current_stock=@current_stock, book_img_link=@book_img_link where book_id='" + txtBookID.Text.Trim() + "'", con);

                    cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", genres);
                    cmd.Parameters.AddWithValue("@author_name", ddlAuthorName.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@publisher_name", ddlPulisherName.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@publish_date", txtPublishDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@language", ddlLanguages.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@edition", txtEdition.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_cost", txtBookCost.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_of_pages", txtPages.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_description", txtBookDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@actual_stock", actual_stock.ToString());
                    cmd.Parameters.AddWithValue("@current_stock", current_stock.ToString());
                    cmd.Parameters.AddWithValue("@book_img_link", filepath);


                    cmd.ExecuteNonQuery();
                    con.Close();
                    grdViewBookDetails.DataBind();
                    Response.Write("<script>alert('Book Updated Successfully');</script>");


                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid Book ID');</script>");
            }
        }


        void getBookByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * from book_master_tbl WHERE book_id='" + txtBookID.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    txtBookName.Text = dt.Rows[0]["book_name"].ToString();
                    txtPublishDate.Text = dt.Rows[0]["publish_date"].ToString();
                    txtEdition.Text = dt.Rows[0]["edition"].ToString();
                    txtBookCost.Text = dt.Rows[0]["book_cost"].ToString().Trim();
                    txtPages.Text = dt.Rows[0]["no_of_pages"].ToString().Trim();
                    txtActualStock.Text = dt.Rows[0]["actual_stock"].ToString().Trim();
                    txtCurrentStock.Text = dt.Rows[0]["current_stock"].ToString().Trim();
                    txtBookDescription.Text = dt.Rows[0]["book_description"].ToString();
                    txtIssuedBooks.Text = "" + (Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString()) - Convert.ToInt32(dt.Rows[0]["current_stock"].ToString()));

                    ddlLanguages.SelectedValue = dt.Rows[0]["language"].ToString().Trim();
                    ddlPulisherName.SelectedValue = dt.Rows[0]["publisher_name"].ToString().Trim();
                    ddlAuthorName.SelectedValue = dt.Rows[0]["author_name"].ToString().Trim();

                    lbxGenre.ClearSelection();
                    string[] genre = dt.Rows[0]["genre"].ToString().Trim().Split(',');
                    for (int i = 0; i < genre.Length; i++)
                    {
                        for (int j = 0; j < lbxGenre.Items.Count; j++)
                        {
                            if (lbxGenre.Items[j].ToString() == genre[i])
                            {
                                lbxGenre.Items[j].Selected = true;

                            }
                        }
                    }

                    global_actual_stock = Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString().Trim());
                    global_current_stock = Convert.ToInt32(dt.Rows[0]["current_stock"].ToString().Trim());
                    global_issued_books = global_actual_stock - global_current_stock;
                    global_filepath = dt.Rows[0]["book_img_link"].ToString();

                }
                else
                {
                    Response.Write("<script>alert('Invalid Book ID');</script>");
                }

            }
            catch (Exception ex)
            {

            }
        }

        void fillAuthorPublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT author_name from author_master_tbl;", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlAuthorName.DataSource = dt;
                ddlAuthorName.DataValueField = "author_name";
                ddlAuthorName.DataBind();

                cmd = new SqlCommand("SELECT publisher_name from publisher_master_tbl;", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                ddlPulisherName.DataSource = dt;
                ddlPulisherName.DataValueField = "publisher_name";
                ddlPulisherName.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        bool checkIfBookExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from book_master_tbl where book_id='" + txtBookID.Text.Trim() + "' OR book_name='" + txtBookName.Text.Trim() + "';", con);
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

        void addNewBook()
        {
            try
            {
                string genres = "";
                foreach (int i in lbxGenre.GetSelectedIndices())
                {
                    genres = genres + lbxGenre.Items[i] + ",";
                }
                // genres = Adventure,Self Help,
                genres = genres.Remove(genres.Length - 1);

                string filepath = "~/book_inventory/books1.png";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                filepath = "~/book_inventory/" + filename;


                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO book_master_tbl(book_id,book_name,genre,author_name,publisher_name,publish_date,language,edition,book_cost,no_of_pages,book_description,actual_stock,current_stock,book_img_link) values(@book_id,@book_name,@genre,@author_name,@publisher_name,@publish_date,@language,@edition,@book_cost,@no_of_pages,@book_description,@actual_stock,@current_stock,@book_img_link)", con);

                cmd.Parameters.AddWithValue("@book_id", txtBookID.Text.Trim());
                cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", genres);
                cmd.Parameters.AddWithValue("@author_name", ddlAuthorName.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publisher_name", ddlPulisherName.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@publish_date", txtPublishDate.Text.Trim());
                cmd.Parameters.AddWithValue("@language", ddlLanguages.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@edition", txtEdition.Text.Trim());
                cmd.Parameters.AddWithValue("@book_cost", txtBookCost.Text.Trim());
                cmd.Parameters.AddWithValue("@no_of_pages", txtPages.Text.Trim());
                cmd.Parameters.AddWithValue("@book_description", txtBookDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@actual_stock", txtActualStock.Text.Trim());
                cmd.Parameters.AddWithValue("@current_stock", txtActualStock.Text.Trim());
                cmd.Parameters.AddWithValue("@book_img_link", filepath);

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Book added successfully.');</script>");
                grdViewBookDetails.DataBind();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}