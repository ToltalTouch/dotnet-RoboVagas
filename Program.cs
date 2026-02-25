// See https://aka.ms/new-console-template for more information
Console.WriteLine("Iniciando");

using (var bot = new LinkedInBot())
{
    bot.PerformLogin();
}

Console.WriteLine("Processo finalizado. Pressione qualquer tecla para sair.");
Console.ReadKey();