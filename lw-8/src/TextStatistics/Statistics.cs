using System;
using System.Collections.Generic;

namespace TextStatistics
{
    public class Statistics
    {
        private int _textNum;
		private int _highRankPart;
		private float _avgRank;
		private float _totalRank;
        Redis _redis = new Redis();
        public Statistics()
        {
            _textNum = 0;
            _highRankPart = 0;
            _avgRank = 0.0f;
            _totalRank = 0.0f;

            string value = _redis.GetStatistics();
            if (value != null)
			{
				Console.WriteLine("Get statistics from database");
				var tokens = value.ToString().Split(":");
				if (tokens.Length == 4)
				{
					_textNum = int.Parse(tokens[0]);
					_highRankPart = int.Parse(tokens[1]);
					_avgRank = float.Parse(tokens[2]);
					_totalRank = float.Parse(tokens[3]);
				}
			}

        }
        public void Update(string id)
		{
			float newRank = float.Parse(_redis.Get("rank:" + id));
			Console.WriteLine("newRank: " + newRank);
            ++_textNum;
            _totalRank += newRank;
			_avgRank = _totalRank / _textNum;
            if (newRank > 0.5f)
			{
				++_highRankPart;
			}

            _redis.Add(new KeyValuePair<string, string>("statistics",
						_textNum + ":" + _highRankPart + ":" +
						_avgRank + ":" + _totalRank));
		}
  
  
    }
}
