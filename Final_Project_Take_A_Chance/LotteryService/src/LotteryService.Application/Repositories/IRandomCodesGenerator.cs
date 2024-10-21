using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Application.Repositories
{
    public interface IRandomCodesGenerator
    {
        HashSet<string> GenerateRandomCodes(int count);
        void SaveCodesToRedis(HashSet<string> randomCodes, int lotteryId);
        List<string> GetCodesFromRedis(int lotteryId, int count);
    }
}
