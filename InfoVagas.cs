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

    public void ExibirInfoVagas()
    {
        Console.WriteLine("Exibindo informações das vagas...");
        _driver.Navigate().GoToUrl("https://www.infojobs.com.br/vagas-de-emprego-desenvolvedor-em-distrito-federal-trabalho-home-office.aspx");

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
}