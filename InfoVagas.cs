using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

public class InfoVagas
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public InfoVagas()
    {
        var driverService = EdgeDriverService.CreateDefaultService();
        driverService.HideCommandPromptWindow = true;
        _driver = new EdgeDriver(driverService);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(120));
    }

    private void HandleAlert()
    {
        try
        {
            Thread.Sleep(1000);
            IAlert alert = _driver.SwitchTo().Alert();
            Console.WriteLine($"Alerta encontrado com o texto: {alert.Text}");
            alert.Accept();
            Console.WriteLine("Alerta aceito.");
        }
        catch (NoAlertPresentException)
        {
            Console.WriteLine("Nenhum alerta presente.");
        }
        catch (WebDriverException ex)
        {
            Console.WriteLine($"Erro ao lidar com o alerta: {ex.Message}");
        }
    }

    private void CookiesAlert()
    {
        try
        {
            IWebElement agreeButton = _wait.Until(d => d.FindElement(By.Id("didomi-notice-agree-button")));
            Console.WriteLine("Aceitando cookies...");
            agreeButton.Click();
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Botão de consentimento de cookies não encontrado ou tempo limite excedido.");
        }
    }

    private void VagasLoop()
    {
        var jobElements = _wait.Until(d => d.FindElements(By.XPath("//*[@id='filterSideBar']/div[1]/div[1]")));
        foreach (var jobElement in jobElements)
        {
            Console.WriteLine("Título da vaga: " + jobElement.Text);

            try
            {
                var linkElement = jobElement.FindElement(By.XPath(".//*[@id='VacancyHeader']/div[3]/div[1]/a"));
                Console.WriteLine("Link da vaga: " + linkElement.GetAttribute("href"));
                linkElement.Click();
                Console.WriteLine("Candidatura realizada para a vaga: " + jobElement.Text);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Link da vaga não encontrado.");
            }

        }
    }

    public void ExibirInfoVagas()
    {
        Console.WriteLine("Exibindo informações das vagas...");
        _driver.Navigate().GoToUrl("https://www.infojobs.com.br/vagas-de-emprego-desenvolvedor-em-distrito-federal-trabalho-home-office.aspx");

        HandleAlert();
        CookiesAlert();
        VagasLoop();
    }
}