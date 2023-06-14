using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

class News
{
    public string Title { get; set; }
    public string Source { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
}

class NewsReader
{
    private const string NewsApiBaseUrl = "https://newsapi.org/v2/";
    private const string NewsApiKey = "YOUR_API_KEY";
    private const int PageSize = 5;

    public void ReadNews(string country, string category, int page)
    {
        try
        {
            string apiUrl = $"{NewsApiBaseUrl}top-headlines?country={country}&category={category}&apiKey={NewsApiKey}&pageSize={PageSize}&page={page}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(apiUrl);
                dynamic newsData = JsonConvert.DeserializeObject(json);

                List<News> newsList = new List<News>();

                foreach (var article in newsData.articles)
                {
                    News news = new News
                    {
                        Title = article.title,
                        Source = article.source.name,
                        Description = article.description,
                        Url = article.url
                    };

                    newsList.Add(news);
                }

                DisplayNews(newsList, page, newsData.totalResults);
                ShowMenu(country, category, page);
            }
        }
        catch (WebException ex)
        {
            Console.WriteLine("Error occurred while fetching news. Please try again later.");
            // Log the exception details to a file or database for troubleshooting.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private void DisplayNews(List<News> newsList, int currentPage, int totalResults)
    {
        if (newsList.Count == 0)
        {
            Console.WriteLine("No news articles found.");
            return;
        }

        Console.WriteLine($"Page: {currentPage} of {GetTotalPages(totalResults)}");
        Console.WriteLine("---------------------------------------");

        for (int i = 0; i < newsList.Count; i++)
        {
            News news = newsList[i];

            Console.WriteLine($"[{i + 1}] Title: {news.Title}");
            Console.WriteLine($"    Source: {news.Source}");
            Console.WriteLine($"    Description: {news.Description}");
            Console.WriteLine($"    URL: {news.Url}");
            Console.WriteLine("---------------------------------------");
        }
    }

    private int GetTotalPages(int totalResults)
    {
        return (int)Math.Ceiling((double)totalResults / PageSize);
    }

    private void ShowMenu(string country, string category, int currentPage)
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. View Article Details");
        Console.WriteLine("2. Search News");
        Console.WriteLine("3. Sort News");
        Console.WriteLine("4. Filter by Source");
        Console.WriteLine("5. Exit");
        Console.WriteLine("Enter your choice:");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                ViewArticleDetails(country, category, currentPage);
                break;
            case 2:
                SearchNews(country, category);
                break;
            case 3:
                SortNews(country, category);
                break;
            case 4:
                FilterBySource(country, category);
                break;
            case 5:
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. Exiting...");
                Environment.Exit(0);
                break;
        }
    }

    private void ViewArticleDetails(string country, string category, int currentPage)
    {
        Console.WriteLine("Enter the index of the article to view details:");
        int index = int.Parse(Console.ReadLine());

        // Validate the index
        if (index < 1 || index > PageSize)
        {
            Console.WriteLine("Invalid article index.");
            ShowMenu(country, category, currentPage);
            return;
        }

        // Get the selected article from the newsList
        News selectedArticle = newsList[(currentPage - 1) * PageSize + (index - 1)];

        // Display the article details
        Console.WriteLine($"Title: {selectedArticle.Title}");
        Console.WriteLine($"Source: {selectedArticle.Source}");
        Console.WriteLine($"Description: {selectedArticle.Description}");
        Console.WriteLine($"URL: {selectedArticle.Url}");

        // Prompt the user for further actions
        Console.WriteLine("Press any key to go back to the menu.");
        Console.ReadKey();
        ShowMenu(country, category, currentPage);
    }

    private void SearchNews(string country, string category)
    {
        Console.WriteLine("Enter the search query:");
        string searchQuery = Console.ReadLine();

        // Modify the API request URL to include the search query
        string apiUrl = $"{NewsApiBaseUrl}top-headlines?country={country}&category={category}&apiKey={NewsApiKey}&pageSize={PageSize}&q={searchQuery}";

        // Fetch and display the search results
        // ...
    }

    private void SortNews(string country, string category)
    {
        Console.WriteLine("Select a sorting option:");
        Console.WriteLine("1. Relevance");
        Console.WriteLine("2. Date");
        Console.WriteLine("3. Popularity");
        Console.WriteLine("Enter your choice:");

        int choice = int.Parse(Console.ReadLine());

        // Modify the API request URL based on the selected sorting option
        // ...

        // Fetch and display the sorted news articles
        // ...
    }

    private void FilterBySource(string country, string category)
    {
        Console.WriteLine("Enter the name or ID of the news source:");
        string source = Console.ReadLine();

        // Modify the API request URL to include the source parameter
        string apiUrl = $"{NewsApiBaseUrl}top-headlines?country={country}&category={category}&apiKey={NewsApiKey}&pageSize={PageSize}&sources={source}";

        // Fetch and display the filtered news articles
        // ...
    }
}

class Program
{
    static void Main(string[] args)
    {
        NewsReader newsReader = new NewsReader();

        Console.WriteLine("Enter the country (e.g., us, uk):");
        string country = Console.ReadLine();

        Console.WriteLine("Enter the news category (e.g., business, technology):");
        string category = Console.ReadLine();

        Console.WriteLine("Enter the page number:");
        int page = int.Parse(Console.ReadLine());

        Console.WriteLine("\nFetching news...\n");
        newsReader.ReadNews(country, category, page);
    }
}
