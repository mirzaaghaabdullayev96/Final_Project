using LotteryService.Application.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Infrastructure.Repositories
{
    public class RandomCodesGenerator : IRandomCodesGenerator
    {
        private readonly IDatabase _redisDatabase;
        private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly Random _random = new();

        public RandomCodesGenerator(IConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
        }
        public HashSet<string> GenerateRandomCodes(int count)
        {
            var randomCodes = new HashSet<string>();

            while (randomCodes.Count < count)
            {
                var code = GenerateRandomCode();
                randomCodes.Add(code);
            }

            return randomCodes;
        }

        public void SaveCodesToRedis(HashSet<string> randomCodes, int lotteryId)
        {
            var redisKey = $"Lottery:{lotteryId}:TicketCodes";

            _redisDatabase.KeyDelete(redisKey);

            foreach (var code in randomCodes)
            {
                _redisDatabase.ListRightPush(redisKey, code);
            }
        }


        public List<string> GetCodesFromRedis(int lotteryId, int count)
        {
            var redisKey = $"Lottery:{lotteryId}:TicketCodes";
            var codes = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var code = _redisDatabase.ListLeftPop(redisKey);
                if (!string.IsNullOrEmpty(code))
                {
                    codes.Add(code);
                }
                else
                {
                    throw new InvalidOperationException("Not enough codes");
                }
            }

            return codes;
        }

        private string GenerateRandomCode()
        {
            var code = new char[5];

            code[0] = _letters[_random.Next(_letters.Length)];
            code[1] = _letters[_random.Next(_letters.Length)];

            var number = _random.Next(100, 1000);
            var numberString = number.ToString();
            code[2] = numberString[0];
            code[3] = numberString[1];
            code[4] = numberString[2];

            return new string(code);
        }
    }
}
