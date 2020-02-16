using System;
using System.IO;
using System.Net.Sockets;
using Application.Interfaces;
using Domain;

namespace Infrastructure.ChatBot
{
    public class QuestionsBot : ITwitchbot
    {
        private readonly string _ip = "irc.twitch.tv";
        private readonly int _port = 6667;

        private TcpClient _tcpClient;
        private StreamReader _inputStream;
        private StreamWriter _outputStream;

        public bool IsRunning { get; set; }
        public Bot Bot { get; set; }
        public Settings Settings { get; set; }

        public bool StartBot()
        {
            try
            {
                Console.WriteLine("yo in start");
                InitializeBot();
                ConnectToChannel();
                IsRunning = true;
                ReadMessages();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong in StartBot: " + ex);
                return false;
            }
        }

        public void StopBot()
        {
            IsRunning = false;
            Console.WriteLine("yo in stop");
            _tcpClient = null;
            _inputStream = null;
            _outputStream = null;
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
                _outputStream.WriteLine("NICK " + Bot.Nick);
                _outputStream.WriteLine("JOIN #" + Settings.TargetChannel);
                _outputStream.Flush();
                Console.WriteLine("Pass: " + Bot.TMIToken);
                Console.WriteLine("Nick: " + Bot.Nick);
                Console.WriteLine("Target: " + Settings.TargetChannel);
                Console.WriteLine("Connecting to channel successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Connecting to channel failed");
            }
        }

        private void ReadMessage()
        {
            try
            {
                string message = _inputStream.ReadLine();
                Console.Out.WriteLine(message);
                if (message.Contains("PRIVMSG"))
                {
                    // Console.WriteLine("From: " + getUserFromRawMessage(message));
                    string userName = getUserFromRawMessage(message);
                    // Console.WriteLine("Message: " + getMessageContentFromRawMessage(message));
                    string messageContent = getMessageContentFromRawMessage(message);
                    CheckMessage(userName, messageContent);
                }
                else if(message.Contains("PING :tmi.twitch.tv"))
                {
                    RespondToPing();
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Problem reading message: " + ex.Message);
            }
        }

        private string getUserFromRawMessage(string message)
        {
            int indexOfParseSign = message.IndexOf('!');
            // Console.WriteLine(indexOfParseSign);
            // Skips the ":" at the beginning cuts of the "!" at the end
            string userName = message.Substring(1, indexOfParseSign - 1);
            // Console.WriteLine(userName);
            return userName;
        }

        private string getMessageContentFromRawMessage(string message)
        {
            int indexOfParseSign = message.IndexOf(" :");
            // The " :" marks the beginning of the message the message
            string messageContent = message.Substring(indexOfParseSign + 2);
            return messageContent;
        }

        private void ReadMessages()
        {
            Console.WriteLine("Reading");
            while (IsRunning)
            {
                try
                {
                    ReadMessage();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Rip reading messages: " + e);
                }
            }
        }
        private void SendWhisperMessage(string userName, string messageContent)
        {
            try
            {
                string ircStyleMessage = ":" + Bot.Nick + "!" + Bot.Nick + "@" + Bot.Nick +
                                         ".tmi.twitch.tv PRIVMSG #" + Settings.TargetChannel + " :" + userName + " " + messageContent;
                Console.WriteLine(ircStyleMessage);
                SendIrcMessage(ircStyleMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        private void SendMessage(string messageContent)
        {
            try
            {
                string ircStyleMessage = ":" + Bot.Nick + "!" + Bot.Nick + "@" + Bot.Nick +
                                         ".tmi.twitch.tv PRIVMSG #" + Settings.TargetChannel + " :" + messageContent;
                Console.WriteLine(ircStyleMessage);
                SendIrcMessage(ircStyleMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        private void RespondToPing()
        {
            try
            {
                string ircStyleMessage = "PONG :tmi.twitch.tv";
                Console.WriteLine(ircStyleMessage);
                SendIrcMessage(ircStyleMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SendIrcMessage(string ircStyleMessage)
        {
            _outputStream.WriteLine(ircStyleMessage);
            _outputStream.Flush();
        }

        private void CheckMessage(string userName, string messageContent)
        {
            if (messageContent.StartsWith( Settings.Prefix + "q "))
            {
                WriteQuestion(userName);
            } else if (messageContent.StartsWith(Settings.Prefix + "h"))
            {
                DisplayHelp();
            }
        }

        private void WriteQuestion(string userName)
        {
            string thankYouMessage = "Thanks for asking the question! We'll look at it shortly";
            SendWhisperMessage(userName, thankYouMessage);
        }

        private void DisplayHelp()
        {
            string infoMessage = "To ask a question please type: \"" + Settings.Prefix + "q <your message>\"";
            SendMessage(infoMessage);
        }
    }
}