using OpenQA.Selenium;
using GametrackerServerScraper.Models;
using System.Net;
using System.Collections.ObjectModel;

namespace GametrackerServerScraper.Functional;

public class GametrackerScraper : IScraper
{
    private readonly string url = "https://www.gametracker.com/server_info";
    private readonly IWebDriver driver;

    public GametrackerScraper(IWebDriver driver) 
    {
        this.driver = driver;
    }

    public ServerInfo GetServerInfo(string serverIp)
    {
        driver.Navigate().GoToUrl($"{url}/{serverIp}");

        IWebElement nameElement = driver.FindElement(By.ClassName("blocknewheadertitle"));
        IWebElement currentMapElement = driver.FindElement(By.Id("HTML_curr_map"));
        IWebElement currentPlayersElement = driver.FindElement(By.Id("HTML_num_players"));
        IWebElement maxPlayersElement = driver.FindElement(By.Id("HTML_max_players"));

        List<string> players = [];
        try 
        {
            IEnumerable<IWebElement> playersListElement = driver.FindElement(By.Id("HTML_online_players"))
                .FindElement(By.ClassName("table_lst_stp"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr")).Skip(1)
            ;

            foreach (IWebElement playerElement in playersListElement)
            {
                ReadOnlyCollection<IWebElement> cells = playerElement.FindElements(By.TagName("td"));

                string name = cells[1].FindElement(By.TagName("a")).Text.Trim();

                players.Add(name);
            }
        }
        catch (NoSuchElementException) 
        {
            // No players online
        }
        

        ServerInfo info = new ServerInfo
        {
            Name = nameElement.Text,
            CurrentMap = currentMapElement.Text,
            CurrentPlayers = int.Parse(currentPlayersElement.Text ?? "0"),
            MaxPlayers = int.Parse(maxPlayersElement.Text ?? "0"),
            Players = players
        };

        return info;
    }

    public void Dispose() 
    {
        driver.Close();
    }
}