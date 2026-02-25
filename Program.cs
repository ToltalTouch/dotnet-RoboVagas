// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

var driverService = EdgeDriverService.CreateDefaultService();
driverService.HideCommandPromptWindow = true;

Console.WriteLine("Iniciando");

using (IWebDriver driver = new EdgeDriver(driverService))
{
    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));

    try
    {
        Console.WriteLine("Acessando Linkedin para login");
        
        driver.Navigate().GoToUrl("https://www.linkedin.com/uas/login?session_redirect=%2Foauth%2Fv2%2Flogin-success%3Fapp_id%3D108833024%26auth_type%3DAC%26flow%3Dkey%253A8ba3652a-1714-4b58-a76d-29e091d6bdc7&fromSignIn=1&trk=oauth&cancel_redirect=%2Foauth%2Fv2%2Flogin-cancel%3Fapp_id%3D108833024%26auth_type%3DAC%26flow%3Dkey%253A8ba3652a-1714-4b58-a76d-29e091d6bdc7");
        
        string loginUrl = driver.Url;

        Console.WriteLine("Realizando login");
        Console.Write("Digite seu email: ");
        IWebElement usernameInput = wait.Until(e => e.FindElement(By.Id("username")));
        usernameInput.SendKeys(Console.ReadLine() ?? "");

        Console.Write("Digite sua senha: ");
        IWebElement passwordInput = wait.Until(e => e.FindElement(By.Id("password")));
        passwordInput.SendKeys(ReadPassword());
        passwordInput.SendKeys(Keys.Enter);

        try
        {
            wait.Until(d => d.Url != loginUrl);
            Console.WriteLine("Login bem-sucedido!");
            Console.WriteLine("Nova URL: " + driver.Title);
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Falha no login: URL não mudou após o tempo limite.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ocorreu um erro: " + ex.Message);
    }
}

static string ReadPassword()
{
    string password = "";
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true);

        if (!char.IsControl(key.KeyChar))
        {
            password += key.KeyChar;
            Console.Write("*");
        }
        else
        {
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }
    }
    while (key.Key != ConsoleKey.Enter);

    Console.WriteLine();
    return password;
}