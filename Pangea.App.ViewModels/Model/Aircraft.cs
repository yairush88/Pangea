namespace Pangea.App.ViewModels.Model
{
    public class Aircraft
    {
        public Aircraft(string id, bool isLead)
        {
            Id = id;
            IsLead = isLead;
        }

        public string Id { get; set; }
        public bool IsLead { get; set; }
    }
}
