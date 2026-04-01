namespace TDDByExample
{
    public static class PairesListExtension
    {
        public static Pair GetPairTo(this List<Pair> pairs,string to)
        {
            return pairs.Where(p => p.To== to).FirstOrDefault();
        }
        public static bool ContainsTo(this List<Pair> pairs,string to)
        {
            return pairs.Where(p=>p.To== to).Any();
        }
  
    }

   
}
