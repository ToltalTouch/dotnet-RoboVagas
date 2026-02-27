using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

public class InfoVagas
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private bool _disposed = false;

    public InfoVagas()
    {
        var driverService = EdgeDriverService.CreateDefaultService();
        driverService.HideCommandPromptWindow = true;

        var options = new EdgeOptions();
        options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2);
        options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);

        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-blink-features=AutomationControlled");

        _driver = new EdgeDriver(driverService, options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(120));
    }

    private void CookiesAlert()
    {
        try
        {
            IWebElement agreeButton = _wait.Until(driver =>
            {
                try
                {
                    var el = driver.FindElement(By.Id("didomi-notice-agree-button"));
                    return (el.Displayed && el.Enabled) ? el : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
            Console.WriteLine("Aceitando cookies...");
            agreeButton.Click();
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Botão de consentimento de cookies não encontrado ou tempo limite excedido.");
        }
    }

    private void RowDown()
    {
        Console.WriteLine("Rolando a página o maximo para baixo...");
        for (int i = 0; i < 10; i++)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, document.body.scrollHeight);");
            Thread.Sleep(1000);
        }
    }

    private void VagasLoop()
    {
        var jobElements = _wait.Until(d => d.FindElements(By.XPath("//*[@id='filterSideBar']/div")));

        if (jobElements.Count == 0)
        {
            Console.WriteLine("Nenhuma vaga encontrada.");
            return;
        }
        else
        {
            try
            {
                var vagasRows = _wait.Until(d => d.FindElements(By.ClassName("grid-row")));
                foreach (var vagaRow in vagasRows)
                {
                    Console.WriteLine("Título da vaga: " + vagaRow.Text);

                    try
                    {
                        var linkElement = vagaRow.FindElement(By.XPath(".//*[@id='VacancyHeader']/div[3]/div[1]/a"));
                        Console.WriteLine("Link da vaga: " + linkElement.GetAttribute("href"));
                        linkElement.Click();
                        Console.WriteLine("Candidatura realizada para a vaga: " + vagaRow.Text);
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Link da vaga não encontrado.");
                    }
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Elementos de vaga não encontrados ou tempo limite excedido.");
                return;
            }
        }
    }

    public void ExibirInfoVagas()
    {
        Console.WriteLine("Exibindo informações das vagas...");
        _driver.Navigate().GoToUrl("https://www.infojobs.com.br/vagas-de-emprego-desenvolvedor-em-distrito-federal-trabalho-home-office.aspx");

        CookiesAlert();
        RowDown();
        VagasLoop();
    }
}