using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.IO;

public partial class Contact2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Visible = false;
        }
       
    }

    private void DisplayLabelMessage(string message)
    {
        lblMessage.Text = message;
        lblMessage.ForeColor = System.Drawing.Color.Red;
        lblMessage.Font.Bold = true;
        lblMessage.Visible = true;
    }
    private void ResetFields()
    {
        txtMessage.Text = "";
        txtName.Text = "";
        txtSender.Text = "";        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string ext = Path.GetExtension(txtAttachment.PostedFile.FileName);
            bool validFormat = false;
            switch (ext)
            {
                case ".xls":
                case ".xlsx":
                case ".doc":
                case ".docx":
                case ".pdf":
                case ".jpg":
                case ".png":
                case ".gif":
                case "":
                    validFormat = true;
                    break;
                default:
                    validFormat = false;
                    break;
            }
            if (validFormat)
            {
                string message = SendEmail();
                if (message.ToString() == "true") {
                    DisplayLabelMessage(txtName.Text.ToUpper() + ", Thank you for contancting us!");
                    ResetFields();
                }
                else
                {
                    DisplayLabelMessage("something went wrong: " + message);                    
                }
                
            }
            else
                DisplayLabelMessage("Invalid file format attached!");
        }
        catch (Exception) { }
    }

    public string SendEmail()
    {
        
        MailMessage mm = new MailMessage();
        mm.From = new MailAddress(txtSender.Text);
        mm.Subject = "Contact Us Email From Website";
        string messageBody = "You have received inquiry from contact us form.<br><br>" + "Name: " + txtName.Text + "<br>" + "Email: " + txtSender.Text + "<br>" + "Message: " + txtMessage.Text + "<br>";
        mm.Body = messageBody;
        mm.IsBodyHtml = true;

        mm.To.Add(new MailAddress("srali@archplasticsllc.com"));
        //mm.To.Add(new MailAddress("darshitapatel@hotmail.com"));
      
        if (txtAttachment.PostedFile != null)
        {
            try
            {
                string strFileName =
                System.IO.Path.GetFileName(txtAttachment.PostedFile.FileName);
                Attachment attachFile =
                new Attachment(txtAttachment.PostedFile.InputStream, strFileName);
                mm.Attachments.Add(attachFile);
            }
            catch
            {

            }
        }
        SmtpClient smtp = new SmtpClient();
        try
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential("ralisujata@gmail.com", "lifeis2good");
            smtp.EnableSsl = true; //Depending on server SSL Settings true/false
            smtp.Timeout = 20000;
            smtp.Send(mm);
            return "true";

        }
        catch(Exception ex)
        {
            mm.Dispose();
            smtp = null;
            return ex.ToString();
        }

    }
}