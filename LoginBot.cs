using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

public class LinkedInBot : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public LinkedInBot()
    {
        var driverService = EdgeDriverService.CreateDefaultService();
        driverService.HideCommandPromptWindow = true;
        _driver = new EdgeDriver(driverService);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(120));
    }

    public void PerformLogin()
    {
        try
        {
            Console.WriteLine("Acessando Linkedin para login");
            _driver.Navigate().GoToUrl("https://www.linkedin.com/uas/login?session_redirect=%2Foauth%2Fv2%2Flogin-success%3Fapp_id%3D108833024%26auth_type%3DAC%26flow%3Dkey%253A1bdfba01-b590-4a4e-8d63-75bd396635f7&fromSignIn=1&trk=oauth&cancel_redirect=%2Foauth%2Fv2%2Flogin-cancel%3Fapp_id%3D108833024%26auth_type%3DAC%26flow%3Dkey%253A1bdfba01-b590-4a4e-8d63-75bd396635f7");
            string loginUrl = _driver.Url;

            Console.WriteLine("Realizando login");
            Console.Write("Digite seu email: ");
            IWebElement usernameInput = _wait.Until(e => e.FindElement(By.Id("username")));
            usernameInput.SendKeys(Console.ReadLine() ?? "");

            Console.Write("Digite sua senha: ");
            IWebElement passwordInput = _wait.Until(e => e.FindElement(By.Id("password")));
            passwordInput.SendKeys(ConsoleHelper.ReadPassword());
            passwordInput.SendKeys(Keys.Enter);

            try
            {
                _wait.Until(d => d.Url != loginUrl);
                Console.WriteLine("Login bem-sucedido!");
                Console.WriteLine("Título da página: " + _driver.Title);
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

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}