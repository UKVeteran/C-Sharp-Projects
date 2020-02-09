using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonSESBulkMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

      

        //Form Load 
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxRegion.SelectedIndex = 0; //Select the first Index by Defualt US WEST (Oregon)
            SelectAWSRegion = RegionEndpoint.USWest2;
        }

        // SES API KEYs LINK
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://console.aws.amazon.com/iam/home?#security_credential");
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
           

            buttonSend.Enabled = false; //avoid multiple clicks
            pictureBoxLoading.Visible = true; //Show loading image (just for design)
            backgroundWorker1.RunWorkerAsync(); //run the code in background for better user experience

        }

        //Import Email list from Text File
        private void buttonTxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;

            DialogResult dr = openFile.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@openFile.FileName);
                string line;
                while ((line = file.ReadLine()) != null)
                {

                    // add to listbox
                    listBoxEMails.Items.Add(line);

                }

                file.Close();


            }
        }

        private RegionEndpoint SelectAWSRegion = RegionEndpoint.USWest2;
        private void comboBoxRegion_SelectedIndexChanged(object sender, EventArgs e)
        {

            // set the Region according to combobox selection
            if (comboBoxRegion.SelectedIndex == 0)
            {
                SelectAWSRegion = RegionEndpoint.USWest2;
            }

            if (comboBoxRegion.SelectedIndex == 1)
            {
                SelectAWSRegion = RegionEndpoint.USEast1;
            }

            if (comboBoxRegion.SelectedIndex == 2)
            {
                SelectAWSRegion = RegionEndpoint.USWest1;
            }



        }

        //Main Sending code using the background worker
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            Amazon.Runtime.AWSCredentials newcred = new Amazon.Runtime.BasicAWSCredentials(textBoxPORT.Text, textBoxUSER.Text);

            using (var client = new AmazonSimpleEmailServiceClient(newcred, SelectAWSRegion))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = textBoxSenderEmail.Text,
                    Destination = new Destination
                    {

                        ToAddresses = listBoxEMails.Items.Cast<String>().ToList()
                        // new List<string> { listBoxEMails.Items.Cast<String>().ToList() }
                    },
                    Message = new Amazon.SimpleEmail.Model.Message
                    {
                        Subject = new Content(textBoxSubject.Text),
                        Body = new Body
                        {
                            
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBoxHTMLBody.Text
                            },
                          
                        }
                    },

                };
                try
                {
                    var response = client.SendEmail(sendRequest);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error message: " + ex.Message);
                }
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonSend.Enabled = true; 
            pictureBoxLoading.Visible = false;
            MessageBox.Show("Finished!");
        }



        

        //AWS SES Single Send Method for customzing (not used here in this application)
        public string SES_SendMessage(string FromEmail, string ToEmail,
            string subject, string Body, string APIKEY, string APISecret, RegionEndpoint region)
        {
            Amazon.Runtime.AWSCredentials newcred = new Amazon.Runtime.BasicAWSCredentials(APIKEY, APISecret);

            using (var client = new AmazonSimpleEmailServiceClient(newcred, region))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = FromEmail,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { ToEmail }
                    },
                    Message = new Amazon.SimpleEmail.Model.Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = Body
                            }
                        }
                    },

                };
                try
                {
                    var response = client.SendEmail(sendRequest);
                    return "Sent";
                }
                catch (Exception ex)
                {

                    return "Error message: " + ex.Message;

                }
            }
        }

     
    }
}
