using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using static System.Windows.Forms.Design.AxImporter;

namespace BinanceChecker
{
    public partial class Form1 : Form
    {
        IWebDriver driver;
        int nTotal_Number = 0;
        int nValid_Number = 0;
        int nInvalid_Number = 0;
        int nCounter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] phoneNumbers = File.ReadAllLines("phonenumber.txt");
            var validPhoneNumbers = new List<string>();

            nTotal_Number = phoneNumbers.Count();
            nTotal.Text = nTotal_Number.ToString();

            foreach (string phoneNumber in phoneNumbers)
            {
                // Check if the phone number starts with "44" and remove it
                if (phoneNumber.StartsWith("44"))
                {
                    // Remove the first 2 characters (the "44")
                    string formattedNumber = phoneNumber.Substring(2);

                    validPhoneNumbers.Add(phoneNumber);

                    // launch an application
                    driver.Navigate().GoToUrl("https://accounts.binance.com/en/register?");

                    driver.FindElement(By.Name("username")).SendKeys(formattedNumber);

                    driver.FindElement(By.XPath("//*[@id=\"wrap_app\"]/main/div[1]/div[1]/form/div[1]/div[2]/div/div[1]/div[1]")).Click();

                    driver.FindElement(By.XPath("//*[@id=\"44-GB-United Kingdom\"]/div/div/div")).Click();

                    driver.FindElement(By.XPath("//*[@id=\"wrap_app\"]/main/div[1]/div[1]/form/button")).Click();

                    bool IsValid = false;
                    try
                    {
                        var msgElement = driver.FindElement(By.ClassName("bn-formItem-errMsg"));
                        if (msgElement == null) IsValid = false;
                        else if (msgElement.Text == "This account already exists; please log in.") IsValid = true;
                        else IsValid = false;
                    }
                    catch (Exception ex)
                    {
                        IsValid = false;
                    }

                    if (IsValid)
                    {
                        MessageBox.Show("Valid");
                        nValid_Number++;
                        
                    }
                    else MessageBox.Show("Invalid");
                }

                nCounter++;
                nInvalid_Number = nTotal_Number - nValid_Number;

                nValid.Text = nValid_Number.ToString();
                nInvalid.Text = nInvalid_Number.ToString();

                progressBar1.Value = (nCounter / nTotal_Number) * 100;
            }

            // quitting browser
            driver.Quit();

            File.WriteAllLines("result.txt", validPhoneNumbers);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            // Create ChromeOptions if needed
            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless"); // Optional: run in headless mode
            //chromeOptions.AddArgument("--window-position=-32000,-32000");

            // Initiate the Webdriver
            driver = new ChromeDriver(chromeDriverService, chromeOptions);

            // adding an implicit wait of 20 secs
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            driver.Quit();
        }

        protected override void OnClosed(EventArgs e)
        {
            driver.Quit();
            base.OnClosed(e);
        }
    }
}
