using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using static System.Windows.Forms.Design.AxImporter;
using OpenQA.Selenium.Support.UI;
using static OpenQA.Selenium.BiDi.Modules.Script.RemoteValue.WindowProxy;

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
            }
            Thread th = new Thread(new ThreadStart(() =>
            {
                
                var validPhoneNumbers = new List<string>();

                nTotal_Number = phoneNumbers.Count();

                this.Invoke(new Action(() =>
                {
                    nTotal.Text = nTotal_Number.ToString();
                }));

                foreach (string phoneNumber in phoneNumbers)
                {
                    if (!bStatus)
                    {
                        break;
                    }
                    // Check if the phone number starts with "44" and remove it
                    if (phoneNumber.StartsWith("44"))
                    {
                        // Remove the first 2 characters (the "44")
                        string formattedNumber = phoneNumber.Substring(2);

                        // launch an application
                        driver.Navigate().GoToUrl("https://accounts.binance.com/en/register?");

                        driver.FindElement(By.Name("username")).SendKeys(formattedNumber);

                        driver.FindElement(By.XPath("//*[@id=\"wrap_app\"]/main/div[1]/div[1]/form/div[1]/div[2]/div/div[1]/div[1]")).Click();

                        driver.FindElement(By.XPath("//*[@id=\"44-GB-United Kingdom\"]")).Click();

                        IWebElement element = driver.FindElement(By.Name("agreement"));

                        // Get the current class value
                        string currentClass = element.GetAttribute("class");

                        // Add a new class value (e.g., "new-class") to the existing class
                        if(!currentClass.Contains("checked"))
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
                driver.Quit();
            }));
            th.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            // Create ChromeOptions if needed
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless"); // Optional: run in headless mode
            //chromeOptions.AddArgument("--window-position=-32000,-32000");
            //chromeOptions.AddArgument($"--proxy-server=http://p.webshare.io:80");

            // Use proxy credentials (for basic authentication)
            //chromeOptions.AddArgument($"--proxy-auth=colypymn-DE-rotate:0iz4w3c0v65c");

            string proxyAddress = "p.webshare.io:80";   // Proxy address
            string username = "veyqeiwh-DE-HR-PT-rotate";  // Proxy username
            string password = "ciccgwzfxczu";  // Proxy password

            Proxy proxy = new Proxy
            {
                HttpProxy = proxyAddress,
                SslProxy = proxyAddress
            };

            string encodedCredentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            string proxyAuth = $"http://{encodedCredentials}@{proxyAddress}";

            // Inject the proxy with authentication
            chromeOptions.AddArgument($"--proxy-server={proxyAuth}");

            // Initiate the Webdriver
            driver = new ChromeDriver(chromeDriverService, chromeOptions);

            // adding an implicit wait of 20 secs
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bStatus = false;
            driver.Quit();
            Application.Exit();
        }

        protected override void OnClosed(EventArgs e)
        {
            driver.Quit();
            base.OnClosed(e);
        }
    }
}
