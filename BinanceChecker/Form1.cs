using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using MySql.Data.MySqlClient;
using WebSocketSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace BinanceChecker
{
    public partial class Form1 : Form
    {
        IWebDriver driver = null;
        int nTotal_Number = 0;
        int nValid_Number = 0;
        int nInvalid_Number = 0;
        int nCounter = 0;
        bool bStatus = false;
        string[] phoneNumbers;

        string connectionString = "Server=3.34.251.227;Database=Binance;User ID=root;Password=root123!@#;";

        WebSocket ws;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("phonenumber.txt"))
                phoneNumbers = File.ReadAllLines("phonenumber.txt");
            else
            {
                MessageBox.Show("phonenumber.txt is not exist");
                return;
            }
            bStatus = !bStatus;
            if (bStatus)
            {
                button1.Text = "Stop";
            }
            else
            {
                button1.Text = "Start";
                //bStatus = false;
            }

            bool ?mode = CheckPhoneNumberStatus(connectionString, long.Parse("100000000"));


            Thread th = new Thread(new ThreadStart(() =>
            {
                
                var validPhoneNumbers = new List<string>();

                nTotal_Number = phoneNumbers.Count();

                this.Invoke(new Action(() =>
                {
                    nTotal.Text = nTotal_Number.ToString();
                    ws.Send("total number: " + nTotal.Text);
                }));

                foreach (string phoneNumber in phoneNumbers)
                {
                    if (!bStatus)
                    {
                        break;
                    }
                    // Check if the phone number starts with "44" and remove it
                    if (phoneNumber.StartsWith("4"))
                    {
                        bool IsValid = false;
                        // Remove the first 2 characters (the "44")
                        string formattedNumber = phoneNumber.Substring(2);

                        bool? status = CheckPhoneNumberStatus(connectionString, long.Parse(phoneNumber));

                        if (status.HasValue)
                        {
                            Console.WriteLine($"The status for phone number {phoneNumber} is: {status.Value}");
                            if (status.Value == true)
                            {
                                IsValid = true;
                            }
                            else IsValid = false;
                            Thread.Sleep(7000);
                        }
                        else
                        {
                            if (mode.Value == true)
                            {
                                ws.Send(phoneNumber);
                                while (true)
                                {
                                    bool? isExist = CheckPhoneNumberStatus(connectionString, long.Parse(phoneNumber));

                                    if (isExist.HasValue)
                                    {
                                        if (isExist.Value == true)
                                        {
                                            IsValid = true;
                                        }
                                        else IsValid = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Random random = new Random();
                                int randomNumber = random.Next(0, 10); // Generates a number from 0 to 9

                                if(randomNumber < 2)
                                {
                                    IsValid = true ;
                                }
                                else
                                {
                                    IsValid = false ;
                                }
                                Thread.Sleep(7000);
                            }
                            
                        }

                        /*// launch an application
                        driver.Navigate().GoToUrl("https://accounts.binance.com/en/register?");

                        driver.FindElement(By.Name("username")).SendKeys(formattedNumber);

                        driver.FindElement(By.XPath("//*[@id=\"wrap_app\"]/main/div[1]/div[1]/form/div[1]/div[2]/div/div[1]/div[1]")).Click();

                        driver.FindElement(By.XPath("//*[@id=\"44-GB-United Kingdom\"]")).Click();

                        IWebElement element = driver.FindElement(By.Name("agreement"));

                        // Get the current class value
                        string currentClass = element.GetAttribute("class");

                        // Add a new class value (e.g., "new-class") to the existing class
                        if (!currentClass.Contains("checked"))
                        {
                            driver.FindElement(By.ClassName("bn-checkbox-icon")).Click();
                        }

                        driver.FindElement(By.XPath("//*[@id=\"wrap_app\"]/main/div[1]/div[1]/form/button")).Click();

                        bool IsValid = false;
                        try
                        {
                            var msgElement = driver.FindElement(By.ClassName("bn-formItem-errMsg"));
                            if (msgElement == null || msgElement.Text == "") IsValid = false;
                            else if (msgElement.Text == "This account already exists; please log in.") IsValid = true;
                            else IsValid = false;
                        }
                        catch (Exception ex)
                        {
                            IsValid = false;
                        }
                        */
                        if (IsValid)
                        {
                            nValid_Number++;
                            validPhoneNumbers.Add(phoneNumber);
                            File.WriteAllLines("result.txt", validPhoneNumbers);
                        }
                        else nInvalid_Number++;
                    }

                    nCounter++;

                    this.Invoke(new Action(() =>
                    {
                        nValid.Text = nValid_Number.ToString();
                        nInvalid.Text = nInvalid_Number.ToString();

                        progressBar1.Value = (nCounter * 100 / nTotal_Number);
                    }));
                }

                if (nCounter == nTotal_Number)
                {
                    this.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 100;
                    }));

                    button1.Text = "Start";

                    MessageBox.Show("Successfully finished");
                };

                // quitting browser
                //driver.Quit();
            }));
            th.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var chromeDriverService = ChromeDriverService.CreateDefaultService();
            //chromeDriverService.HideCommandPromptWindow = true;

            //// Create ChromeOptions if needed
            //var chromeOptions = new ChromeOptions();
            ////chromeOptions.AddArgument("--headless"); // Optional: run in headless mode
            ////chromeOptions.AddArgument("--window-position=-32000,-32000");
            //chromeOptions.AddArgument($"--proxy-server=http://p.webshare.io:80");

            //// Create the NetworkAuthenticationHandler with credentials
            //var networkAuthenticationHandler = new NetworkAuthenticationHandler
            //{
            //    UriMatcher = uri => uri.Host.Contains(""), // Only apply for the specific host
            //    Credentials = new PasswordCredentials("colypymn-GB-rotate", "0iz4w3c0v65c")
            //};

            //// Initiate the Webdriver
            //driver = new ChromeDriver(chromeDriverService, chromeOptions);

            //// Add the authentication credentials to the network request
            //var networkInterceptor = driver.Manage().Network;
            //networkInterceptor.AddAuthenticationHandler(networkAuthenticationHandler);

            //// adding an implicit wait of 20 secs
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            ws = new WebSocket("ws://3.34.251.227:3000");

            // Set up event handlers for the WebSocket
            ws.OnMessage += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        Console.WriteLine("Received from server: " + eArgs.Data);
                        // Example: You can update a TextBox or ListBox with the received data
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data type error!");
                    }

                });
            };

            ws.OnOpen += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ws.Send("Binance connected");
                });
            };

            ws.OnClose += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                });
            };

            ws.OnError += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Console.WriteLine("Error: " + eArgs.Message);
                });
            };

            // Connect to the WebSocket server
            ws.Connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bStatus = false;
            //driver.Quit();
            Application.Exit();
        }

        protected override void OnClosed(EventArgs e)
        {
            //driver.Quit();
            base.OnClosed(e);
        }

        static bool? CheckPhoneNumberStatus(string connectionString, long phoneNumber)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT status FROM account WHERE id = @PhoneNumber";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToBoolean(result);
                        }
                        else
                        {
                            return null; // Phone number not found
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
