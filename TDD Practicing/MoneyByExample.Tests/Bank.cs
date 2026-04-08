using System;
using System.Collections.Generic;
using System.Text;

namespace TDDByExample
{
    public  class Bank
    {
        public  Dictionary<string, List<Pair>> _rateChangeTable =
            new Dictionary<string, List<Pair>>();
          

        public  int GetRateChangeFromTo(string from, string to)
        {
            int rate = 1;
            List<Pair> currenyTable = null;

            if (from.Equals(to))
                return rate;

            if (_rateChangeTable.ContainsKey(from))
            {
                currenyTable = _rateChangeTable[from];
                rate = currenyTable.GetPairTo(to).Rate;

            }else if (_rateChangeTable.ContainsKey(to))
            {
                currenyTable = _rateChangeTable[to];
                rate = currenyTable.GetPairTo(from).Rate;
            }
            else
            {
                throw new RatePairChangeNotFoundException($"The pair {from} -> {to} or {to} -> {from} not suported by bank.");
            }



                return rate;
        }
        public  bool AddRate(string from, string to, int rateValue)
        {
            var isFromExist = IsFromExist(from);
            var isToExist = IsToExist(from, to);

            if (isFromExist && isToExist)
                return false;

            if (isFromExist && !isToExist)
            {
                AddPairToCurrency(from, to, rateValue);
                return true;
            }
            CreateCurrencyWithPair(from, to, rateValue);
            return true;

        }
        private void AddPairToCurrency(string from, string to, int rateValue)
        {
            var pair = CreatePair(from, to, rateValue);
            AddPair(pair, _rateChangeTable[from]);
        }
        private Pair CreatePair(string from, string to, int rateValue)
        {
            return new Pair(from, to, rateValue);
        }
        private static void AddPair(Pair pair, List<Pair> currencyTable)
        {
            currencyTable.Add(pair);
        }
        private void CreateCurrencyWithPair(string from,string to,int rateValue)
        {
            var pairs = new List<Pair>();
            ;
            AddPair(CreatePair(from, to, rateValue), pairs);
            _rateChangeTable.Add(from, pairs);
        }
        private bool IsFromExist(string from)
        {
            return _rateChangeTable.ContainsKey(from);
        }
        private bool IsToExist(string from, string to)
        {
            if (!IsFromExist(from))
                return false;
            return _rateChangeTable[from].ContainsTo(to);
        }
        public Money Reduce(IExpression source,string to)
        {
            return source.Reduce(this,to);
        }
    }

   
}
