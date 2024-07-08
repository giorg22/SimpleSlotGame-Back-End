using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Shared;
using Application.Slots.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace SlotGame.ConsoleClient
{
    class Program
    {
        static string baseUrl = "https://localhost:7127";
        static string hubUrl = $"{baseUrl}/slotshub";
        static string apiUrl = $"{baseUrl}/api/user/login";
        private static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            connection.On<Response<SpinResultResponse>>("ReceiveSpinResult", result =>
            {
                if(result.Success == false)
                {
                    Console.WriteLine($"Error: {result.Error}");
                }
                else
                {
                    var response = result.Data;
                    Console.WriteLine($"Spin Result: {response.Result} | Win Amount: {response.WinAmount} | Current Balance: {response.CurrentBalance}");
                }
            });

            await connection.StartAsync();

            Console.WriteLine("Enter username for login or registration");
            var username = Console.ReadLine();
            var userId = await Login(username);

            while (true)
            {
                Console.WriteLine("Enter bet amount to spin");
                var betAmount = Console.ReadLine();
                if(decimal.TryParse(betAmount, out var betAmount2))
                {
                    await connection.InvokeAsync("Spin", userId, betAmount2);
                }
                else
                {
                    Console.WriteLine("Invalid bet amount. Please enter a valid decimal number.");
                }
            }

            await connection.StopAsync();
        }

        public static async Task<string> Login(string username)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{apiUrl}/{username}", null);
            var result = await response.Content.ReadFromJsonAsync<Response<string>>();
            return result.Data;
        }
    }
}
