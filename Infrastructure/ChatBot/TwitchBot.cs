using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.ChatBot
{
    public class TwitchBot : BackgroundService, ITwitchbot
    {

        private readonly string _ip = "irc.twitch.tv";
        private readonly int _port = 6667;

        private TcpClient _tcpClient;
        private StreamReader _inputStream;
        private StreamWriter _outputStream;
        private Thread _readMessageThread;
        private CancellationToken _cancellationToken;
        
        public bool IsRunning { get; set; }
        public Bot Bot { get; set; }
        public Settings Settings { get; set; }

        public async void StartBot()
        {
            IsRunning = true;
            Console.WriteLine("yo in start");
            InitializeBot();
            ConnectToChannel();
            _cancellationToken = new CancellationToken();
            await ExecuteAsync(_cancellationToken);
        }
        
        public void StopBot()
        {
            IsRunning = false;
            Console.WriteLine("yo in stop");
            Console.WriteLine(_readMessageThread.IsBackground);
            _tcpClient = null;
            _inputStream = null;
            _outputStream = null;
            IsRunning = false;
        }

        private void InitializeBot()
        {
            Console.WriteLine("Initializing");
            try
            {
                _tcpClient = new TcpClient(_ip, _port);
                _inputStream = new StreamReader(_tcpClient.GetStream());
                _outputStream = new StreamWriter(_tcpClient.GetStream());
                Console.WriteLine("Initialization successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Initializing failed");
            }
        }

        private void ConnectToChannel()
        {
            Console.WriteLine("Connecting to channel");
            try
            {
                _outputStream.WriteLine("PASS " + Bot.TMIToken);
                _outputStream.WriteLine("NICK " + Bot.TMIToken);
                _outputStream.WriteLine("JOIN #" + Settings.TargetChannel);
                _outputStream.Flush();
                Console.WriteLine("Connecting to channel successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Connecting to channel failed");
            }
        }

        private string ReadMessage()
        {
            try
            {
                string message = _inputStream.ReadLine();
                return message;
            }
            catch (Exception ex)
            {
                return "Error receiving message: " + ex.Message;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Reading");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = ReadMessage();
                    if (message.Contains("PRIVMSG"))
                    {
                        Console.WriteLine("yo in privmsg");
                    }
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                }
                
            }
        }
    }
}