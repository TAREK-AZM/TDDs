namespace TDDByExample
{
    public class Pair
    {
        public string From { get; set; }
        public string To { get; set; }

        public int Rate { get; set; }

        public Pair(string from,string to,int rate) {
            From = from;
            To = to;
            Rate = rate;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null ) return false;
            if(obj is Pair pair)
            {
                return From.Equals(pair.From) && To.Equals(pair.To);
            }
            return false;

        }

        public override int GetHashCode()
        {
            return From.GetHashCode() + To.GetHashCode() ;
        }
    }

   
}
