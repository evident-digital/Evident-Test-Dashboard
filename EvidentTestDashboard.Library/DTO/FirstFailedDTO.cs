namespace EvidentTestDashboard.Library.DTO
{
    public class FirstFailedDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Duration { get; set; }
        public string Href { get; set; }
        public int? BuildID
        {
            get
            {
                if(ID == null
                    || !ID.Contains("build:(id:"))
                {
                    return null;
                }

                var token = "build:(id:";
                var idString = ID.Substring(ID.IndexOf(token) + token.Length).Replace(")", string.Empty);
                int result;
                if(int.TryParse(idString, out result))
                {
                    return result;
                }

                return null;
            }
        }
    }
}